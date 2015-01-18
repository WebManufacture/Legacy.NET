using System.Threading;

namespace MRS.Hardware.UI.Library
{
    public enum FTStatus
    {
        Sended, Received, Error, Complete
    }

    public delegate void StatsAsyncDelegate(byte value);
    public delegate void StatsEventDelegate(FTStatus status);

    public class Stats
    {
        private Thread stats;

        public event StatsAsyncDelegate OnGetStats;

        public Stats()
        {
            stats = new Thread(GetStats);
        }

        public void Start()
        {
            stats.Start();
        }

        public void Reset()
        {
            stats.Abort();
        }

        protected void GetStats()
        {
            while (Thread.CurrentThread.ThreadState != ThreadState.AbortRequested)
            {
                if (OnGetStats != null)
                {
                   // OnGetStats(Manager.Read());
                }
            }
        }

        public void Stop()
        {
            Reset();
        }
    }
}