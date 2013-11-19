using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using MRS.Hardware.Server;

namespace MRS.Hardware.HTTPServer
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RefreshPorts();
        }

        private string defaultPort = "COM2";
  
        public void RefreshPorts()
        {
            ddlPorts.Items.Clear();
            foreach (var item in SerialPort.GetPortNames())
            {
                var li = new ListItem(item, item);
                ddlPorts.Items.Add(li);
                if (item == defaultPort)
                {
                    li.Selected = true;
                    li.Text += " (Default)";
                }
            }
        }

        protected void btnConnect_Click(object sender, EventArgs e)
        {
            if (ddlPorts.SelectedItem != null)
            {
                //Uart.Init(A);
            }
        }
        /*
        private void sendBtn_Click(object sender, EventArgs e)
        {
            if (txtCommand.Text != "")
            {
                var strs = txtCommand.Text.Split(' ');
                var bytes = new byte[strs.Length];
                for (int i = 0; i < strs.Length; i++)
                {
                    bytes[i] = byte.Parse(strs[i], NumberStyles.HexNumber);
                }

                log.AppendText(DateTime.Now.ToString("HH:mm:ss") + ":>" + txtCommand.Text + "\n");
                UART_Server.Send(bytes);
            }
        }

        private void dtnDisconnect_Click(object sender, EventArgs e)
        {
            UART_Server.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            log.Clear();
        }
         * */
    }
}
