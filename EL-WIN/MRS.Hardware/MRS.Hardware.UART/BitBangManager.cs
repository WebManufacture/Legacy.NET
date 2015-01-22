using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FTDIDriver = FTD2XX_NET.FTDI;

namespace MRS.Hardware.UART
{
    public class BitBangManager : FtdiManager
    {
        FTDIDriver Chip;

        public BitBangManager() : base()
        {
            SetBitMode(255);
        }

        public BitBangManager(uint index) : base(index)
        {
            SetBitMode(255);
        }

        public void SetBitMode(byte mask)
        {
            FTDIDriver.FT_STATUS ftStatus = Chip.SetBitMode(0, FTDIDriver.FT_BIT_MODES.FT_BIT_MODE_RESET);
            CheckError("Failed to reset bit bang.", ftStatus);
            ftStatus = Chip.SetBitMode(mask, FTDIDriver.FT_BIT_MODES.FT_BIT_MODE_ASYNC_BITBANG);
            CheckError("Failed to set bit bang.", ftStatus);
        }

        public byte ReadByte()
        {   
            byte bytes = 0;
            FTDIDriver.FT_STATUS ftStatus = Chip.GetPinStates(ref bytes);
            //FireOnRead(bytes);
            CheckError("Failed to read from device", ftStatus);
            return bytes;
        } 


        public uint Pwm(byte mask, byte loMask, double value)
        {
            int size = 100;
            byte[] dataToWrite = new byte[size];
            UInt32 numBytesWritten = 0;

            if (value > 1)
            {
                value = value - (int)value;
            }
            int switchElem = (int)Math.Round(size * value);
            for (int j = 0; j < switchElem; j++)
            {
                dataToWrite[j] = mask;
            }
            for (int j = switchElem; j < size; j++)
            {
                dataToWrite[j] = loMask;
            }
            FTDIDriver.FT_STATUS status = Chip.Write(dataToWrite, size, ref numBytesWritten);
            CheckError("Status incorrect!", status);
            if (numBytesWritten != size)
            {
                Error("Checksum fault: " + size + " != " + numBytesWritten, status);
            }
            return numBytesWritten;
        }

    }
}
