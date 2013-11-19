using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LPTStepMotor;

namespace StepMotorController
{
    public partial class Form1 : Form
    {
        private LptPortAdapter adapter;

        public Form1()
        {
            InitializeComponent();
            adapter = new LptPortAdapter();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            lptTimer.Enabled = !lptTimer.Enabled;
        }

        private void lptTimer_Tick(object sender, EventArgs e)
        {
            lblPin1.BackColor = Color.White;
            lblPin2.BackColor = Color.White;
            switch (adapter.Value)
            {
                case 0:
                    lblPin1.BackColor = Color.Green;
                    adapter.Value = 1;
                    break;
                case 1:
                    lblPin2.BackColor = Color.Green;
                    adapter.Value = 2;
                    break;
                case 2:
                    lblPin1.BackColor = Color.Green;
                    lblPin2.BackColor = Color.Green;
                    adapter.Value = 3;
                    break;
                default:
                    adapter.Value = 0;
                    break;
            }
        }
    }
}
