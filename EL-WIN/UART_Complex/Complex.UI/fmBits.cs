using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MRS.Hardware.UI.Library;
using MRS.Hardware.UART;

namespace MRS.Hardware.UI.Analyzer
{
    public partial class fmBits : Form
    {
        private BitBangManager manager;
        private Effects effects;

        public fmBits()
        {
            InitializeComponent();
            manager = new BitBangManager();
            //manager.OnError += new OnErrorHandler(Manager_OnError);
//effects = new Effects(manager);
        }

        public fmBits(uint index)
        {
            InitializeComponent();
            manager = new BitBangManager(index);
            //manager.OnError += new OnErrorHandler(Manager_OnError);
            effects = new Effects(manager);
        }

        protected override void OnClosed(EventArgs e)
        {
            effects.Stop();
            manager.Close();
            base.OnClosed(e);
        }

        bool Manager_OnError(string error)
        {
            Program.LogError(error);
            return true;
        }


        private void Random_Click(object sender, EventArgs e)
        {
            effects.Random();
        }

        private void Shim_Click(object sender, EventArgs e)
        {
            byte delay;
            if (byte.TryParse(txtDelay.Text, out delay))
            {
                effects.Shim(delay);
            }
            else
            {
                effects.Shim(0);
            }
        }

        private void Stop_Click(object sender, EventArgs e)
        {
            effects.Stop();
        }

        private void Reset_Click(object sender, EventArgs e)
        {
            effects.Reset();
        }

        private void SetAll_Click(object sender, EventArgs e)
        {
            effects.Full();
        }

        private void running_Click(object sender, EventArgs e)
        {
            effects.Running();
        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            byte delay;
            if (byte.TryParse(txtDelay.Text, out delay))
            {
                manager.Send(delay);
            }
        }

        private void btnRead_Click(object sender, EventArgs e)
        {
            byte delay = manager.ReadByte();
            txtDelay.Text = delay.ToString();
        }

        void Manager_OnRead(byte value)
        {
            //data = value;
        }
    }
}
