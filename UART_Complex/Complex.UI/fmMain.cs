using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using MRS.Hardware.UART;
using Newtonsoft.Json;

namespace MRS.Hardware.UI.Analyzer
{
    public partial class fmMain : Form
    {
        protected volatile byte data;

        SerialManager device = null;
        static List<SerialConfig> ComPortConfigs;

        public void ShowMessage(string message)
        {
            txtConsole.Text += message;
            txtConsole.Text += "\n-----------------------------------\n";
        }

        public void ShowError(string message, Exception err)
        {
            txtConsole.Text += message + "\n";
            txtConsole.Text += err.Message + "\n";
            txtConsole.Text += err.Source + "\n";
            txtConsole.Text += err.StackTrace + "\n";
            txtConsole.Text += "\n-----------------------------------\n";
        }

        public void ShowError(string message)
        {
            txtConsole.Text += message;
            txtConsole.Text += "\n!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!\n";
        }

        public void FillDevicesList()
        {
            boxConnect.DropDownItems.Clear();
            cbCOMs.Items.Clear();
            var serials = SerialPort.GetPortNames();
            var info = "Available COMs: \n";
            foreach (var port in serials)
            {
                cbCOMs.Items.Add(port);
                info += port + "\n";
            }
            ShowMessage(info);
            var devList = FtdiManager.GetDevicesList();
            if (devList == null)
            {
                ShowError("GetDeviceInfo: Device Error!");
                return;
            }
            if (devList.Length > 0)
            {
                info = "";
                for (uint i = 0; i < devList.Length; i++)
                {
                    info += "Device: " + devList[i].ID + "\n";
                    info += "Type: " + devList[i].Type + "\n";
                    info += "SerialNumber: " + devList[i].SerialNumber + "\n";
                    info += "Description: " + devList[i].Description + "\n";
                    info += "\n";
                    ToolStripItem btn = new ToolStripButton(devList[i].Description);
                    btn.Click += deviceBtn_Click;
                    btn.Tag = i;
                    boxConnect.DropDownItems.Add(btn);
                }
                ShowMessage(info);
            }
            else
            {
                ShowMessage("No devices found");
            }
        }

        public void FillStoredDevices()
        {
            foreach (var cfg in ComPortConfigs)
            {
                var item = cbStoredDevices.Items.Add(cfg);
            }
        }

        void deviceBtn_Click(object sender, EventArgs e)
        {/*
            SelectedDevice = Convert.ToUInt32(this.Tag);
            FtdiManager.GetDeviceInfo();*/
            ToolsMenu.Enabled = true;
        }

        public fmMain()
        {
            InitializeComponent();
            FillDevicesList();

            var settings = ConfigurationManager.AppSettings;
            var comPortsConfig = JsonConvert.DeserializeObject<List<string>>(settings["COM_PORTS"]);
            ComPortConfigs = new List<SerialConfig>();
            foreach (var cfg in comPortsConfig)
            {
                ComPortConfigs.Add(SerialConfig.Parse(cfg));
            }
            FillStoredDevices();
        }

        private void Close_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void BitBang_Click(object sender, EventArgs e)
        {
           // fmBits bt = new fmBits(SelectedDevice.Value);
//bt.Show();
        }


        private void btnStats_Click(object sender, EventArgs e)
        {
            if (device != null)
            {
                fmStats stats = new fmStats(device);
                stats.Show();
            }
        }


        private void Rescan_Click(object sender, EventArgs e)
        {
            FillDevicesList();
        }

        private void Analizer_Click(object sender, EventArgs e)
        {
            if (device != null && device is SerialPacketManager)
            {
                fmAnalizer stats = new fmAnalizer(device as SerialPacketManager);
                stats.Show();
            }
        }

        private void UART_Click(object sender, EventArgs e)
        {
            if (device != null)
            {
                fmUART stats = new fmUART(device);
                stats.Show();
            }
        }

        private void Reset_Click(object sender, EventArgs e)
        {
            if (device != null && device is FtdiManager)
            {
                var manager = (device as FtdiManager);
                manager.ResetChip();
                manager.Close();
            }
        }

        private void ADC_Click(object sender, EventArgs e)
        {
            if (device != null && device is SerialPacketManager)
            {
                fmADC stats = new fmADC(device as SerialPacketManager);
                stats.Show();
            }
        }

        private void HttpStart_Click(object sender, EventArgs e)
        {

        }

        private void HttpStop_Click(object sender, EventArgs e)
        {

        }

        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        void SelectedPort_Disposed(EDeviceState state, SerialManager sm)
        {
            ShowMessage(sm.PortName + " -->  " + state);
            if (state == EDeviceState.Offline)
            {
                DisableItems();
                device = null;
            }
        }

        private void boxConnect_Click(object sender, EventArgs e)
        {

        }

        private void fmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (device != null)
            {
                device.Close();
            }
        }

        private void disconnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (device != null)
            {
                device.Close();
            }
        }


        protected void OpenDevice(SerialConfig cfg)
        {
            if (cfg == null) return;
            if (device != null)
            {
                device.Close();
            }
            try
            {
                switch (cfg.RxPacketType)
                {
                    case PacketType.Raw:
                        device = new SerialManager(cfg);
                        break;
/*case PacketType.Simple:
                    case PacketType.SimpleCoded:
                    case PacketType.SimpleCRC:
                    case PacketType.Sized:                    
                    case PacketType.SizedOld:
                    case PacketType.SizedCRC:
                    case PacketType.SizedCRC_old:
                    case PacketType.PacketInvariant:
                        device = new SerialPacketManager(cfg);
                        break;*/
                    case PacketType.Addressed:
                    case PacketType.AddressedOld:
                        device = new SerialAddressedManager(cfg);
                        break;
                    case PacketType.XRouting:
                        device = new XRoutingManager(cfg);
                        break;
                    default: 
                        device = new SerialPacketManager(cfg);
                        break;
                }
                if (device.Connect())
                {
                    device.OnStateChange += SelectedPort_Disposed;
                    ShowMessage(cfg.PortName + "  " + "\nConnected!");
                    ShowMessage(JsonConvert.SerializeObject(cfg));
                    EnableItems();
                }
                else
                {
                    DisableItems();
                    device.Close();
                    device = null;
                    ShowError(cfg.PortName + "Connecting error: " + device.LastError);
                }
            }
            catch (Exception err)
            {
                ShowError(cfg.PortName + "Connecting error: ", err);
            }
        }

        private void menuUARTconnect_Click(object sender, EventArgs e)
        {
            if (cbCOMs.SelectedIndex >= 0 && cbUARTSpeeds.SelectedIndex >= 0)
            {
                var portName = cbCOMs.SelectedItem + "";
                SerialConfig cfg = ComPortConfigs.FirstOrDefault(c => c.PortName == portName);
                SerialManager sm;
                if (cfg == null)
                {
                    cfg = new SerialConfig(portName)
                    {
                        Speed = Convert.ToUInt32(cbUARTSpeeds.SelectedItem + "")
                    };
                }
                OpenDevice(cfg);
            }
        }

        private void cbStoredDevices_SelectedIndexChanged(object sender, EventArgs e)
        {
            var dev = cbStoredDevices.SelectedItem as SerialConfig;
            OpenDevice(dev);
        }

        private void btnUART_Click(object sender, EventArgs e)
        {

        }

        public void EnableItems()
        {
            if (device != null)
            {
                ToolsMenu.Enabled = true;
                btnUART.Enabled = true;
                if (device is SerialPacketManager)
                {
                    btnAnalyzer.Enabled = true;
                }
                if (device is XRoutingManager)
                {
                    btnADC.Enabled = true;
                }
            }
        }

        public void DisableItems()
        {
            if (device != null)
            {
                ToolsMenu.Enabled = false;
                btnUART.Enabled = false;
                btnAnalyzer.Enabled = false;
                btnADC.Enabled = false;
            }
        }
    }
}
