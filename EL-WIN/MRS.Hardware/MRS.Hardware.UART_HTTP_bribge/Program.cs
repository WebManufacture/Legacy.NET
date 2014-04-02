using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
//using MRS.Hardware.CommunicationsServices;
using System.Net;
using SuperWebSocket;

namespace MRS.Hardware.UART_HTTP_bribge
{
    class Program
    {
        static SuperWebSocket.WebSocketServer socketServer;
        static TcpService tcpService;
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
            var callbacks = settings["CALLBACK_CLIENTS"];

            var callbackClients = new String[]{callbacks};

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
            //socketServer.Start();


            serial = new UART.Serial(comPort, comPortSpeed);
            Console.WriteLine("COM: " + comPort + " " + comPortSpeed);
            serial.OnReceive += serial_OnReceive;
            serial.OnError += serial_OnError;
            serial.OnStateChange += serial_OnStateChange;
            serial.Connect();


            tcpService = new TcpService();
            tcpService.OnData += tcpService_OnData;
            tcpService.OnClientState += tcpService_OnClientState;
            tcpService.Start(callbackClients);

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
                serial.SendSized(ParseArray(value));
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


        static byte[] ParseArray(string item)
        {
            var serialized = item.Replace("[", "").Replace("]", "");
            var bytesString = serialized.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            var bytes = new byte[bytesString.Length];
            for (var i = 0; i < bytesString.Length; i++)
            {
                bytes[i] = Convert.ToByte(bytesString[i]);
            }
            return bytes;
        }

        static string JsonArray(byte[] data)
        {
            var bytes = new string[data.Length];
            for (var i = 0; i < data.Length; i++)
            {
                bytes[i] = data[i] + "";
            }
            var str = String.Join(",", bytes);
            return "[" + str + "]";
        }

          
        private static void server_socketClosed(object sender, EventArgs e)
        {
            Console.WriteLine("Sock client dis connected");
        }

        //--------------------------------------------------------------------------------------------------


        static void tcpService_OnClientState(int clientId, string message)
        {
            Console.WriteLine("TCP " + clientId + " " + message);
        }


        static void tcpService_OnData(int clientId, byte[] data)
        {
            var txt = Encoding.UTF8.GetString(data);
            var items = txt.Split(';');
            
                Console.WriteLine("TCP " + clientId + " --> " + items.Length);
            
            foreach (var item in items){
                if (item.Length > 0)
                {

                    if (serial.State >= UART.EDeviceState.PortOpen && serial.State < UART.EDeviceState.Offline)
                    {
                        if (items.Length <= 20)
                        {
                           Console.WriteLine("          " + item);
                        }
                        var serialized = item.Replace("[", "").Replace("]", "");
                        var bytesString = serialized.Split(new string[]{","}, StringSplitOptions.RemoveEmptyEntries);
                        var bytes = new byte[bytesString.Length];
                        for (var i = 0; i < bytesString.Length; i++)
                        {
                            bytes[i] = Convert.ToByte(bytesString[i]);
                        }
                        serial.SendSized(bytes);
                    }
                    var sessions = socketServer.GetAllSessions();
                    var segment = getMessage("to-uart-data", item);
                    foreach (var session in sessions)
                    {
                        session.Send(segment);
                    }
                }
            }
        }

        //--------------------------------------------------------------------------------------------------


        private static void serial_OnError(byte[] data)
        {
            if (socketServer != null && socketServer.State == SuperSocket.SocketBase.ServerState.Running)
            {
                Console.WriteLine("UART ERROR " + data.Length);
                var sessions = socketServer.GetAllSessions();
                var segment = getMessage("uart-error", JsonArray(data));
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
            Console.WriteLine("UART << " + data.Length);
            if (socketServer != null && socketServer.State == SuperSocket.SocketBase.ServerState.Running)
            {
                var sessions = socketServer.GetAllSessions();
                var segment = getMessage("from-uart-data", JsonArray(data));
                foreach (var session in sessions)
                {
                    session.Send(segment);
                }
            }
            if (tcpService != null)
            {
                Console.WriteLine("UART << " + data.Length);
                tcpService.Send(data);
            }
        }
    }
}
