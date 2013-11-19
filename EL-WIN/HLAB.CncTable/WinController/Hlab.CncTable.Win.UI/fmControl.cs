using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MRS.Hardware.Server;

namespace Hlab.CncTable.Win.UI
{
    public partial class fmControl : Form
    {
        protected int step = 100;
        protected ushort speedX = 1000;
        protected ushort speedY = 1000;
        protected ushort speedZ = 1000;

        public fmControl()
        {
            InitializeComponent();
        }

        private void fmControl_Load(object sender, EventArgs e)
        {
            speedX = (ushort)numXSpeed.Value;
            speedY = (ushort)numYSpeed.Value;
            speedZ = (ushort)numZSpeed.Value;
        }

        private void fmControl_KeyPress(object sender, KeyPressEventArgs e)
        {
           
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            if (CncController.LastState != null && !CncController.InMove && CncController.Ready)
            {
                var command = new CoordMotorCommand(MRS.Hardware.Server.CommandType.Go);
                command.X = CncController.LastState.x + step;
                command.Y = CncController.LastState.y;
                command.Z = CncController.LastState.z;
                command.Speed = speedX;
                CncController.SendCoordCommand(command);
            }
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            if (CncController.LastState != null && !CncController.InMove && CncController.Ready)
            {
                var command = new CoordMotorCommand(MRS.Hardware.Server.CommandType.Go);
                command.X = CncController.LastState.x - step;
                command.Y = CncController.LastState.y;
                command.Z = CncController.LastState.z;
                command.Speed = speedX;
                CncController.SendCoordCommand(command);
            }
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            if (CncController.LastState != null && !CncController.InMove && CncController.Ready)
            {
                var command = new CoordMotorCommand(MRS.Hardware.Server.CommandType.Go);
                command.X = CncController.LastState.x;
                command.Y = CncController.LastState.y + step;
                command.Z = CncController.LastState.z;
                command.Speed = speedY;
                CncController.SendCoordCommand(command);
            }
        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            if (CncController.LastState != null && !CncController.InMove && CncController.Ready)
            {
                var command = new CoordMotorCommand(MRS.Hardware.Server.CommandType.Go);
                command.X = CncController.LastState.x;
                command.Y = CncController.LastState.y - step;
                command.Z = CncController.LastState.z;
                command.Speed = speedY;
                CncController.SendCoordCommand(command);
            }
        }

        private void fmControl_KeyDown(object sender, KeyEventArgs e)
        {
            step = 100;
            if (e.Shift)
            {
                step = 1000;
                lblSteps.Text = step.ToString();
            }
            if (e.Control)
            {
                step = 10;
                lblSteps.Text = step.ToString();
            }
            if (e.KeyCode == Keys.NumPad8)
            {
                var cl = btnUp.BackColor;
                btnUp.BackColor = Color.Yellow;
                btnUp_Click(null, e);
                btnUp.BackColor = cl;
                e.Handled = true;
            }
            if (e.KeyCode == Keys.NumPad6)
            {
                var cl = btnRight.BackColor;
                btnRight.BackColor = Color.Yellow;
                btnRight_Click(null, e);
                btnRight.BackColor = cl;
                e.Handled = true;
            }
            if (e.KeyCode == Keys.NumPad4)
            {
                var cl = btnLeft.BackColor;
                btnLeft.BackColor = Color.Yellow;
                btnLeft_Click(null, e);
                btnLeft.BackColor = cl;
                e.Handled = true;
            }
            if (e.KeyCode == Keys.NumPad2)
            {
                var cl = btnDown.BackColor;
                btnDown.BackColor = Color.Yellow;
                btnDown_Click(null, e);
                btnDown.BackColor = cl;
                e.Handled = true;
            }
        }

        private void btnBottom_Enter(object sender, EventArgs e)
        {
            this.Focus();
        }

        private void btnTop_Click(object sender, EventArgs e)
        {
            if (CncController.LastState != null && !CncController.InMove)
            {
                var command = new CoordMotorCommand(MRS.Hardware.Server.CommandType.Go);
                command.X = CncController.LastState.x;
                command.Y = CncController.LastState.y;
                command.Z = CncController.LastState.z - step;
                command.Speed = speedZ;
                CncController.SendCoordCommand(command);
            }
        }

        private void btnBottom_Click(object sender, EventArgs e)
        {
            if (CncController.LastState != null && !CncController.InMove)
            {
                var command = new CoordMotorCommand(MRS.Hardware.Server.CommandType.Go);
                command.X = CncController.LastState.x;
                command.Y = CncController.LastState.y;
                command.Z = CncController.LastState.z + step;
                command.Speed = speedZ;
                CncController.SendCoordCommand(command);
            }
        }

        private void btnSpindle_Click(object sender, EventArgs e)
        {
            if (CncController.SpindleState)
            {
                CncController.Spindle(false);
                btnSpindle.BackColor = Color.Gray;
            }
            else
            {
                CncController.Spindle(true);
                btnSpindle.BackColor = Color.Orange;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            step = Int32.Parse((sender as Button).Text);
            lblSteps.Text = step.ToString();
        }

        private void numZSpeed_ValueChanged(object sender, EventArgs e)
        {
            speedX = (ushort)numXSpeed.Value;
            speedY = (ushort)numYSpeed.Value;
            speedZ = (ushort)numZSpeed.Value;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            CncController.Stop();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            var command = new CoordMotorCommand(MRS.Hardware.Server.CommandType.Go);
            command.X = 0;
            command.Y = 0;
            command.Z = 0;
            command.Speed = 7000;
            CncController.SendCoordCommand(command);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (CncController.LastState != null && !CncController.InMove)
            {
                var command = new CoordMotorCommand(MRS.Hardware.Server.CommandType.Go);
                command.X = CncController.LastState.x + step;
                command.Y = CncController.LastState.y + step;
                command.Z = CncController.LastState.z;
                command.Speed = (ushort)((speedX + speedY) / 2);
                CncController.SendCoordCommand(command);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (CncController.LastState != null && !CncController.InMove)
            {
                var command = new CoordMotorCommand(MRS.Hardware.Server.CommandType.Go);
                command.X = CncController.LastState.x + step;
                command.Y = CncController.LastState.y - step;
                command.Z = CncController.LastState.z;
                command.Speed = (ushort)((speedX + speedY) / 2);
                CncController.SendCoordCommand(command);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (CncController.LastState != null && !CncController.InMove)
            {
                var command = new CoordMotorCommand(MRS.Hardware.Server.CommandType.Go);
                command.X = CncController.LastState.x - step;
                command.Y = CncController.LastState.y - step;
                command.Z = CncController.LastState.z;
                command.Speed = (ushort)((speedX + speedY) / 2);
                CncController.SendCoordCommand(command);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (CncController.LastState != null && !CncController.InMove)
            {
                var command = new CoordMotorCommand(MRS.Hardware.Server.CommandType.Go);
                command.X = CncController.LastState.x - step;
                command.Y = CncController.LastState.y + step;
                command.Z = CncController.LastState.z;
                command.Speed = (ushort)((speedX + speedY) / 2);
                CncController.SendCoordCommand(command);
            }
        }
    }
}
