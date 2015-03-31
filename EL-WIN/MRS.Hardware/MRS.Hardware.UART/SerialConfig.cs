using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;

namespace MRS.Hardware.UART
{
    public class SerialConfig
    {
        public string DeviceName = "";
        public string PortName;
        public bool AutoConnect;
        public uint Speed = 115200;
        public byte DataBits = 8;
        public Parity Parity = Parity.None;
        public StopBits StopBits = StopBits.One;
        public PacketType PacketType = PacketType.Raw;
        public PacketType RxPacketType = PacketType.Sized;
        public PacketType TxPacketType = PacketType.Sized;
        public byte ReceiverCRC = 231;
        public byte TransmitterCRC = 231;
        public byte DefaultDeviceAddr = 00;
        public string initialConfig;

        public SerialConfig()
        {

        }
        
        public SerialConfig(string cfg)
        {
            var parts = cfg.Split(new Char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            this.initialConfig = cfg;
            int index = 0;
            if (!parts[index].StartsWith("COM"))
            {
                this.DeviceName = parts[index];
                index++;
            }
            this.PortName = parts[index]; index++;
            this.AutoConnect = Boolean.Parse(parts[index]); index++;
            this.Speed = UInt32.Parse(parts[index]); index++;
            this.DataBits = Byte.Parse(parts[index]); index++;
            this.Parity = (Parity)Byte.Parse(parts[index]); index++;
            this.StopBits = (StopBits)Byte.Parse(parts[index]); index++;
            //this.PacketType = (PacketType)Byte.Parse(parts[index]); index++;
            this.RxPacketType = (PacketType)Byte.Parse(parts[index]); index++;
            this.TxPacketType = (PacketType)Byte.Parse(parts[index]); index++;
            this.PacketType = this.RxPacketType;
            if (parts.Length > index)
            {
                this.ReceiverCRC = Byte.Parse(parts[index]); index++;
                this.TransmitterCRC = Byte.Parse(parts[index]); index++;
            }
            if (parts.Length > index)
            {
                this.DefaultDeviceAddr = Byte.Parse(parts[index]); index++;
            }
        }

        public override string ToString()
        {
            return DeviceName + " " + PortName + " " + Speed + " (" + DataBits + ", " + Parity + ", " + StopBits + ") RX:" + RxPacketType + " TX:" + TxPacketType;
        }

        public static SerialConfig Parse(string cfg)
        {
            return new SerialConfig(cfg);
        }

    }


}
