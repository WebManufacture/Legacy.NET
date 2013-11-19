using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MRS.Hardware.Server;
using MRS.Hardware.UART;
using CommandType = MRS.Hardware.Server.CommandType;

namespace Hlab.CncTable.Win.UI
{


    public partial class fmMain : Form
    {
        public fmMain()
        {
            InitializeComponent();
        }

        private void fmMain_Load(object sender, EventArgs e)
        {
            RefreshPorts();
            CncController.OnMessage += UART_OnMessage;
            CncController.OnCommand += UART_OnCommand;
            CncProgram.OnStateChange += ProgramOnOnStateChange;
            HttpServer.OnClientState += HttpServerOnOnClientState;
            ClientChangeState(HttpServer.ClientState);
            CncController.Init(Application.StartupPath);
            HttpServer.Init("http://*:8008");
            CncController.Open();
            HttpServer.Open();
        }

        protected void CommandSended(MotorCommand command)
        {
            var str = (command.Line > 0 ? command.Line.ToString() : "-") + " " + command.Command.ToString() + " ";
            var cc = command as CoordMotorCommand;
            if (cc != null)
            {
                str += " " + cc.X;
                str += " " + cc.Y;
                str += " " + cc.Z;
                str += " " + cc.Speed;
            }
            if (log.Lines.Length > 60)
            {
                log.Lines[log.Lines.Length - 1] = null;
            }
            log.Text = (str + "\n") + log.Text;
        }

        protected void ChangeState(EDeviceState state)
        {
            if (state >= EDeviceState.Online)
            {
                lnkState.ForeColor = Color.Green;
            }
            if (state == EDeviceState.Busy)
            {
                lnkState.ForeColor = Color.Orange;
            }
            if (state == EDeviceState.Offline)
            {
                lnkState.ForeColor = Color.DarkRed;
            }
            if (state == EDeviceState.Error)
            {
                lnkState.ForeColor = Color.Red;
            }
            lnkState.Text = state.ToString();
        }

        protected void ClientChangeState(HttpClientState state)
        {
            if (state == HttpClientState.Online)
            {
                lblHttpConnection.ForeColor = Color.Green;
            }
            if (state == HttpClientState.Connected)
            {
                lblHttpConnection.ForeColor = Color.Orange;
            }
            if (state == HttpClientState.Disconnected)
            {
                lblHttpConnection.ForeColor = Color.DarkRed;
            }
            lblHttpConnection.Text = state.ToString();
        }

        protected void SetState(MotorState dState)
        {
            lblCommand.ForeColor = Color.Black;
            if (dState.Command >= CommandType.Null && dState.Command <= CommandType.Error)
            {
                if (dState.state == 0 || dState.state == 9)
                {
                    lblX.Text = dState.x + "";
                    lblY.Text = dState.y + "";
                    lblZ.Text = dState.z + "";
                    if (dState.state == 9)
                    {
                        lblProgram.Text = "INITIALIZED " + dState.line;
                        lblProgram.ForeColor = Color.DarkGoldenrod;
                    }
                    else
                    {
                        lblProgram.Text = "IDLE";
                        lblProgram.ForeColor = Color.Gray;
                    }
                }
                if (dState.state == 1)
                {
                    if (dState.Command != CommandType.State)
                    {
                        lblCommand.Text = dState.Command.ToString();
                        lblProgram.Text = "(WORKING)";
                    }
                    lblX.Text = dState.x + " -> " + dState.xLimit;
                    lblY.Text = dState.y + " -> " + dState.yLimit;
                    lblZ.Text = dState.z + " -> " + dState.zLimit;
                }
                if (dState.state == 2)
                {
                    if (dState.Command != CommandType.State)
                    {
                        lblCommand.Text = dState.Command.ToString();
                        lblProgram.Text = "(STOP)";
                    }
                    lblX.Text = dState.x + " - " + dState.xLimit;
                    lblY.Text = dState.y + " - " + dState.yLimit;
                    lblZ.Text = dState.z + " - " + dState.zLimit;
                }
                if (dState.state == 4)
                {
                    lblCommand.Text = dState.Command.ToString();
                    lblCommand.ForeColor = Color.Red;
                    lblProgram.Text = "(ERROR) " + dState.line;
                    lblX.Text = dState.x + " - " + dState.xLimit;
                    lblY.Text = dState.y + " - " + dState.yLimit;
                    lblZ.Text = dState.z + " - " + dState.zLimit;
                    //log.AppendText(DateTime.Now.ToString("HH:mm:ss") + "<" + format + "\n");
                }
                lblA.Text = CncController.StateA.ToString();
                lblB.Text = CncController.StateB.ToString();
            }
            else
            {
                if (dState.Command == CommandType.Contact)
                {
                    lblProgram.Text = "CONTACT " + dState.state;
                }
            }
            
        }


        private void ProgramStateChange(CncProgramState state)
        {
            if (state == CncProgramState.NotStarted)
            {
                foreach (var motorCommand in CncProgram.Commands)
                {
                    if (motorCommand is CoordMotorCommand)
                    {
                        var cmcommand = motorCommand as CoordMotorCommand;
                        log.AppendText(cmcommand.Command + " " + cmcommand.X + " " + cmcommand.Y + " " + cmcommand.Z + "\n");
                    }
                    else
                    {
                        log.AppendText(motorCommand.Command + "\n");
                    }
                }
            }
            lblProgram.Text = state.ToString();
        }

        void UART_Server_OnStateChange(EDeviceState state)
        {
            try
            {
                Invoke(new Action<EDeviceState>(ChangeState), state);
            }
            catch(Exception e)
            {
            }
        }

        void UART_OnMessage(MotorState state)
        {
            try
            {
                Invoke(new Action<MotorState>(SetState), state);
            }
            catch (Exception e)
            {
            }
        }

        void UART_OnCommand(MotorCommand command)
        {
            try
            {
                Invoke(new Action<MotorCommand>(CommandSended), command);
            }
            catch (Exception e)
            {
            }
        }


        private void HttpServerOnOnClientState(HttpClientState state)
        {
            try
            {
                Invoke(new Action<HttpClientState>(ClientChangeState), state);
            }
            catch (Exception e)
            {
            }
        }

        private void ProgramOnOnStateChange(CncProgramState state)
        {
            try
            {
                Invoke(new Action<CncProgramState>(ProgramStateChange), state);
            }
            catch (Exception e)
            {
            }
        }


        private void btnReset_Click(object sender, EventArgs e)
        {
            RefreshPorts();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            RefreshPorts();
        }

        void RefreshPorts()
        {
            cbPort.Items.Clear();
            foreach (var item in CncController.GetDevicesList())
            {
                cbPort.Items.Add(item.Value);
            }

            if (cbPort.Items.Count > 0)
            {
                cbPort.SelectedIndex = 0;
            }
        }

        private void sendBtn_Click(object sender, EventArgs e)
        {
            /*if (txtCommand.Text != "")
            {
                var strs = txtCommand.Text.Split(' ');
                var bytes = new byte[strs.Length];
                for (int i = 0; i < strs.Length; i++)
                {
                    bytes[i] = byte.Parse(strs[i], NumberStyles.HexNumber);
                }
               // AddLogToDevice(bytes);
               // Uart.SendCommand(bytes);
            }*/
        }

        private void dtnDisconnect_Click(object sender, EventArgs e)
        {
            CncController.Close();
            HttpServer.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            log.Clear();
        }

        private void stateTmr_Tick(object sender, EventArgs e)
        {
            if (Uart.State >= EDeviceState.Online && CncController.InMove)
            {
               // CncController.GetStateAsync();
            }
            else
            {
                
            }
        }

        private void fmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            CncController.Stop();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            CncController.Pause();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            CncController.Resume();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            ChangeState(Uart.GetState());
        }

        private void btnControl_Click(object sender, EventArgs e)
        {
            fmControl controlForm = new fmControl();
            controlForm.Show();
        }

        private void resetZbtn_Click(object sender, EventArgs e)
        {
            if (CncController.LastState != null && CncController.Ready && !CncController.InMove)
            {
                var command = new CoordMotorCommand(MRS.Hardware.Server.CommandType.Rebase);
                command.X = CncController.LastState.x;
                command.Y = CncController.LastState.y;
                command.Z = 0;
                command.Speed = 7000;
                CncController.SendCoordCommand(command);
            }
        }

        private void resetYbtn_Click(object sender, EventArgs e)
        {
            if (CncController.LastState != null && CncController.Ready && !CncController.InMove)
            {
                var command = new CoordMotorCommand(MRS.Hardware.Server.CommandType.Rebase);
                command.X = CncController.LastState.x;
                command.Y = 0;
                command.Z = CncController.LastState.z;
                command.Speed = 7000;
                CncController.SendCoordCommand(command);
            }
        }

        private void resetXbtn_Click(object sender, EventArgs e)
        {
            if (CncController.LastState != null && CncController.Ready && !CncController.InMove)
            {
                var command = new CoordMotorCommand(MRS.Hardware.Server.CommandType.Rebase);
                command.X = 0;
                command.Y = CncController.LastState.y;
                command.Z = CncController.LastState.z;
                command.Speed = 7000;
                CncController.SendCoordCommand(command);
            }
        }

        private void lblHttpConnection_Click(object sender, EventArgs e)
        {
            if (HttpServer.ClientState == HttpClientState.Online)
            {
                HttpServer.Close();
            }
            else
            {
                HttpServer.Open();
            }
        }

        private void lnkState_Click(object sender, EventArgs e)
        {
            if (CncController.IsOpen)
            {
                CncController.Close();
            }
            else
            {
                CncController.Open();
            }
        }
    }
}

