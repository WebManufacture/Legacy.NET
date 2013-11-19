using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MRS.Hardware.UART;

namespace HLab.eBox
{
    public delegate void DeviceStateChanged(EDeviceState state);
    public delegate void SimpleInvokeMethod();

    public partial class fmMain : Form
    {
        Serial port = null;

        List<Task> TaskList;

        int currentTask;
        Task readedTask;
        EBoxSettings currentSettings;

        bool ProgrammingMode = false;

        public event DeviceStateChanged OnDeviceStateChange;

        public SimpleInvokeMethod deviceConnectedMethod;
        public SimpleInvokeMethod showTaskMethod;
        public SimpleInvokeMethod showSettingsMethod;
        public SimpleInvokeMethod writeCompleteMethod;
        public SimpleInvokeMethod writeProgressMethod;

        public fmMain()
        {
            InitializeComponent();
        }

        private void fmMain_Load(object sender, EventArgs e)
        {
            cbPort.Items.AddRange(Serial.GetPorts());
            cbType.Items.Add(new KeyValuePair<string, byte>("Pin", 01));
            cbType.Items.Add(new KeyValuePair<string, byte>("PwmUp", 02));
            cbType.Items.Add(new KeyValuePair<string, byte>("PwmDown", 03));
            cbType.Items.Add(new KeyValuePair<string, byte>("Pwm", 04));
            cbType.Items.Add(new KeyValuePair<string, byte>("Beep", 12));
            cbType.Items.Add(new KeyValuePair<string, byte>("System", 13));
            //cbType.Items.Add(new KeyValuePair<string, byte>("Down", 02));
            cbType.SelectedIndex = 0;
            OnDeviceStateChange += deviceStateChanged;
            deviceConnectedMethod = DeviceConnected;
            showTaskMethod = showTask;
            showSettingsMethod = showSettings;
            writeCompleteMethod = writeComplete;
            writeProgressMethod = writeProgress;
            OnDeviceStateChange(EDeviceState.Offline);
            pnlControls.Enabled = false;
            cbStartMode.SelectedIndex = 0;
            cbEndMode.SelectedIndex = 0;
            TaskList = new List<Task>();
        }

        private void cbPort_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (port != null)
            {
                port.OnReceive -= port_OnReceive;
                port.Close();
                OnDeviceStateChange(EDeviceState.Offline);

            }
            port = new Serial(cbPort.SelectedItem + "", 115200, paritySel.Checked ? Parity.Odd : Parity.None, 8, StopBits.One, 1000);
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
            tbReaded.Text = "";
            SendCommand(UartCommand.get, 0);
        }

        void SendCommand(UartCommand command, ushort address)
        {
            var dta = new byte[3];
            dta[0] = (byte)command;
            dta[1] = (byte)(address << 8);
            dta[2] = (byte)(address);
            port.Send(dta, 1, false);
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

        void SendCommand(UartCommand command, ushort address, byte[] data)
        {
            var dta = new byte[7];
            dta[0] = (byte)command;
            dta[1] = (byte)(address << 8);
            dta[2] = (byte)(address);
            if (data != null)
            {
                data.CopyTo(dta, 3);
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
            pnlControls.Enabled = true;
        }

        void showSettings()
        {
            if (currentSettings != null)
            {
                tbReaded.Text += "S:" + currentSettings.BeginMode + " E:" + currentSettings.EndMode + "\r\n";
                cbStartMode.SelectedItem = currentSettings.BeginMode + "";
                cbEndMode.SelectedItem = currentSettings.EndMode + "";
            }
        }

        void showTask()
        {
            tbReaded.Text += "P" + readedTask.Port + " ";
            var value = readedTask.PercentValue;
            switch (readedTask.TaskType){
                case TaskTypes.pin : tbReaded.Text += "V"; break;
                case TaskTypes.pwmDown: tbReaded.Text += "D"; value = readedTask.value; break;
                case TaskTypes.pwmUp: tbReaded.Text += "U"; value = readedTask.value; break;
            }
            tbReaded.Text += value + " S" + readedTask.start + "\r\n";
        }

        void writeComplete()
        {
            lblError.Text = "Write complete";
        }

        void writeProgress()
        {
            lblError.Text = "Записано " + currentTask + " из " + TaskList.Count;
        }

        void port_OnReceive(byte[] data)
        {
            if ((UartCommand)data[0] == UartCommand.get)
            {
                OnDeviceStateChange(EDeviceState.Online);
                ushort addr = (ushort)(data[1] << 8);
                addr += data[2];
                if (addr == 0)
                {
                    currentSettings = new EBoxSettings(data, 3);
                    Invoke(showSettingsMethod);
                }
                else
                {
                    if (data[3] + data[4] + data[5] + data[6] == 0)
                    {
                        readedTask = null;
                        ShowProgram();
                        return;
                    }
                    var cTask = new Task(data, 3);
                    cTask.Address = addr;
                    readedTask = cTask;
                    Invoke(showTaskMethod);
                }
                addr++;
                SendCommand(UartCommand.get, addr);
            }
            if ((UartCommand)data[0] == UartCommand.set)
            {
                ushort addr = (ushort)(data[1] << 8);
                addr += data[2];
                if (TaskList != null && currentTask < TaskList.Count)
                {
                    addr++;
                    Invoke(writeProgressMethod);
                    SendCommand(UartCommand.set, addr, TaskList[currentTask]);
                    currentTask++;
                }
                else
                {
                    if (currentTask == TaskList.Count)
                    {
                        addr++;
                        SendCommand(UartCommand.set, addr, new byte[4]);
                        currentTask++;
                    }
                    else
                    {
                        Invoke(writeCompleteMethod);
                    }
                }
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
            task.Port = (byte)(cbChannel.Value);
            task.Type = ((KeyValuePair<string, byte>)cbType.SelectedItem).Value;
            SendCommand(UartCommand.task, 0, task);
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            SendCommand(UartCommand.reset, 0);
        }


        private void btnStart_Click(object sender, EventArgs e)
        {
            SendCommand(UartCommand.restart, 0);
        }


        private void btnPause_Click(object sender, EventArgs e)
        {
            SendCommand(UartCommand.pause, 0);
        }

        private void btnResume_Click(object sender, EventArgs e)
        {
            SendCommand(UartCommand.resume, 0);
        }

        private void button18_Click(object sender, EventArgs e)
        {
            var btn = sender as Button;
            var pn = byte.Parse(btn.Text);
            Task task = new Task();
            if (btn.BackColor == Color.Silver)
            {
                btn.BackColor = Color.LightGreen;
                task.value = 255;
            }
            else
            {
                btn.BackColor = Color.Silver;
                task.value = 0;
            }
            task.start = 0;
            task.Port = (byte)(pn - 1);
            task.TaskType = TaskTypes.pin;
            //task.IsActive = true;
            SendCommand(UartCommand.task, 0, task);
        }

        private void button21_Click(object sender, EventArgs e)
        {
            var btn = sender as Button;
            var pn = btn.Text == "B" ? 20 : (btn.Text == "R" ? 21 : (btn.Text == "G" ? 22 : 0));
            Task task = new Task();
            if (btn.ForeColor == Color.Black)
            {
                btn.ForeColor = Color.Silver;
                task.value = 255;
            }
            else
            {
                btn.ForeColor = Color.Black;
                task.value = 0;
            }
            task.start = 0;
            task.Port = (byte)pn;
            task.TaskType = 0;
            //task.IsActive = true;
            SendCommand(UartCommand.task, 0, task);
        }

        private void btnWrite_Click(object sender, EventArgs e)
        {
            if (CompileProgram())
            {
                SendProgram();
            }
            //P24 V255 S1
            //P25 V255 S2
            //P26 V255 S3
        }

        private void btnCompile_Click(object sender, EventArgs e)
        {
            CompileProgram();
        }

        protected void ShowProgram()
        {

        }

        protected void SendProgram()
        {
            currentTask = 0;
            var startMode = (RunMode)Enum.Parse(typeof(RunMode), cbStartMode.SelectedItem + "");
            var endMode = (RunMode)Enum.Parse(typeof(RunMode), cbEndMode.SelectedItem + "");
            currentSettings = new EBoxSettings(startMode, endMode);
            SendCommand(UartCommand.set, 0, currentSettings.GetBytes());
        }

        protected bool CompileProgram()
        {
            tbProg.Text = tbProg.Text.Trim();
            if (tbProg.Text.Length > 0)
            {
                TaskList = new List<Task>();
                for (var i = 0; i < tbProg.Lines.Length; i++)
                {
                    if (tbProg.Lines[i].StartsWith("//")) continue;
                    var task = ParseTask(tbProg.Lines[i]);
                    if (task != null)
                    {
                        TaskList.Add(task);
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            lblError.Text = "Compiled success";
            return true;
        }

        protected Task ParseTask(string text)
        {
            if (string.IsNullOrEmpty(text)) return null;
            var task = new Task();
            var items = text.Split(' ');
            //task.IsActive = true;
            task.TaskType = TaskTypes.pin;
            for (var i = 0; i < items.Length; i++)
            {
                try
                {
                    var item = items[i].ToUpperInvariant();
                    if (item.StartsWith("P"))
                    {
                        item = item.Remove(0, 1);
                        task.Port = Byte.Parse(item);
                        continue;
                    }
                    if (item.StartsWith("V"))
                    {
                        item = item.Remove(0, 1);
                        task.TaskType = TaskTypes.pin;
                        task.PercentValue = Byte.Parse(item);
                        continue;
                    }
                    if (item.StartsWith("D"))
                    {
                        item = item.Remove(0, 1);
                        task.TaskType = TaskTypes.pwmDown;
                        task.value = Byte.Parse(item);
                        continue;
                    }
                    if (item.StartsWith("U"))
                    {
                        item = item.Remove(0, 1);
                        task.TaskType = TaskTypes.pwmUp;
                        task.value = Byte.Parse(item);
                        continue;
                    }
                    if (item.StartsWith("S"))
                    {
                        item = item.Remove(0, 1);
                        task.start = ushort.Parse(item);
                        continue;
                    }
                }
                catch (Exception e)
                {
                    lblError.Text = (i + 1) + ": " + e;
                    return null;
                }
            }
            return task;
        }


    }
}
