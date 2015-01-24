using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;
using System.IO.Ports;

namespace MRS.Hardware.UART
{
    public class XPacket
    {
        public XPacket()
        {

        }

        public XPacket(byte[] data)
        {
            if (data.Length >= 1)
            {
                if (data.Length >= 2){
                    if (data.Length >= 4){
                        DestinationAddr = data[0];
                        DestinationType = data[1];
                        SourceAddr = data[2];
                        SourceType = data[3];
                        Data = new byte[data.Length - 4];
                        Array.Copy(data, 4, Data, 0, Data.Length);
                    }
                    else{
                        DestinationAddr = data[0];
                        DestinationType = data[1];
                        Data = new byte[data.Length - 2];
                        Array.Copy(data, 2, Data, 0, Data.Length);
                    }
                }
                else{
                    DestinationAddr = data[0];
                    Data = new byte[data.Length-1];
                    Array.Copy(data, 1, Data, 0, Data.Length);
                }
            }
            else{
                Data = data;
            }
        }

        public byte DestinationAddr;
        public byte DestinationType;
        public byte SourceAddr;
        public byte SourceType;

        public UInt16 Destination
        {
            get
            {
                return (UInt16)(((UInt16)DestinationAddr) << 8 + DestinationType);
            }
        }

        public UInt16 Source
        {
            get
            {
                return (UInt16)(((UInt16)SourceAddr) << 8 + SourceType);
            }
        }

        public UInt32 Mask
        {
            get
            {
                return (UInt32)(((UInt32)Destination) << 16 + Source);
            }
        }

        public int Length
        {
            get
            {
                return Data.Length;
            }
        }

        public byte[] Data = new byte[0];

        public byte[] ToBytes()
        {
            var buf = new byte[Data.Length + 4];
            buf[0] = DestinationAddr;
            buf[1] = DestinationType;
            buf[2] = SourceAddr;
            buf[3] = SourceType;
            Data.CopyTo(buf, 4);
            return buf;
        }

        public string ToFactoryRecord()
        {
            return Encoding.ASCII.GetString(Data);
        }
    }

    public class XDevice
    {
        public XDevice()
        {

        }

        public string FactoryRec;
        public byte Addr;
    }

    public delegate void OnReceiveXPacketHandler(XPacket packet, XRoutingManager manager);

    public class XRoutingManager : SerialPacketManager
    {
        public byte DeviceAddr = 254;
        public byte[] FactoryRec = Encoding.ASCII.GetBytes(".NET XRT");
        public bool ReceiveBroadcast = true;
        public OnReceiveXPacketHandler[] RoutingTable = new OnReceiveXPacketHandler[255];
        public List<XDevice> KnownDevices = new List<XDevice>();
        public event OnReceiveXPacketHandler OnReceiveXPacket;
        public byte LastDeviceAddr;
        
        public XRoutingManager(byte deviceAddr, bool receiveBroadcast) : base() {
            this.DeviceAddr = deviceAddr;
            this.ReceiveBroadcast = receiveBroadcast;
        }

        protected override void OnLoad()
        {
            RoutingTable[1] = _msg_routing_GiveFactoryNum; //ДРУГОЕ УСТРОЙСТВО РЕГИСТРИРУЕТСЯ
            RoutingTable[4] = _msg_routing_GiveFactoryNum;      //УСТРОЙСТВА знакомятся
            //RoutingTable[5] = _msg_routing_SendFactoryNum; //Исследование сети
            base.OnLoad();
        }

        public XRoutingManager(SerialPort port) : base(port) { }

        public XRoutingManager(SerialConfig config) : base(config) {
            receiverCRCValue = config.ReceiverCRC;
            transmitterCRCValue = config.TransmitterCRC;
            DeviceAddr = config.DefaultDeviceAddr;
        }

        public XRoutingManager(SerialPort port, SerialConfig config) : base(port, config) {
            receiverCRCValue = config.ReceiverCRC;
            transmitterCRCValue = config.TransmitterCRC;
            DeviceAddr = config.DefaultDeviceAddr;
        }

        public XRoutingManager(string portName) : base(portName) { }

        public XRoutingManager(string portName, int speed) : base(portName, speed) { }

        public XRoutingManager(string portName, int speed, int timeout) : base(portName, speed, timeout) { }

        public XRoutingManager(string portName, int speed, Parity parity, byte dataBits, StopBits stops, int timeout)
            : base(portName, speed, parity, dataBits, stops, timeout) { }

        public override bool Connect()
        {
            if (base.Connect())
            {
                SendLookPacket(); 
                return true;
            }
            return false;
        }

        protected override bool callReceive(byte[] data)
        {
            base.callReceive(data);
            if (data.Length < 4) return false;
            if (data[0] == DeviceAddr)
            {
                return processXPacket(new XPacket(data));
            }
            else
            {
                if (ReceiveBroadcast && data[0] == 0)
                {
                    return processXPacket(new XPacket(data));
                }
            }
            return false;
        }

        protected virtual bool processXPacket(XPacket packet)
        {
            if (RoutingTable[packet.DestinationType] != null)
            {
                RoutingTable[packet.DestinationType](packet, this);
            }
            if (OnReceiveXPacket != null)
            {
                OnReceiveXPacket(packet, this);
            }
            return true;
        }

        public override byte[] ReadData()
        {
            var data = base.ReadData();
            var buf = new byte[data.Length - 4];
            Array.Copy(data, 4, buf, 0, buf.Length);
            return buf;
        }

        public virtual bool Send(XPacket packet)
        {
            return base.Send(packet.ToBytes());
        }
        
        public override bool Send(byte addr, byte[]data)
        {
            return Send(new XPacket(){
                DestinationAddr = LastDeviceAddr,
                DestinationType = addr,
                SourceAddr = DeviceAddr,
                SourceType = 0,
                Data = data   
            });
        }

        public override bool Send(byte data)
        {
            return Send(new XPacket()
            {
                DestinationAddr = LastDeviceAddr,
                DestinationType = data,
                SourceAddr = DeviceAddr,
                SourceType = 0
            });
        }

        private void _msg_routing_GiveFactoryNum(XPacket packet, XRoutingManager manager)
        {
            if (packet.SourceAddr == 0 && packet.DestinationType == 1)
            {
                var fr = packet.ToFactoryRecord();
                XDevice device = KnownDevices.FirstOrDefault<XDevice>(xd => xd.FactoryRec == fr);
                if (device == null)
                {
                    device = new XDevice() { FactoryRec = fr };
                    byte addr = 01;
                    while (KnownDevices.FirstOrDefault<XDevice>(xd => xd.Addr == addr) != null)
                    {
                        addr++;
                        if (addr >= 255) throw new Exception("All addressed closed!");
                    }
                    device.Addr = addr;
                    KnownDevices.Add(device);
                }
                LastDeviceAddr = device.Addr;
                
                /*
                #dstAddr  00
                #dstType  02
                #srcAddr  254
                #srcType  A6
                FactoryNum
                */

                Send(new XPacket() { 
                    Data = packet.Data, 
                    SourceAddr = DeviceAddr, 
                    SourceType = device.Addr, 
                    DestinationType = 2 }
                );
                State = EDeviceState.Working;
            }
            if (packet.DestinationAddr == DeviceAddr && packet.DestinationType == 4)
            {
                XDevice device = KnownDevices.FirstOrDefault<XDevice>(xd => xd.Addr == packet.SourceAddr);
                if (device == null)
                {
                    device = new XDevice() { Addr = packet.SourceAddr, FactoryRec = packet.ToFactoryRecord() };
                    KnownDevices.Add(device);
                }
                if (packet.SourceAddr == 0)
                {
                    if (device.Addr == 0)
                    {
                        byte addr = 01;
                        while (KnownDevices.FirstOrDefault<XDevice>(xd => xd.Addr == addr) != null)
                        {
                            addr++;
                            if (addr >= 255) throw new Exception("All addressed closed!");
                        }
                        device.Addr = addr;
                    }

                    /*
                      #dstAddr  3F
                      #dstType  04
                      #srcAddr  23
                      #srcType  00
                      FactoryNum
                    */

                    Send(new XPacket()
                    {
                        Data = packet.Data,
                        SourceAddr = DeviceAddr,
                        SourceType = device.Addr,
                        DestinationType = 2
                    });
                }
                else
                {
                    if (packet.SourceAddr == DeviceAddr) DeviceAddr--;//ЕСЛИ В СЕТИ УЖЕ ЕСТЬ УСТРОЙСТВО С ТАКИМ АДРЕСОМ
                }
                LastDeviceAddr = device.Addr;
                State = EDeviceState.Working;
            }
            if (packet.DestinationType == 5)
            {
                Send(new XPacket()
                {
                    Data = FactoryRec,
                    SourceAddr = DeviceAddr,
                    SourceType = 05,
                    DestinationAddr = packet.DestinationAddr,
                    DestinationType = 4
                });
            }
        }

        public void SendLookPacket()
        {
            /*
              #dstAddr  00
              #dstType  05
              #srcAddr  3F
              #srcType  00
              FactoryNum
            */

            Send(new XPacket()
               {
                   Data = FactoryRec,
                   SourceAddr = DeviceAddr,
                   SourceType = 00,
                   DestinationAddr = 0,
                   DestinationType = 5
               });
        }

    }
}
