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
    public class SerialAddressedManager : SerialPacketManager
    {
        public byte DeviceAddr = 00;
        public bool ReceiveBroadcast = true;

        protected SerialAddressedManager(byte deviceAddr, bool receiveBroadcast) : base() {
            this.DeviceAddr = deviceAddr;
            this.ReceiveBroadcast = receiveBroadcast;
        }
        
        public SerialAddressedManager(SerialPort port) : base(port) { }

        public SerialAddressedManager(SerialConfig config) : base(config) { }

        public SerialAddressedManager(string portName) : base(portName) { }

        public SerialAddressedManager(string portName, int speed) : base(portName, speed) { }

        public SerialAddressedManager(string portName, int speed, int timeout) : base(portName, speed, timeout) { }

        public SerialAddressedManager(string portName, int speed, Parity parity, byte dataBits, StopBits stops, int timeout)
            : base(portName, speed, parity, dataBits, stops, timeout) { }

        protected override bool callReceive(byte[] data)
        {
            if (data.Length < 1) return false;
            if (data[0] == DeviceAddr)
            {
                return base.callReceive(data);
            }
            else
            {
                if (ReceiveBroadcast && data[0] == 0)
                {
                    return base.callReceive(data);
                }
            }
            return false;
        }
        
        public virtual bool SendAddressed(byte addr, byte[] data)
        {
            var buf = new byte[data.Length + 1];
            buf[0] = addr;
            data.CopyTo(buf, 1);
            return Send(buf);
        }

        public virtual bool SendAddressed(int addr, byte[] data)
        {
            var buf = new byte[data.Length + 2];
            buf[0] = (byte)(addr >> 8);
            buf[1] = (byte)(addr);
            data.CopyTo(buf, 2);
            return Send(buf);
        }

        public virtual bool SendAddressed(int addr, byte command, byte[] data)
        {
            var buf = new byte[data.Length + 3];
            buf[0] = (byte)(addr >> 8);
            buf[1] = (byte)(addr);
            buf[2] = (byte)(command);
            data.CopyTo(buf, 3);
            return Send(buf);
        }

        public virtual bool SendAddressed(int addr, byte command)
        {
            return Send(new byte[] { (byte)(addr >> 8), (byte)addr, command });
        }

        new protected bool Send(byte data)
        {
            return base.Send(data);
        }
        
        new protected bool SendPacket(byte[] buffer, byte sizeByteLength, bool useCRC)
        {
            return base.SendPacket(buffer, sizeByteLength, useCRC);
        }

        public override bool Send(byte[] buffer)
        {
            if (buffer.Length < 1) return false;
            return base.Send(buffer);
        }

        public override bool Send(string data)
        {
            return base.Send('\0' + data);
        }
    }
}
