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
    public enum PacketReadingState
    {
        free,
        sizeWaiting,
        dataReading,
        crcCheck,
        finishCheck
    }

    public class SerialPacketManager : SerialManager
    {
        public const byte PACKET_SIZED_START_BYTE = (byte)ASCII.SOH;
        public const byte PACKET_UNSIZED_START_BYTE = (byte)ASCII.STX;
        public const byte PACKET_END_TEXT = (byte)ASCII.ETX;
        public const byte PACKET_END_TRANSMIT = (byte)ASCII.EOT;
        public const int MAX_PACKET_DATA_SIZE = 256 * 256;
        protected byte receiverStart = 0;
        protected byte transmitterStart = PACKET_SIZED_START_BYTE;
        protected byte receiverEnd = PACKET_END_TEXT;
        protected byte transmitterEnd = PACKET_END_TEXT;
        protected byte? receiverCRCValue = null;
        protected byte? transmitterCRCValue = null;

        new protected event OnReceiveByteHandler OnReceiveByte;

        protected override void OnLoad()
        {
            base.OnReceiveByte += onReceiveByte;
            base.OnLoad();
        }
                
        protected SerialPacketManager() : base() {
            
        }

        public SerialPacketManager(SerialPort port) : base(port) {
           
        }

        public SerialPacketManager(SerialPort port, SerialConfig config) : this(port)
        {
            if (config.RxPacketType == PacketType.SimpleCRC || config.RxPacketType == PacketType.SizedCRC || config.RxPacketType == PacketType.SizedCRC_old)
            {
                receiverCRCValue = config.ReceiverCRC;
            }
            if (config.TxPacketType == PacketType.SimpleCRC || config.TxPacketType == PacketType.SizedCRC || config.TxPacketType == PacketType.SizedCRC_old)
            {
                transmitterCRCValue = config.TransmitterCRC;
            }
            if (config.RxPacketType == PacketType.SizedOld || config.RxPacketType == PacketType.SizedCRC_old)
            {
                receiverEnd = PACKET_END_TRANSMIT;
            }
            if (config.TxPacketType == PacketType.SizedOld || config.TxPacketType == PacketType.SizedCRC_old)
            {
                transmitterEnd = PACKET_END_TRANSMIT;
            }
            if (config.RxPacketType == PacketType.Simple || config.RxPacketType == PacketType.SimpleCoded || config.RxPacketType == PacketType.SimpleCRC)
            {
                receiverStart = PACKET_UNSIZED_START_BYTE;
                receiverEnd = PACKET_END_TEXT;
            }
            if (config.TxPacketType == PacketType.Simple || config.TxPacketType == PacketType.SimpleCoded || config.TxPacketType == PacketType.SimpleCRC)
            {
                transmitterStart = PACKET_UNSIZED_START_BYTE;
                transmitterEnd = PACKET_END_TEXT;
            }
        }

        public SerialPacketManager(SerialConfig config) : base(config)
        {
            if (config.RxPacketType == PacketType.SimpleCRC || config.RxPacketType == PacketType.SizedCRC || config.RxPacketType == PacketType.SizedCRC_old)
            {
                receiverCRCValue = config.ReceiverCRC;
            }
            if (config.TxPacketType == PacketType.SimpleCRC || config.TxPacketType == PacketType.SizedCRC || config.TxPacketType == PacketType.SizedCRC_old)
            {
                transmitterCRCValue = config.TransmitterCRC;
            }
            if (config.RxPacketType == PacketType.SizedOld || config.RxPacketType == PacketType.SizedCRC_old)
            {
                receiverEnd = PACKET_END_TRANSMIT;
            }
            if (config.TxPacketType == PacketType.SizedOld || config.TxPacketType == PacketType.SizedCRC_old)
            {
                transmitterEnd = PACKET_END_TRANSMIT;
            }
            if (config.RxPacketType == PacketType.Simple || config.RxPacketType == PacketType.SimpleCoded || config.RxPacketType == PacketType.SimpleCRC)
            {
                receiverStart = PACKET_UNSIZED_START_BYTE;
                receiverEnd = PACKET_END_TEXT;
            }
            if (config.TxPacketType == PacketType.Simple || config.TxPacketType == PacketType.SimpleCoded || config.TxPacketType == PacketType.SimpleCRC)
            {
                transmitterStart = PACKET_UNSIZED_START_BYTE;
                transmitterEnd = PACKET_END_TEXT;
            }
        }

        public SerialPacketManager(string portName) : base(portName) {
           
        }

        public SerialPacketManager(string portName, int speed) : base(portName, speed) {
            
        }

        public SerialPacketManager(string portName, int speed, int timeout) : base(portName, speed, timeout) {
            
        }

        public SerialPacketManager(string portName, int speed, Parity parity, byte dataBits, StopBits stops, int timeout)
            : base(portName, speed, parity, dataBits, stops, timeout) {
        }

        protected PacketReadingState packetState = PacketReadingState.free;
        private byte[] receivingBuf;
        private int readingIndex;
        private byte crc;
        
        protected void onReceiveByte(byte data, SerialManager sm)
        {
            if (State < EDeviceState.Online) State = EDeviceState.Online;
            switch (packetState)
            {
                case PacketReadingState.free:
                    if (data == receiverStart || receiverStart == 0)
                    {
                        if (receiverCRCValue.HasValue)
                        {
                            crc = receiverCRCValue.Value;
                        }
                        readingIndex = 0;
                        if (data == PACKET_UNSIZED_START_BYTE)
                        {
                            packetState = PacketReadingState.dataReading;
                            receivingBuf = new byte[MAX_PACKET_DATA_SIZE];
                            return;
                        }
                        if (data == PACKET_SIZED_START_BYTE)
                        {
                            packetState = PacketReadingState.sizeWaiting;
                            return;
                        }
                    }
                    break;
                case PacketReadingState.sizeWaiting:
                    if (data <= 0)
                    {
                        packetState = PacketReadingState.free;
                    }
                    else
                    {
                        receivingBuf = new byte[data];
                        if (data > 0){                        
                            packetState = PacketReadingState.dataReading;
                        }
                        else{
                            packetState = PacketReadingState.finishCheck;
                        }
                    }
                    break;
                case PacketReadingState.dataReading:                    
                    if ((receiverStart == PACKET_UNSIZED_START_BYTE) && data == PACKET_END_TEXT){
                        var buf = new byte[readingIndex + 1];
                        Array.Copy(receivingBuf, 0, buf, 0, buf.Length);
                        receivingBuf = buf;
                        packetState = receiverCRCValue.HasValue ? PacketReadingState.crcCheck : PacketReadingState.finishCheck;                        
                        return;
                    }
                    receivingBuf[readingIndex] = data;
                    crc ^= data;
                    readingIndex++;   
                    if (readingIndex >= receivingBuf.Length)
                    {
                        packetState = receiverCRCValue.HasValue ? PacketReadingState.crcCheck : PacketReadingState.finishCheck;                        
                    }
                    break;
                case PacketReadingState.crcCheck:
                        if (data == crc)
                        {
                            packetState = PacketReadingState.finishCheck;
                        }
                        else
                        {
                            callError(new Exception("Wrong CRC sum!"));
                        }
                        break;
                case PacketReadingState.finishCheck:
                        packetState = PacketReadingState.free;
                        if ((receiverEnd > 0 && data == receiverEnd) || (receiverEnd == 0 && (data == PACKET_END_TRANSMIT || data == PACKET_END_TEXT))){
                            callReceive(receivingBuf);
                        }
                        else
                        {
                            callError(new Exception("Incorrect finishing byte!"));
                        }
                    break;
            }
        }

        public override byte[] ReadData()
        {
            if (device == null) return null;
            int data =  0;
            try
            {
                while (data != PACKET_SIZED_START_BYTE)
                {
                    data = device.ReadByte();
                }
                do
                {
                    data = device.ReadByte();
                } while (data < 0);
            }
            catch (TimeoutException)
            {
                return null;
            }
            var buf = new byte[data];
            for (var i = 0; i < buf.Length; i++)
            {
                do
                {
                    data = device.ReadByte();
                } while (data < 0);
                buf[i] = (byte)data;
            }
            while (data != PACKET_END_TEXT && data != PACKET_END_TRANSMIT)
            {
                data = device.ReadByte();
            }
            return buf;
        }

        new protected byte[] ReadData(uint bytes)
        {
            return null;
        }

        new protected byte[] ReadData(int bytes)
        {
            return null;
        }

        new protected int ReadByte()
        {
            return base.ReadByte();
        }

        new protected string ReadString()
        {
            return base.ReadString();
        }

        public override bool Send(string data){        
            return Send(Encoding.ASCII.GetBytes(data));
        }
        
        public override bool Send(byte data)
        {
            return Send(new byte[] { data });
        }

        public virtual bool Send(byte[] data1, byte[] data2)
        {
            var buf = new byte[data1.Length + data2.Length];
            data1.CopyTo(buf, 0);
            data2.CopyTo(buf, data1.Length);
            return Send(buf) ;
        }

        public virtual bool Send(byte command, byte[] buffer)
        {
            return this.Send(new byte[] { command }, buffer);
        }

        public override bool Send(byte[] buffer)
        {
            if (buffer.Length > 255)
            {
                return SendPacket(buffer, 2, transmitterCRCValue.HasValue);
            }
            else
            {
                return SendPacket(buffer, 1, transmitterCRCValue.HasValue);
            }
        }

        public virtual bool SendPacket(byte[] buffer, byte sizeByteLength, bool useCRC)
        {
            if (device == null) return false;
            if (writeState > UARTWritingState.free) return false;
            if (buffer.Length > 255 || buffer.Length == 0) return false;
            if (!device.IsOpen)
            {
                return false;
            }
            setWriteState(UARTWritingState.writing);
            var bufAddLen = 2;
            bufAddLen += sizeByteLength;
            if (useCRC) bufAddLen += 1;
            var buf = new byte[buffer.Length + bufAddLen];
            buf[0] = sizeByteLength > 0 ? PACKET_SIZED_START_BYTE : PACKET_UNSIZED_START_BYTE;
            if (sizeByteLength > 0)
            {
                buf[sizeByteLength] = (byte)buffer.Length;
            }
            buffer.CopyTo(buf, 1 + sizeByteLength);
            if (useCRC)
            {
                byte crc = transmitterCRCValue.Value;
                for (int i = 0; i < buffer.Length; i++)
                {
                    crc ^= buffer[i];
                }
                buf[buf.Length - 2] = crc;
            }
            buf[buf.Length - 1] = transmitterEnd;
            device.Write(buf, 0, buf.Length);
            callSend(buffer);
            setWriteState(UARTWritingState.free);
            return true;
        }

        
    }
}
