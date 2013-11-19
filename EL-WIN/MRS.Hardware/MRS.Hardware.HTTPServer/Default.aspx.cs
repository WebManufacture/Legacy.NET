using System;
using System.Collections;
using System.Configuration;
using System.Data;
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

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            var state = UART_Server.GetState();
        }

        protected void stop_Click(object sender, EventArgs e)
        {
            UART_Server.Stop();
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            UART_Server.M1();
        }

        protected void M1b_Click(object sender, EventArgs e)
        {
            UART_Server.M1b();
        }

        protected void Button4_Click(object sender, EventArgs e)
        {
            UART_Server.M2();
        }

        protected void M2b_Click(object sender, EventArgs e)
        {
            UART_Server.M2b();
        }
    }
}
