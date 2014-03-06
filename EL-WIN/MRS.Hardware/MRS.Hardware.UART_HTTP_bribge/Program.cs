using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using MRS.Hardware.CommunicationsServices;
using System.Net;

namespace MRS.Hardware.UART_TCP_bribge
{
    class Program
    {
        static HardwareTcpServer server;
        static HttpToUartBridge bridge;
        static UART.Serial serial;

        static void Main(string[] args)
        {
            var settings = ConfigurationManager.AppSettings;
            var port = settings["COM_PORT"];
            var speed = Convert.ToInt32(settings["COM_SPEED"]);
            var httpPort = Convert.ToInt32(settings["HTTP_PORT"]);
            serial = new UART.Serial(port, speed);
            Console.WriteLine("UART: " + port + " - " + speed);
            server = new HardwareTcpServer(tcpPort);
            server.OnServerState += server_OnServerState;
            server.OnClientState += server_OnClientState;
            server.OnData += server_OnData;
            serial.OnReceive += serial_OnReceive;
            serial.OnStateChange += serial_OnStateChange;
            serial.Connect();

            bridge = new HttpToUartBridge(6200);

            TcpListener listener = new TcpListener(IPAddress.Parse("188.127.233.35"), tcpPort);
            listener.Start();
            Console.WriteLine("TCP: " + tcpPort);
            while (Thread.CurrentThread.ThreadState == ThreadState.Running)
            {
                server_OnServerState("Listening " + tcpPort);
                while (!listener.Pending())
                {
                    Thread.Sleep(300);
                }
                server.AcceptTcpClient(listener.AcceptSocket());
            }
            server_OnServerState("Stopped");
            listener.Stop();
            server.Close();
            serial.Close();
        }

        static void serial_OnStateChange(UART.EDeviceState state)
        {
            Console.WriteLine("UART -> " + state);
        }

        static void server_OnClientState(int clientId, string message)
        {
            Console.WriteLine("TCP_Client" + clientId + "  ->  " + message + " " + server.GetClientIP(clientId));
        }

        static void server_OnServerState(string state)
        {
            Console.WriteLine("TCP -> " + state);
        }

        static void server_OnData(int clientId, byte[] data)
        {
            serial.Send(data);
        }

        static void serial_OnReceive(byte[] data)
        {
            server.Send(data);
        }
    }
}
