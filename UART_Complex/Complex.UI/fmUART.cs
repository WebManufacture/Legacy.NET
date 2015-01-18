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

namespace MRS.Hardware.UI.Analyzer
{
    public partial class fmUART : Form
    {
        SerialManager manager;

        public fmUART()
        {
            InitializeComponent();
            manager = new SerialManager();
        }

        public fmUART(SerialManager device)
        {
            InitializeComponent();
            manager = device;
        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            var baud = Convert.ToInt32(cbBaud.SelectedItem);
            var bits = Convert.ToByte(cbBits.SelectedItem);
            var stop = (StopBits)Enum.Parse(typeof(StopBits), cbStop.SelectedItem + "");
            var parity = (Parity)Enum.Parse(typeof(Parity), cbParity.SelectedItem + "");
           // manager.SetParams(baud, bits, stop, parity);
        }


        public void Sended(string data)
        {
            txtSend.Text += "> " + data + "\n";
        }

        public void Readed(byte data)
        {
            Readed(data.ToString());
        }

        public void Readed(string data)
        {
            var date = DateTime.Now;
            txtReceive.Text = "< " + data + "    -  " + date.ToLongTimeString() + "::" + date.Millisecond + "\n" + txtReceive.Text;
        }

        private void btnWrite_Click(object sender, EventArgs e)
        {
            var data = Convert.ToByte(txtByte.Text);
            if (manager.Send(data))
            {
                Sended(data.ToString());
            }
            else
            {
                //txtSend.Text += "! " + manager.LastError + "\n";
            }
        }

        private void btnExec_Click(object sender, EventArgs e)
        {
            var txt = boxCMD.Text;
            byte val;
            if (Byte.TryParse(txt, out val)){
                if (manager.Send(val))
                {
                    Sended(txt);
                }
                return;
            }
            var values = txt.Split(' ');
            if (values.Length > 1 && values[0] != ""){
                if (values[0] == "C" || values[0] == "c"){
                    if (Byte.TryParse(values[1], out val)){
                        if (values.Length > 2){
                            byte[] data = new byte[values.Length - 1];
                            data[0] = val;
                            for (var i = 1; i < values.Length - 1; i++){
                                data[i] = Convert.ToByte(values[i + 1]);
                            }
                            manager.Send(data);
                        }
                        else{
                            manager.Send(val);
                        }
                        Sended(txt);    
                    }
                }
            }
        }

        private void btnRead_Click(object sender, EventArgs e)
        {
            var data = manager.ReadByte();
           // Readed(data);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            receiveTimer.Enabled = !receiveTimer.Enabled;
        }

        private void receiveTimer_Tick(object sender, EventArgs e)
        {
            var bytes = manager.BytesAvailable();
            if (bytes > 0)
            {
                var data = manager.ReadData(bytes);
                var str = "";
                foreach (byte dta in data)
                {
                    Readed(dta);
                }                
            }
        }
    }
}
