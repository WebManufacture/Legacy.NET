using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Net;
using SuperWebSocket;
using MRS.Hardware.CommunicationsServices;
using MRS.Hardware.UART;
using Newtonsoft.Json;

namespace MRS.Hardware.UART_HTTP_bribge
{
    public struct SerialPacket
    {
        public string PortName;
        public byte[] Data;
    }

    class Program
    {
        static SuperWebSocket.WebSocketServer socketServer;
        static SmallTcpService tcpService;
        static SmallHttpServer httpServer;
        static List<SerialConfig> ComPortConfigs;
        static string ServerId;
        static Dictionary<string, SerialManager> Serials = new Dictionary<string, SerialManager>();
        static string SocketMsg = "{\"type\":\"{0}\", \"data\":{1} }";
        static void Main(string[] args)
        {
            Console.BufferHeight = 10000;
            Console.BufferWidth = 160;
            Console.WindowWidth = 160;
            Console.WindowHeight = 60;
            var settings = ConfigurationManager.AppSettings;
            var httpPort = Convert.ToInt32(settings["HTTP_PORT"]);
            var wsPort = Convert.ToInt32(settings["WS_PORT"]);
            var callbacks = settings["CALLBACK_CLIENTS"];
            ServerId = settings["ServerId"];
            var comPortsConfig = JsonConvert.DeserializeObject<List<string>>(settings["COM_PORTS"]);

            var callbackClients = callbacks.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);


            ComPortConfigs = new List<SerialConfig>();

            foreach (var serial in comPortsConfig)
            {
                var cfg = SerialConfig.Parse(serial);
                if (AddPort(cfg))
                {
                    ComPortConfigs.Add(cfg);
                }
            }

            Log("HTTP: " + httpPort);
            httpServer = new SmallHttpServer(httpPort);
            httpServer.OnConnect += httpServer_OnConnect;
            httpServer.OnData += httpServer_OnData;
            httpServer.Start();

            Log("WEB-SOCKET: " + wsPort);
            socketServer = new WebSocketServer();
            socketServer.Setup(wsPort);
            socketServer.NewMessageReceived += socketServer_NewMessageReceived;
            socketServer.SessionClosed += socketServer_SessionClosed;
            socketServer.NewSessionConnected += socketServer_NewSessionConnected;
            socketServer.Start();

            /*
            serial = new UART.Serial(comPort, comPortSpeed);
            Log("COM: " + comPort + " " + comPortSpeed);
            serial.OnReceive += serial_OnReceive;
            serial.OnError += serial_OnError;
            serial.OnStateChange += serial_OnStateChange;
            serial.Connect();
            */

            tcpService = new SmallTcpService();
            tcpService.OnData += tcpService_OnData;
            tcpService.OnClientState += tcpService_OnClientState;
            tcpService.Start(callbackClients);

            Thread.CurrentThread.Join();

            httpServer.Close();
            socketServer.Stop();
            tcpService.Close();

        }

        protected static string GetServerInfo()
        {
            var ports = String.Join(",", SerialManager.GetPorts());
            var cfg = JsonConvert.SerializeObject(ComPortConfigs);
            return "{\"ServerId\": \"" + ServerId + "\",\"Serials\": \"" + ports + "\", \"Configs\":" + cfg + "}";
        }

        protected static bool AddPort(SerialConfig cfg)
        {
            if (Serials.ContainsKey(cfg.PortName))
            {
                //Serials[cfg.PortName].SetParams((uint)cfg.Speed, cfg.DataBits, cfg.StopBits, cfg.Parity);
                if (!Serials[cfg.PortName].IsOpened)
                {
                    Serials[cfg.PortName].Connect();
                }
                return false;
            }
            SerialManager sm;
            switch (cfg.RxPacketType)
            {
                case PacketType.SimpleCoded:
                case PacketType.SimpleCRC: throw new NotImplementedException("PacketType.SimpleCRC have no manager today!");
                case PacketType.Sized:
                    sm = new SerialPacketManager(cfg);
                    sm.OnReceive += serial_OnReceive;
                    break;
                case PacketType.SizedOld:
                    sm = new SerialPacketManager(cfg);
                    sm.OnReceive += serial_OnReceive;
                    break;
                case PacketType.Addressed:
                    sm = new SerialAddressedManager(cfg);
                    (sm as SerialAddressedManager).DeviceAddr = 0xAA;
                    (sm as SerialAddressedManager).OnReceive += serial_OnReceive;
                    break;
                case PacketType.AddressedOld:
                    sm = new SerialAddressedManager(cfg);
                    (sm as SerialAddressedManager).DeviceAddr = 0xAA;
                    (sm as SerialAddressedManager).OnReceive += serial_OnReceive;
                    break;
                case PacketType.XRouting:
                    sm = new XRoutingManager(cfg);
                    (sm as XRoutingManager).DeviceAddr = 0xAB;
                    (sm as XRoutingManager).OnReceiveXPacket += serial_OnReceiveXRouting;
                    break;
                default:
                    sm = new SerialManager(cfg);
                    sm.OnReceive += serial_OnReceive;
                    break;
            }
            Log(cfg);
            if (!sm.Connect())
            {
                Warn("Can't connect " + cfg.PortName);
                return false;
            }
            sm.OnStateChange += serial_OnStateChange;
            sm.OnError += serial_OnError;
            Serials.Add(cfg.PortName, sm);
            return true;
        }
        
        private static void Info(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(msg);
            Console.ResetColor();
        }

        private static void Log(object msg)
        {
            Log(msg + "");
        }

        private static void Log(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(msg);
            Console.ResetColor();
        }

        private static void Warn(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(msg);
            Console.ResetColor();
        }

        private static void Error(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(msg);
            Console.ResetColor();
        }

        private static bool Send(SerialPacket packet)
        {
            return Send(packet.PortName, packet.Data, null);
        }


        private static bool Send(SerialPacket packet, WebSocketSession session)
        {
            return Send(packet.PortName, packet.Data, session);
        }

        private static bool Send(string portName, byte[] data, WebSocketSession thisSession)
        {
            if (!Serials.ContainsKey(portName))
            {
                Error("Port " + portName + " doesn't exists");
                return false;
            }
            var serial = Serials[portName];
            try
            {

                if (serial.State >= UART.EDeviceState.PortOpen && serial.State < UART.EDeviceState.Offline)
                {
                    serial.Send(data);
                    var sessions = GetSessions(serial.PortName);
                    var segment = getMessage("to-uart-data", SerializeData(data));
                    foreach (var session in sessions)
                    {
                        if (session != thisSession)
                        {
                            session.Send(segment);
                        }
                    }
                }
            }
            catch (Exception err)
            {
                serial_OnError(err, serial);
                return false;
            }
            return true;
        }

        protected static string getMessage(string type, string data)
        {
            return SocketMsg.Replace("{0}", type).Replace("{1}", data);
        }

        private static void socketServer_NewSessionConnected(WebSocketSession session)
        {
            var portName = session.Path;
            if (portName != null && portName != "")
            {
                portName = portName.Replace("/", "");
                Info("Sock client " + session.SessionID + " connected with path " + portName);
                if (Serials.ContainsKey(portName))
                {
                    session.Path = portName;
                }
            }
            else
            {
                Info("Sock client " + session.SessionID + " connected without path ");
                session.Send(getMessage("config", JsonConvert.SerializeObject(ComPortConfigs)));
            }
            session.Close();
        }

        static void socketServer_SessionClosed(WebSocketSession session, SuperSocket.SocketBase.CloseReason value)
        {
            Warn(session.SessionID + " closed " + value);
        }

        static void socketServer_NewMessageReceived(WebSocketSession thisSession, string value)
        {
            Log("WS: " + DateTime.Now.ToString("hh:mm:ss.f") + " --> " + value);
            try
            {
                var packet = JsonConvert.DeserializeObject<SerialPacket>(value);
                Send(packet, thisSession);
            }
            catch (Exception e)
            {
                serial_OnError(e, null);
            }
        }


        private static IEnumerable<WebSocketSession> GetSessions(string portName)
        {
            return socketServer.GetSessions(s => s.Path == portName);
        }


//--------------------------------------------------------------------------------------------------
//--------------------------------HTTP           --------------------------------------------------
//--------------------------------------------------------------------------------------------------


        private static void httpServer_OnData(string value, HttpListenerContext context)
        {
            Log("HTTP " + context.Request.Url.AbsolutePath + " --> " + value);
            try
            {
                var portName = context.Request.Url.AbsolutePath;
                if (portName != null && portName != "")
                {
                    portName = portName.Replace("/", "");
                }
                Send(portName, JsonConvert.DeserializeObject<byte[]>(value), null);
            }
            catch (Exception e)
            {
                serial_OnError(e, null);
            }
        }


        private static void httpServer_OnConnect(HttpListenerContext context)
        {
            var portName = context.Request.Url.AbsolutePath;
            Info("HTTP client connected " + portName) ;
            if (portName != null && portName != "")
            {
                portName = portName.Replace("/", "");
            }
            else
            {
                var info = Encoding.UTF8.GetBytes(GetServerInfo());
                context.Response.OutputStream.Write(info, 0, info.Length);
                return;
            }
            var cfg = ComPortConfigs.FirstOrDefault(s => s.PortName == portName);
            if (cfg == null)
            {
                cfg = new SerialConfig();
                cfg.PortName = portName;
                cfg.Speed = UInt32.Parse(context.Request.QueryString["speed"]);
                if (AddPort(cfg))
                {
                    ComPortConfigs.Add(cfg);
                }
            }
            var dta = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(cfg));
            context.Response.OutputStream.Write(dta, 0, dta.Length);
            //httpServer.Send(getMessage("config", ));
        }
        

        //--------------------------------------------------------------------------------------------------


        static void tcpService_OnClientState(int clientId, string message)
        {
            if (message == "connected")
            {
                Info("TCP " + clientId + " " + message);
                tcpService.Send(GetServerInfo());
            }
            else
            {
                if (message == "connecting")
                {
                    Log("TCP " + clientId + " " + message);
                }
                else
                {
                    Warn("TCP " + clientId + " " + message);
                }
            }
        }


        static void tcpService_OnData(int clientId, byte[] data)
        {
            var txt = Encoding.UTF8.GetString(data);
            try
            {

                var items = JsonConvert.DeserializeObject<string[]>(txt);
                if (items.Length == 0) return;
                if (items[0].IndexOf('[') < 0)
                {
                    var cfg = JsonConvert.DeserializeObject<SerialConfig>(items[0]);
                    if (AddPort(cfg))
                    {
                        ComPortConfigs.Add(cfg);
                    }
                }
                Log("TCP " + clientId + " --> " + items.Length);

                foreach (var item in items)
                {
                    if (item.Length > 0)
                    {
                        var packet = JsonConvert.DeserializeObject<SerialPacket>(item);
                        if (items.Length <= 20)
                        {
                            Log("          " + item);
                        }
                        Send(packet);
                    }
                }

            }
            catch (Exception err)
            {
                serial_OnError(err, null);
            }
        }

        //--------------------------------------------------------------------------------------------------


        static void serial_OnError(Exception e, SerialManager sm)
        {
            var portName = "";
            if (sm != null)
            {
                portName = sm.PortName;
            }
            if (socketServer != null && socketServer.State == SuperSocket.SocketBase.ServerState.Running)
            {
                Error(portName + " ERROR: " + e.Message);
                var sessions = socketServer.GetAllSessions();
                var segment = getMessage("uart-error", e.Message);
                foreach (var session in sessions)
                {
                    session.Send(segment);
                }
            }
        }

        static void serial_OnStateChange(UART.EDeviceState state, SerialManager sm)
        {
            if (state == EDeviceState.Online || state == EDeviceState.PortOpen)
            {
                Info(sm.PortName + " -> " + state);
                return;
            }
            if (state == EDeviceState.Offline)
            {
                Warn(sm.PortName + " -> " + state);
                return;
            }
            if (state == EDeviceState.Error)
            {
                Error(sm.PortName + " -> " + state);
                return;
            }
            Log(sm.PortName + " -> " + state);
        }

        static void serial_OnReceive(byte[] data, SerialManager sm)
        {
            var portName = "";
            if (sm != null)
            {
                portName = sm.PortName;
            }
            if (data.Length > 40)
            {
                Log(portName + " << " + data.Length);
            }
            else
            {
                var str = SerializeData(data);
                Log(portName + " << " + str);
            }
            if (socketServer != null && socketServer.State == SuperSocket.SocketBase.ServerState.Running)
            {
                var sessions = GetSessions(portName);
                var segment = getMessage("from-uart-data", SerializeData(data));
                foreach (var session in sessions)
                {
                    session.Send(segment);
                }
            }
            if (tcpService != null)
            {
                tcpService.Send(JsonConvert.SerializeObject(new SerialPacket() { PortName = portName, Data = data }));
            }
        }
        
        private static void serial_OnReceiveXRouting(XPacket packet, XRoutingManager manager)
        {
            serial_OnReceive(packet.ToBytes(), manager);
        }

        public static string SerializeData(byte[] dta)
        {
            var str = "[";
            for (var i=0; i < dta.Length; i++)
            {
                str += dta[i] + ",";
            }
            str = str.TrimEnd(',') + "]";
            return str;
        }
    }
}
