using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MRS.Hardware.UART;

namespace HLab.eBox
{
    public delegate void DeviceStateChanged(EDeviceState state);
    public delegate void SimpleInvokeMethod();

    public partial class fmMain : Form
    {
        Serial port = null;

        List<Task> TaskList = new List<Task>();

        Task currentTask;

        public event DeviceStateChanged OnDeviceStateChange;

        public SimpleInvokeMethod deviceConnectedMethod;
        public SimpleInvokeMethod showTaskMethod;

        public fmMain()
        {
            InitializeComponent();
        }

        private void fmMain_Load(object sender, EventArgs e)
        {
            cbPort.Items.AddRange(Serial.GetPorts());
            cbType.Items.Add(new KeyValuePair<string, byte>("Fill", 00));
            cbType.Items.Add(new KeyValuePair<string, byte>("Up", 01));
            cbType.Items.Add(new KeyValuePair<string, byte>("Down", 02));
            cbType.SelectedIndex = 0;
            OnDeviceStateChange += deviceStateChanged;
            deviceConnectedMethod = DeviceConnected;
            showTaskMethod = showTask;
            OnDeviceStateChange(EDeviceState.Offline);
        }

        private void cbPort_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (port != null)
            {
                port.OnReceive -= port_OnReceive;
                port.Close();
                OnDeviceStateChange(EDeviceState.Offline);

            }
            port = new Serial(cbPort.SelectedItem + "", 128000, Parity.Odd, 8, StopBits.One, 1000);
            port.OnReceive += port_OnReceive;
            var error = port.Connect();
            if (error == "ok")
            {
                OnDeviceStateChange(EDeviceState.PortOpen);
            }
            else
            {
                lblState.Text = error;
                OnDeviceStateChange(EDeviceState.Error);
                lblState.ForeColor = Color.Red;
            }
            SendCommand(UartCommand.get, 0, null);
        }

        void SendCommand(UartCommand command, ushort address, Task data)
        {
            var dta = new byte[7];
            dta[0] = (byte)command;
            dta[1] = (byte)(address << 8);
            dta[2] = (byte)(address);
            if (data != null)
            {
                data.GetBytes().CopyTo(dta, 3);
            }
            port.Send(dta, 1, false);
        }

        void deviceStateChanged(EDeviceState state)
        {
            if (state == EDeviceState.Offline)
            {
                lblState.Text = "Offline";
                lblState.ForeColor = Color.Gray;
            }
            if (state == EDeviceState.PortOpen)
            {
                lblState.Text = "Opened";
                lblState.ForeColor = Color.Yellow;
            }
            if (state == EDeviceState.Online)
            {
                Invoke(deviceConnectedMethod);
                lblState.Text = "Connected";
                lblState.ForeColor = Color.Green;
            }
        }

        void DeviceConnected()
        {
            lblState.Text = "Connected";
            lblState.ForeColor = Color.Green;
        }

        void showTask()
        {
            tbProg.Text += currentTask.Address + " " + currentTask.Port + (currentTask.TaskType > 0 ? (currentTask.TaskType == 3 ? " S " : (currentTask.TaskType == 1 ? " / " : "\\")) : " - ");
            tbProg.Text += " " + currentTask.value;
            tbProg.Text += " " + currentTask.Time.ToString();
            tbProg.Text += "\n";
        }


        void port_OnReceive(byte[] data)
        {
            if ((UartCommand)data[0] == UartCommand.get)
            {
                OnDeviceStateChange(EDeviceState.Online);
                ushort addr = (ushort)(data[1] << 8);
                addr += data[2];
                if (data[3] + data[4] + data[5] + data[6] == 0)
                {
                    return;
                }
                currentTask = new Task(data, 3);
                currentTask.Address = addr;
                TaskList.Add(currentTask);
                addr++;
                SendCommand(UartCommand.get, addr, null);
            }
        }

        private void stateTimer_Tick(object sender, EventArgs e)
        {
            if (port != null)
            {
                lblState.Text = port.GetState() + "";
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            Task task = new Task();
            task.value = (byte)cbFill.Value;
            task.start = 0;
            task.Port = (byte)(cbChannel.Value - 1);
            task.TaskType = ((KeyValuePair<string, byte>)cbType.SelectedItem).Value;
            task.IsActive = true;
            SendCommand(UartCommand.exec, 0, task);
        }

        private void btnMMode_Click(object sender, EventArgs e)
        {
            SendCommand(UartCommand.mmode, 0, null);
        }
    }
}
