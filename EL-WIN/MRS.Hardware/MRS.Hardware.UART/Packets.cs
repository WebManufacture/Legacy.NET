﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MRS.Hardware.UART
{
    public enum UartCommand : byte
    {
        UNKNOWN = 0x00, //Data
        DATA = 0x20, //Data
        PORT = 0x40, //Port data
        ADC = 0x50, //ADC data
        PWM = 0x60, //PWM data
        BEEPER = 0x60, //beeper signal
        LED = 0x70 //LED signal
    }

    public enum UartSource : byte
    {
        UNKNOWN = 0x00, // no source
        EXT = 0xF0, // external far
        SCH = 0xE0, // schema
        MCU = 0x10, // mcu
        PCD = 0x20, //desktop PC
        PCP = 0x30, //portable PC
        PDA = 0x40, //PDA device
        Peripherial = 0x50,
        BusMCU = 0x60, //bused MCU
        Server = 0xA0, //server
        Other = 0x50
    }


    public class UartPacket
    {
        public UartCommand Command;
        public int Addr;
        public bool HasErrors = false;
        protected byte source;
        protected byte[] data;
        protected uint bytesCount = 0;

        public virtual int Address
        {
            get
            {
                return source & 0x0F;
            }
        }

        public UartPacket(byte command)
        {
            this.Command = (UartCommand) command;
            //Source = (UartSource)(source & 0xF0);
        }

        public virtual byte[] GetData()
        {
            return data;
        }

        public uint BytesCount
        {
            get
            {
                return bytesCount;
            }
        }

        public virtual bool ProcessData(byte[] data)
        {
            this.data = data;
            return true;
        }
    }

    public class ADCPacket : UartPacket
    {
        public int MaxValue = 1023;
        public int Value;
        public int Channel;

        public ADCPacket(byte[] data, byte command)
            : base(command)
        {
            Channel = command & 0x0F;
            bytesCount = 2;
        }

        public override bool ProcessData(byte[] data)
        {
            if (data.Length != 2) return false;
            Value = data[0] + data[1] * 256;
            return true;
        }

        public double GetAbsoluteValue(double vref)
        {
            return vref * Value / MaxValue;
        }

        public string ToString(double vref)
        {
            var FORMAT = "{2}-{3} {0:d4} ({1:F} V)";
            return string.Format(FORMAT, Value, GetAbsoluteValue(vref), DateTime.Now.ToShortDateString(), DateTime.Now.Millisecond);
        }
    }
}
