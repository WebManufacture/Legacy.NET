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
using MRS.Hardware.CommunicationsServices;
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

        fmControl controlForm = new fmControl();

        private void fmMain_Load(object sender, EventArgs e)
        {
            CncController.OnMessage += UART_OnMessage;
            CncController.OnCommand += UART_OnCommand;
            //CncProgram.OnStateChange += ProgramOnOnStateChange;
            Program.TcpServer.OnClientState += TcpServerOnClientState;
            Program.TcpServer.OnServerState += TcpServerOnServerState;
            Program.SerialPort.OnStateChange += UART_Server_OnStateChange;
            Program.HttpServer.OnServerState += HttpServerOnServerState;
            Program.HttpServer.OnData += HttpServer_OnData;
            CncController.Poll();
            TcpServerChangeState(Program.TcpServer.State);
            HttpServerChangeState(Program.HttpServer.State);
            ChangeState(Program.SerialPort.GetState());
        }

        void HttpServer_OnData(string data)
        {
            if (data.StartsWith("["))
            {
                //CncProgram.NewProgram(CncProgram.FromJSON(data));
            }
            else
            {
                MotorCommand command = MotorCommand.FromJSON(data);
                CncController.SendCommand(command);
            }
        }

        protected void CommandSended(MotorCommand command)
        {
            var str = (command.line > 0 ? command.line.ToString() : "-") + " " + command.Command.ToString() + " ";
            var cc = command as MotorCommand;
            if (cc != null)
            {
                str += " " + cc.x;
                str += " " + cc.y;
                str += " " + cc.z;
                str += " " + cc.speed;
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
                btnSerialStatus.ForeColor = Color.Green;
            }
            if (state == EDeviceState.Busy)
            {
                btnSerialStatus.ForeColor = Color.Orange;
            }
            if (state == EDeviceState.Offline)
            {
                btnSerialStatus.ForeColor = Color.DarkRed;
            }
            if (state == EDeviceState.Error)
            {
                btnSerialStatus.ForeColor = Color.Red;
            }
            btnSerialStatus.Text = state.ToString();
        }

        protected void TcpServerChangeState(TcpServerState state)
        {
            if (state == TcpServerState.Connected)
            {
                lblTcpState.ForeColor = Color.Green;
            }
            if (state == TcpServerState.Listening || state == TcpServerState.Free)
            {
                lblTcpState.ForeColor = Color.Orange;
            }
            if (state == TcpServerState.Offline)
            {
                lblTcpState.ForeColor = Color.DarkRed;
            }
            lblTcpState.Text = state.ToString() + " " + Program.TcpServer.Port;
        }

        protected void TcpClientChangeState(string state)
        {
            log.AppendText("TCP>> " + state);
        }

        protected void HttpServerChangeState(HttpServerState state)
        {
            if (state == HttpServerState.Connected)
            {
                lblHttpState.ForeColor = Color.Green;
            }
            if (state == HttpServerState.Listening || state == HttpServerState.Free)
            {
                lblHttpState.ForeColor = Color.Orange;
            }
            if (state == HttpServerState.Offline)
            {
                lblHttpState.ForeColor = Color.DarkRed;
            }
            lblHttpState.Text = state.ToString() + " " + Program.HttpServer.Port;
        }

        private PointF lpoint = new PointF(0, 0);
        private PointF point;
        private Color color;

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
                point = new Point(dState.x/5, dState.y/5);
                color = Color.Green;
                pBox.Refresh();
                lblA.Text = CncController.StateA.ToString();
                lblB.Text = CncController.StateB.ToString();
            }
            else
            {
                /*if (dState.Command == CommandType.Internal)
                {
                    lblProgram.Text = "CONTACT " + dState.state;
                }*/
            }
            
        }

        protected void UartMessage(MotorState dState)
        {
            SetState(dState);
            Program.HttpServer.Send(dState.ToString());
        }


        private void ProgramStateChange(CncProgramState state)
        {
            /*if (state == CncProgramState.NotStarted)
            {
                foreach (var motorCommand in CncProgram.Commands)
                {
                    if (motorCommand.IsCoordCommand)
                    {
                        log.AppendText(motorCommand.Command + " " + motorCommand.X + " " + motorCommand.Y + " " + motorCommand.Z + "\n");
                    }
                    else
                    {
                        log.AppendText(motorCommand.Command + "\n");
                    }
                }
            }*/
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
                Invoke(new Action<MotorState>(UartMessage), state);
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

        private void HttpServerOnServerState(HttpServerState state)
        {
            try
            {
                Invoke(new Action<HttpServerState>(HttpServerChangeState), state);
            }
            catch (Exception e)
            {
            }
        }

        private void TcpServerOnServerState(TcpServerState state)
        {
            try
            {
                Invoke(new Action<TcpServerState>(TcpServerChangeState), state);
            }
            catch (Exception e)
            {
            }
        }

        private void TcpServerOnClientState(int clientId, string state)
        {
            try
            {
                Invoke(new Action<string>(TcpClientChangeState), clientId + " " + state);
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
        
        private void resetZbtn_Click(object sender, EventArgs e)
        {
            if (CncController.LastState != null && CncController.Ready && !CncController.InMove)
            {
                var command = new MotorCommand(MRS.Hardware.Server.CommandType.Rebase);
                command.X = CncController.LastState.x;
                command.Y = CncController.LastState.y;
                command.Z = 0;
                command.Speed = 7000;
                CncController.SendCommand(command);
            }
        }

        private void resetYbtn_Click(object sender, EventArgs e)
        {
            if (CncController.LastState != null && CncController.Ready && !CncController.InMove)
            {
                var command = new MotorCommand(MRS.Hardware.Server.CommandType.Rebase);
                command.X = CncController.LastState.x;
                command.Y = 0;
                command.Z = CncController.LastState.z;
                command.Speed = 7000;
                CncController.SendCommand(command);
            }
        }

        private void resetXbtn_Click(object sender, EventArgs e)
        {
            if (CncController.LastState != null && CncController.Ready && !CncController.InMove)
            {
                var command = new MotorCommand(MRS.Hardware.Server.CommandType.Rebase);
                command.X = 0;
                command.Y = CncController.LastState.y;
                command.Z = CncController.LastState.z;
                command.Speed = 7000;
                CncController.SendCommand(command);
            }
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void lblSerialStatus_Click(object sender, EventArgs e)
        {

        }

        private void lblSerialStatus_DoubleClick(object sender, EventArgs e)
        {
        }

        private void controlsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            controlForm.Show();
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            log.Clear();
        }

        private void pollToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CncController.Poll();
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void reconnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.SerialPort.Connect();
        }

        private void disconnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.SerialPort.Close();
        }

        private void pBox_Paint(object sender, PaintEventArgs e)
        {
            var dc = e.Graphics;
            var pen = new Pen(color);
            pen.Width = 1;
            dc.DrawLine(pen, lpoint, point);
            lpoint = point;
        }

       /* private void lblHttpConnection_Click(object sender, EventArgs e)
        {
            if (HttpServer.ClientState == HttpClientState.Online)
            {
                HttpServer.Close();
            }
            else
            {
                HttpServer.Open();
            }
        }*/


    }
}

