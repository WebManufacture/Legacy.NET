using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Windows.Forms;
using MRS.Hardware.Server;

namespace Hlab.CncTable.Win.UI
{
    public static class Program
    {

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new fmMain());
            HttpServer.Close();
            CncController.Close();
        }
    }
}
