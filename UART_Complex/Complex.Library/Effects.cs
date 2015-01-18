using System.ComponentModel;
using System;
using System.Threading;
using MRS.Hardware.UART;

namespace MRS.Hardware.UI.Library
{
    public class Effects
    {
        private Thread worker;
        private BitBangManager Manager;

        public Effects(BitBangManager manager)
        {
            Manager = manager;
            //Manager.OnError += new FTDIErrorHandler(Manager_OnError);
        }

        bool Manager_OnError(string error)
        {
            if (worker != null)
                worker.Abort();
            return false;
        }

        public void Stop()
        {
            if (worker != null)
                worker.Abort();
            Reset();
        }

        public void Random()
        {
            worker = new Thread(RandomShow);
            worker.Start();
        }

        public void Shim(double delay)
        {
            worker = new Thread(Pwm);
            worker.Start(delay);
        }

        public void Reader()
        {
            worker = new Thread(Read);
            worker.Priority = ThreadPriority.Highest;
            worker.Start();
        }

        protected void RandomShow()
        {
            Random rnd = new Random();
            while (Thread.CurrentThread.ThreadState != ThreadState.AbortRequested)
            {
               // Manager.WriteByte((byte)rnd.Next(0, 256));
            }
        }


        protected void Pwm(object pwm)
        {
            double step = 400;
            //string prgramm = (string) pwm;
            //string[] values = 
            //byte[] values = new byte[]
            //                    {
            //                        1, 2, 4, 16, 64,
            //                        3, 7, 6, 5,
            //                        17, 18, 20, 19, 21, 22, 23,
            //                        64 + 16, 64 + 4, 64 + 1, 64 + 2,
            //                        64 + 3, 64 + 7, 64 + 6, 64 + 5,
            //                        64 + 17, 64 + 18, 64 + 20, 64 + 19, 64 + 21, 64 + 22, 64 + 23
            //                    };
            byte[] values = new byte[]
                                {
                                    1, 2, 4, 16, 64
                                };
            int lastIndex = 0;
            Random rnd = new Random();
            while (Thread.CurrentThread.ThreadState != ThreadState.AbortRequested)
            {
                int newIndex = rnd.Next(0, values.Length);
                PwmCicle(step, values[newIndex], values[lastIndex]);
                lastIndex = newIndex;
            }
        }

        protected void PwmCicle(double pwm, byte himask, byte lomask)
        {
            double step = 1 / pwm;
            for (double i = 0; i <= 1; i += step)
            {
                if (Thread.CurrentThread.ThreadState == ThreadState.AbortRequested)
                    return;
                //Manager.Pwm(himask, lomask, i);
            }
        }

        protected void Run()
        {
            int delay = 400;
            //string prgramm = (string) pwm;
            //string[] values = 
            //byte[] values = new byte[]
            //                    {
            //                        1, 2, 4, 16, 64,
            //                        3, 7, 6, 5,
            //                        17, 18, 20, 19, 21, 22, 23,
            //                        64 + 16, 64 + 4, 64 + 1, 64 + 2,
            //                        64 + 3, 64 + 7, 64 + 6, 64 + 5,
            //                        64 + 17, 64 + 18, 64 + 20, 64 + 19, 64 + 21, 64 + 22, 64 + 23
            //                    };
            byte[] values = new byte[]
                                {
                                    1, 2, 4, 8, 16, 32, 64, 128
                                };
            while (Thread.CurrentThread.ThreadState != ThreadState.AbortRequested)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    //Manager.WriteByte(values[i]);
                    Thread.Sleep(delay);
                }
            }
        }

        protected void Read()
        {
            while (Thread.CurrentThread.ThreadState != ThreadState.AbortRequested)
            {
                //;Manager.ReadByte();
            }
        }

        public void Reset()
        {
           // Manager.WriteByte(0);
        }

        public void Full()
        {
            //Manager.WriteByte(255);
        }

        public void Running()
        {
            worker = new Thread(Run);
            worker.Start();
        }
    }
}