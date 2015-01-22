using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;

namespace MRS.Hardware.UART
{

    public enum PacketType : byte
    {
        Raw = 0,
        Simple = 1,      //UNSIZED  #02 ... data ... #03
        SimpleCoded = 2, //UNSIZED  coded by 2bytes ($3D = #F3 #FD) data      #02 ... coded data ... #03
        SimpleCRC = 3,   //UNSIZED WITH CRC  #02 ... data ... #crc #03
        PacketInvariant = 5,// CAN BE 2, 3, 10, 11, 14, 15
        Sized = 10,      //SIZED  #01 #size ... data ... #03
        SizedOld = 11,   //SIZED  #01 #size ... data ... #04
        SizedCRC = 14,   //SIZED  #01 #size ... data ... #CRC #03
        SizedCRC_old = 15,   //SIZED  #01 #size ... data ... #CRC #04
        Addressed = 20,  //With addr  #01 #size #addr ... data ... #03
        AddressedOld = 21,  //With addr  #01 #size #addr ... data ... #04
        XRouting = 30    //XRouting addressed  #01 #size #dstAddr #dstType #srcAddr #srcType ... data ... #crc #03
    }

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
