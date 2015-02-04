using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Configuration;
using System.Windows.Forms;
using MRS.Hardware.CommunicationsServices;
using MRS.Hardware.Server;
using MRS.Hardware.UART;

namespace Hlab.CncTable.Win.UI
{
    public static class Program
    {

        public static HardwareTcpServer TcpServer;
        public static SmallHttpServer HttpServer;
        public static SerialAddressedManager SerialPort;
        public static CncController Controller;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var settings = ConfigurationManager.AppSettings;
            var port = settings["COM_PORT"];
            var speed = Convert.ToInt32(settings["COM_SPEED"]);
            var tcpPort = Convert.ToInt32(settings["TCP_PORT"]);
            var httpPort = Convert.ToInt32(settings["HTTP_PORT"]);
            var cncAddr = Convert.ToByte(settings["CNC_ADDR"]);
            SerialPort = new SerialAddressedManager(port, speed);
            SerialPort.DeviceAddr = cncAddr;
            //SerialPort.Connect();
            HttpServer = new SmallHttpServer(httpPort);
            HttpServer.Start();
            TcpServer = new HardwareTcpServer(tcpPort);
            TcpServer.Start();
            CncController.Init(SerialPort, Application.StartupPath, cncAddr);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new fmMain());
            HttpServer.Close();
            TcpServer.Close();
            SerialPort.Close();
            CncController.Close();
        }
    }
}
