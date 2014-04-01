using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
//using MRS.Hardware.CommunicationsServices;
using System.Net;
using Newtonsoft.Json;
using SuperWebSocket;

namespace MRS.Hardware.UART_HTTP_bribge
{
    class Program
    {
        static SuperWebSocket.WebSocketServer socketServer;
        static CommunicationsServices.HardwareHttpServer httpServer;
        static UART.Serial serial;
        static string ComPortCFG;
        static string SocketMsg = "{\"type\":\"{0}\", \"data\":{1} }";
        static void Main(string[] args)
        {
            var settings = ConfigurationManager.AppSettings;
            var httpPort = Convert.ToInt32(settings["HTTP_PORT"]);
            var wsPort = Convert.ToInt32(settings["WS_PORT"]);
            var comPort = settings["COM_PORT"];
            var comPortSpeed = Convert.ToInt32(settings["COM_SPEED"]);

            ComPortCFG = "{\"port\": \"" + comPort + "\", \"speed\":\"" + comPortSpeed + "\", \"state\": \"{0}\"}";
            
            //server = new HardwareHttpServer(httpPort);
            Console.WriteLine("HTTP: " + httpPort);
            Console.WriteLine("WEB-SOCKET: " + wsPort);
            //server.OnConnect += server_OnConnect;
            //server.OnData += server_OnData;
           // server.OnServerState += server_OnState;
            //server.Start();

            socketServer = new WebSocketServer();
            socketServer.Setup(wsPort);
            socketServer.NewMessageReceived += socketServer_NewMessageReceived;
            socketServer.SessionClosed += socketServer_SessionClosed;
            socketServer.NewSessionConnected += socketServer_NewSessionConnected;
            socketServer.Start();

            httpServer = new CommunicationsServices.HardwareHttpServer(httpPort);
            httpServer.OnData += httpServer_OnData;
            httpServer.OnServerState += httpServer_OnServerState;

            serial = new UART.Serial(comPort, comPortSpeed);
            Console.WriteLine("COM: " + comPort + " " + comPortSpeed);
            serial.OnReceive += serial_OnReceive;
            serial.OnError += serial_OnError;
            serial.OnStateChange += serial_OnStateChange;
            serial.Connect();

            Thread.CurrentThread.Join();

            serial.Close();
            socketServer.Stop();
        }



        protected static string getMessage(string type, string data){
            return SocketMsg.Replace("{0}", type).Replace("{1}", data);
        }

        private static void socketServer_NewSessionConnected(WebSocketSession session)
        {
            Console.WriteLine("Sock client connected " + session.SessionID);

            session.Send(getMessage("config", ComPortCFG.Replace("{0}", serial.State + "")));
        }

        static void socketServer_SessionClosed(WebSocketSession session, SuperSocket.SocketBase.CloseReason value)
        {
            Console.WriteLine(session.SessionID + " closed " + value);
        }

        static void socketServer_NewMessageReceived(WebSocketSession thisSession, string value)
        {
            Console.WriteLine("WS --> " + value);
            if (serial.State >= UART.EDeviceState.PortOpen && serial.State < UART.EDeviceState.Offline)
            {
                serial.SendSized(JsonConvert.DeserializeObject<byte[]>(value));
            }
            var sessions = socketServer.GetAllSessions();
            var segment = getMessage("to-uart-data", value);
            foreach (var session in sessions)
            {
                if (session != thisSession)
                {
                    session.Send(segment);
                }
            }
            
        }



          
        private static void server_socketClosed(object sender, EventArgs e)
        {
            Console.WriteLine("Sock client dis connected");
        }

        //--------------------------------------------------------------------------------------------------

        static void httpServer_OnData(HttpListenerContext context, string data)
        {
            Console.WriteLine("HTTP --> " + data);
            if (serial.State >= UART.EDeviceState.PortOpen && serial.State < UART.EDeviceState.Offline)
            {
                serial.SendSized(JsonConvert.DeserializeObject<byte[]>(data));
            }
            var sessions = socketServer.GetAllSessions();
            var segment = getMessage("to-uart-data", data);
            foreach (var session in sessions)
            {
                session.Send(segment);
            }
        }

        static void httpServer_OnServerState(CommunicationsServices.HttpServerState state)
        {
            Console.WriteLine("HTTP => " + state);
        }

        static void server_OnConnect(HttpListenerContext context)
        {
            Thread.Sleep(100);
            var pName = context.Request.QueryString["port"];
            var pSpeed = context.Request.QueryString["speed"];
        }
        
        static void server_OnData(HttpListenerContext context, string data)
        {
            
        }        

        //--------------------------------------------------------------------------------------------------


        private static void serial_OnError(byte[] data)
        {
            if (socketServer != null && socketServer.State == SuperSocket.SocketBase.ServerState.Running)
            {
                Console.WriteLine("UART ERROR " + data.Length);
                var sessions = socketServer.GetAllSessions();
                var segment = getMessage("uart-error", JsonConvert.SerializeObject(data));
                foreach (var session in sessions)
                {
                    session.Send(segment);
                }
            }
        }

        static void serial_OnStateChange(UART.EDeviceState state)
        {
            Console.WriteLine("UART -> " + state);
        }

        static void serial_OnReceive(byte[] data)
        {
            if (socketServer != null && socketServer.State == SuperSocket.SocketBase.ServerState.Running)
            {
                Console.WriteLine("UART << " + data.Length);
                var sessions = socketServer.GetAllSessions();
                var segment = getMessage("from-uart-data", JsonConvert.SerializeObject(new List<byte>(data)));
                foreach (var session in sessions)
                {
                    session.Send(segment);
                }
            }
        }
    }
}
