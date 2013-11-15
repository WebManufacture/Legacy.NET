using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HLab.eBox
{
    
    public enum TaskTypes : byte
    {
        pin = 1,
        pwmUp = 2,
        pwmDown = 3,
        pwm = 4,
        beeper = 12,
        system = 13
    }

    public enum RunMode : byte
    {
        idle = 0,
        output = 1,
        processing = 2,
        reset = 3
    }

    public enum UartCommand : byte
    {
        get = 1,
        set = 2,
        task = 5,
        resume = 7,
        pause = 6,
        stop = 9,
        restart = 8,
        reset = 10
    }

    public class EBoxSettings
    {
        public const byte BIT_0 = 1;
        public const byte BIT_1 = 2;
        public const byte BIT_2 = 4;
        public const byte BIT_3 = 8;
        public const byte BIT_4 = 16;
        public const byte BIT_5 = 32;
        public const byte BIT_6 = 64;
        public const byte BIT_7 = 128;

        public byte param1 = 0;
        public byte param2 = 0;
        public byte param3 = 0;
        public byte settings;

        public EBoxSettings()
        {

        }

        public EBoxSettings(RunMode BeginMode, RunMode EndMode)
        {
            this.BeginMode = BeginMode;
            this.EndMode = EndMode;
        }

        public EBoxSettings(long bytes)
        {
            param1 = (byte)(bytes >> 24);
            param2 = (byte)(bytes >> 16);
            param3 = (byte)(bytes >> 8);
            settings = (byte)(bytes);
        }


        public EBoxSettings(byte[] dta)
            : this(dta, 0)
        {

        }

        public EBoxSettings(byte[] dta, int index)
        {
            param1 = dta[index + 0];
            param2 = dta[index + 1];
            param3 = dta[index + 2];
            settings = dta[index + 3];
        }
        
        public RunMode BeginMode
        {
            get
            {
                return (RunMode)((settings & (BIT_2 + BIT_3 + BIT_4)) >> 2);
            }
            set
            {
                settings &= 255 - (BIT_2 + BIT_3 + BIT_4);
                settings |= (byte)((byte)value << 2);  
            }
        }

        public RunMode EndMode
        {
            get
            {
                return (RunMode)((settings & (BIT_5 + BIT_6 + BIT_7)) >> 5);
            }
            set
            {
                settings &= 255 - (BIT_5 + BIT_6 + BIT_7);
                settings |= (byte)((byte)value << 5);                
            }
        }

        public byte[] GetBytes()
        {
            byte[] dta = new byte[4];
            dta[0] = param1;
            dta[1] = param2;
            dta[2] = param3;
            dta[3] = settings;
            return dta;
        }
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
        /*
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
        */

        public TaskTypes TaskType
        {
            get{
                return (TaskTypes)Type;
            }
            set{
                Type = (byte)value;
            }
        }

        public byte Type
        {
            get
            {
                return (byte)(flags & 7);
            }
            set
            {
                if (value <= 7)
                {
                    flags &= 248;
                    flags |= value;
                }
            }
        }


        public byte Port
        {
            get
            {
                return (byte)((flags & 248) >> 3);
            }
            set
            {
                if (value <= 31)
                {
                    flags &= 31;
                    flags |= (byte)((byte)value << 3);
                }
            }            
        }

        public byte PercentValue
        {
            get
            {
                var fval = (float)(value) / (float)255;
                return (byte)(Math.Round(fval * 100));
            }
            set
            {
                if (value <= 100)
                {
                    this.value = (byte)((float)value * 255 / 100);
                }
                else
                {
                    this.value = 255;
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
