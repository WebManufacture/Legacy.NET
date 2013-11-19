using System;
using System.Runtime.InteropServices;

namespace LPTStepMotor
{
    public class LptPortAdapter
    {
        public const int portAddress = 0x378; // Base Port Address

        [DllImport("inpout32.dll", EntryPoint = "Out32")]
        protected static extern void Output(int adress, int value);

        protected int address;
        private byte portData;

        public LptPortAdapter()
            : this(0)
        {

        }

        public LptPortAdapter(int PortNumber)
        {
            address = portAddress + PortNumber;
        }

        public void Out(byte value)
        {
            portData = value;
            try
            {
                Output(portAddress, value);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public byte Value
        {
            get
            {
                return portData;
            }
            set
            {
                Out(value);
            }
        }
    }
}