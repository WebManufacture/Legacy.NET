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
        public uint Speed;
        public byte DataBits = 8;
        public Parity Parity = Parity.None;
        public StopBits StopBits = StopBits.One;
        public PacketType RxPacketType = PacketType.Sized;
        public PacketType TxPacketType = PacketType.Sized;
        public byte ReceiverCRC = 231;
        public byte TransmitterCRC = 231;
        public byte DefaultDeviceAddr = 00;

        public SerialConfig()
        {

        }

        public SerialConfig(string portName)
        {
            PortName = portName;
        }

        public override string ToString()
        {
            return DeviceName + " " + PortName + " " + Speed + " (" + DataBits + ", " + Parity + ", " + StopBits + ") RX:" + RxPacketType + " TX:" + TxPacketType;
        }

        public static SerialConfig Parse(string cfg)
        {
            var parts = cfg.Split(new Char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            var res = new SerialConfig();
            int index = 0;
            if (!parts[index].StartsWith("COM"))
            {
                res.DeviceName = parts[index];
                index++;
            }
            res.PortName = parts[index]; index++;
            res.Speed = UInt32.Parse(parts[index]); index++;
            res.DataBits = Byte.Parse(parts[index]); index++;
            res.Parity = (Parity)Byte.Parse(parts[index]); index++;
            res.StopBits = (StopBits)Byte.Parse(parts[index]); index++;
            res.RxPacketType = (PacketType)Byte.Parse(parts[index]); index++;
            res.TxPacketType = (PacketType)Byte.Parse(parts[index]); index++;
            if (parts.Length > index)
            {
                res.ReceiverCRC = Byte.Parse(parts[index]); index++;
                res.TransmitterCRC = Byte.Parse(parts[index]); index++;
            }
            if (parts.Length > index)
            {
                res.DefaultDeviceAddr = Byte.Parse(parts[index]); index++;
            }
            return res;
        }
    }


}
