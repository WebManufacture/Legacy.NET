using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HLab.eBox
{
    enum UartCommand : byte
    {
        get = 1,
        set = 2,
        add = 3,
        del = 4,
        getDirty = 5,
        exec = 6,
        mmode = 10
    }

    public class Task
    {
        public const byte BIT_0 = 1;
        public const byte BIT_1 = 2;
        public const byte BIT_2 = 4;
        public const byte BIT_3 = 8;
        public const byte BIT_4 = 16;
        public const byte BIT_5 = 32;
        public const byte BIT_6 = 64;
        public const byte BIT_7 = 128;
        public byte flags;
        public byte value;
        public ushort start;
        public ushort Address;

        public Task()
        {

        }

        public Task(byte flags, byte value, ushort start)
        {
            this.flags = flags;
            this.value = value;
            this.start = start;
        }

        public Task(uint dta)
        {
            flags = (byte)(dta >> 24);
            value = (byte)(dta >> 16);
            start = (ushort)(dta);
        }


        public Task(byte[] dta) : this(dta, 0)
        {
            
        }

        public Task(byte[] dta, int index)
        {
            flags = dta[index + 0];
            value = dta[index + 1];
            start = (ushort)(dta[index + 2] * 256 + dta[index + 3]);
        }

        public bool IsActive
        {
            get
            {
                return (flags & BIT_7) > 0;
            }
            set{
                if (value){
                    flags |= BIT_7;
                }
                else{
                    flags &= 255 - BIT_7;
                }
            }
        }

        
	    public byte TaskType{
            get{
                return (byte)(flags & (BIT_6 + BIT_5) >> 5);
            }
            set{
                if (value <= 3){
                    flags &= 255 - (BIT_6 + BIT_5);
                    flags |= (byte)(value << 5);
                }
            }
        }


        public byte Port
        {
            get
            {
                return (byte)(flags & 31);
            }
            set
            {
                if (value <= 31)
                {
                    flags &= 255 - 31;
                    flags |= value;
                }
            }
        }
        
        public byte[] GetBytes()
        {
            byte[] dta = new byte[4];
            dta[0] = flags;
            dta[1] = value;
            dta[2] = (byte)(start >> 8);
            dta[3] = (byte)(start); 
            return dta;
        }
        
        public TimeSpan Time
        {
            get
            {
                byte hours = (byte)(start / 3600);
                byte minutes = (byte)(start / 60 - hours * 60);
                byte seconds = (byte)(start - minutes*60 - hours*3600);
                return new TimeSpan(0, hours, minutes, seconds, 0);
            }
            set
            {
                start = (ushort)(value.Hours * 60 * 60 + value.Minutes * 60 + value.Seconds);
            }
        }
    }
 
}
