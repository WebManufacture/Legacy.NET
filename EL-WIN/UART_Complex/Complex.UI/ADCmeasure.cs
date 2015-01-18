using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MRS.Hardware.UI.Analyzer
{
    public class ADCmeasure
    {
        public static Single AdcSourceVoltage = (Single)5.0;
        public static int AdcMaxValue = 1024;

        public ADCmeasure()
        {

        }

        public ADCmeasure(byte Channel) : this(Channel, 0, 0, 0, 0)
        {

        }

        public ADCmeasure(byte Channel, int now, Single avg, int min, int max)
        {
            this.channel = Channel;
            this.dAvg = avg;
            this.dNow = now;
            this.dMax = max;
            this.dMin = min;
            this.avg = ToVoltage(avg);
            this.now = ToVoltage(now, 2);
            this.min = ToVoltage(min);
            this.max = ToVoltage(max);
        }


        public Single ToVoltage(Single value, byte digits)
        {
            return (Single)Math.Round(value * ADCmeasure.AdcSourceVoltage / AdcMaxValue, digits);
        }

        public Single ToVoltage(Single value)
        {
            return this.ToVoltage(value, 3);
        }

        public byte Channel
        {
            get
            {
                return channel;
            }
        }

        public Single Voltage
        {
            get
            {
                return (Single)Math.Round(avg, 1);
            }
        }

        public Single Avg
        {
            get
            {
                return avg;
            }
        }

        public Single Min
        {
            get
            {
                return min;
            }
        }

        public Single Max
        {
            get
            {
                return max;
            }
        }

        public Single Now
        {
            get
            {
                return now;
            }
        }

        public int DraftNow
        {
            get
            {
                return dNow;
            }
        }

        public int DraftAvg
        {
            get
            {
                return (int)dAvg;
            }
        }

        protected byte channel;
        public Single now;
        public Single avg;
        public Single min;
        public Single max;
        protected Single dAvg;
        protected int dMin;
        protected int dMax;
        protected int dNow;
    }
}
