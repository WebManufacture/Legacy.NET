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
using System.IO;

namespace MRS.Hardware.UART_HTTP_bribge
{
    public enum SerialDirection : byte
    {
        Error = 0,
        ToUART = 1,
        FromUART = 2
    }

    public struct SerialPacketSerialization
    {
        public SerialPacketSerialization(SerialPacket packet, SerialDirection direction)
        {
            PortName = packet.PortName;
            Direction = direction;
            Data = SerialPacket.SerializeData(packet.Data);
            Time = DateTime.Now;
        }

        public SerialPacketSerialization(string portName, byte[] data, SerialDirection direction)
        {
            PortName = portName;
            Direction = direction;
            Data = SerialPacket.SerializeData(data);
            Time = DateTime.Now;
        }

        public DateTime Time;
        public string PortName;
        public SerialDirection Direction;
        public string Data;

        public static SerialPacketSerialization Deserialize(string data)
        {
            return (SerialPacketSerialization)JsonConvert.DeserializeObject(data);
        }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class SerialPacket
    {/*
        public SerialPacket(){
            PortName = "";
            Data = null;
            Direction = SerialDirection.FromUART;
        }
*/
        public SerialPacket(string port, byte[] data)
        {
            PortName = port;
            Data = data;
        }

        public string PortName = "";
        public byte[] Data = null;


        public static SerialPacket Deserialize(string data)
        {
            return (SerialPacket)JsonConvert.DeserializeObject(data);
        }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static string SerializeData(byte[] Data)
        {
            if (Data == null) return "[]";
            var str = "[";
            for (var i = 0; i < Data.Length; i++)
            {
                str += Data[i] + ",";
            }
            str = str.TrimEnd(',') + "]";
            return str;
        }
    }

    public struct ServerInfo
    {
        public string ServerId;
        public DateTime Started;
        public TimeSpan Uptime
        {
            get
            {
                return DateTime.Now - Started;
            }
        }
        public string HttpAddr;
        public int HttpPort;
        public string SocketAddr;
        public int SocketPort;
        public string HttpInterface { get { return HttpAddr + ":" + HttpPort; } }
        public string SocketInterface { get { return SocketAddr + ":" + SocketPort; } }
        public byte SelfAddress;
        public List<string> Serials;
        public List<SerialConfig> Configs;
        public List<string> CallbackClients;        
    }

    class Program
    {
        static SuperWebSocket.WebSocketServer socketServer;
        static SmallTcpService tcpService;
        static SmallHttpServer httpServer;
        static List<SerialConfig> ComPortConfigs;
        static ServerInfo serverInfo;
        static Dictionary<string, SerialManager> Serials = new Dictionary<string, SerialManager>();
        static string SocketMsg = "{\"type\":\"{0}\", \"data\":{1} }";
        static Timer CallbackHeartBeatTimer;
        static int Main(string[] args)
        {
            Console.BufferHeight = 10000;
            Console.BufferWidth = 160;
            Console.WindowWidth = 160;
            Console.WindowHeight = 60;


            var reader = new StreamReader("Config.js");
            var cfgFile = reader.ReadToEnd();
            reader.Close();

            serverInfo = JsonConvert.DeserializeObject<ServerInfo>(cfgFile);
            serverInfo.Started = DateTime.Now;

            var callbackClients = serverInfo.CallbackClients;
            
            ComPortConfigs = new List<SerialConfig>();
            
            foreach (var cfg in serverInfo.Configs)
            {
                if (AddPort(cfg))
                {
                    ComPortConfigs.Add(cfg);
                }
            }

            Log("HTTP: " + serverInfo.HttpInterface);
            httpServer = new SmallHttpServer(serverInfo.HttpPort);
            httpServer.OnConnect += httpServer_OnConnect;
            httpServer.OnData += httpServer_OnData;
            httpServer.Start();

            Log("WEB-SOCKET: " + serverInfo.SocketInterface);
            socketServer = new WebSocketServer();
            socketServer.Setup(serverInfo.SocketPort);
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
            tcpService.Start(callbackClients.ToArray());

            //CallbackHeartBeatTimer = new Timer(


            Thread.CurrentThread.Join();
                        
            foreach (var serial in Serials)
            {
                if (serial.Value != null)
                {
                    serial.Value.Close();
                }
            }

            httpServer.Close();
            socketServer.Stop();
            tcpService.Close();

            return 0;
        }


        protected static string GetServerInfo()
        {
            serverInfo.Serials = SerialManager.GetPorts().ToList();
            var cfg = JsonConvert.SerializeObject(serverInfo);
            return cfg;
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
                    break;
                case PacketType.SizedOld:
                    sm = new SerialPacketManager(cfg);
                    break;
                case PacketType.Addressed:
                    sm = new SerialAddressedManager(cfg);
                    (sm as SerialAddressedManager).DeviceAddr = 0xAA;
                    break;
                case PacketType.AddressedOld:
                    sm = new SerialAddressedManager(cfg);
                    break;
                case PacketType.XRouting:
                    sm = new XRoutingManager(cfg);
                    (sm as XRoutingManager).DeviceAddr = 0xAB;
                    break;
                default:
                    sm = new SerialManager(cfg);
                    break;
            }
            Log(cfg);
            sm.OnReceive += serial_OnReceive;
            sm.OnSend += sm_OnSend;
            sm.OnError += serial_OnError;
            sm.OnStateChange += serial_OnStateChange;
            if (!sm.Connect())
            {
                Warn("Can't connect " + cfg.PortName);
                sm.OnReceive -= serial_OnReceive;
                sm.OnSend -= sm_OnSend;
                sm.OnError -= serial_OnError;
                sm.OnStateChange -= serial_OnStateChange;
                return false;
            }
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
            return Send(packet.PortName, packet.Data, null, true);
        }

        private static bool Send(SerialPacket packet, WebSocketSession session)
        {
            return Send(packet.PortName, packet.Data, session, false);
        }

        private static bool Send(string portName, byte[] data, WebSocketSession thisSession, bool fromTCP)
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
                    serial.PreventSendingEvent = true;
                    serial.Send(data);
                    serial.PreventSendingEvent = false;
                    var sessions = GetSessions(serial.PortName);
                    var segment = getMessage(portName, SerialDirection.ToUART, data);
                    if (sessions != null)
                    {
                        foreach (var session in sessions)
                        {
                            if (session != thisSession)
                            {
                                session.Send(segment);
                            }
                        }
                    }
                    if (tcpService != null && !fromTCP)
                    {
                        tcpService.Send(segment);
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

        protected static string getMessage(string portName, SerialDirection direction, string data)
        {
            return getMessage(portName, direction, Encoding.ASCII.GetBytes(data));
        }

        protected static string getMessage(string portName, SerialDirection direction, byte[] data)
        {
            return (new SerialPacketSerialization(portName, data, direction)).Serialize();
        }

        private static void socketServer_NewSessionConnected(WebSocketSession session)
        {
            var portName = session.Path.Replace("/", "");
            if (portName != null && portName != "")
            {
                portName = portName.Replace("/", "");
                Info("Sock client " + session.SessionID + " connected with path " + portName);
                if (Serials.ContainsKey(portName))
                {
                    session.Path = portName;
                    return;
                }
                else
                {
                    session.Send("port not open!");
                }
            }
            else
            {
                Info("Sock client " + session.SessionID + " connected without path ");
                session.Send(JsonConvert.SerializeObject(ComPortConfigs));
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
            if (socketServer == null) return null;
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
                Send(portName, JsonConvert.DeserializeObject<byte[]>(value), null, false);
            }
            catch (Exception e)
            {
                serial_OnError(e, null);
            }
        }


        private static bool httpServer_OnConnect(HttpListenerContext context)
        {
            var portName = context.Request.Url.AbsolutePath;
            Info("HTTP client connected " + portName);
            if (portName != null && portName != "")
            {
                portName = portName.Replace("/", "");
            }
            else
            {
                var info = Encoding.UTF8.GetBytes(GetServerInfo());
                context.Response.OutputStream.Write(info, 0, info.Length);
                context.Response.Close();
                return true;
            }
            var cfg = ComPortConfigs.FirstOrDefault(s => s.PortName == portName);
            if (cfg != null)
            {
                cfg = new SerialConfig();
                cfg.PortName = portName;
                cfg.Speed = UInt32.Parse(context.Request.QueryString["speed"]);
                if (AddPort(cfg))
                {
                    ComPortConfigs.Add(cfg);
                }
            }
            var dta = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(serverInfo));
            context.Response.OutputStream.Write(dta, 0, dta.Length);
            context.Response.Close();
            return true;
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
            var item = Encoding.UTF8.GetString(data);
            try
            {
                if (item.Length > 0)
                {
                    Send(JsonConvert.DeserializeObject<SerialPacket>(item));
                }
            }
            catch (Exception err)
            {
                serial_OnError(err, null);
            }
        }

        //--------------------------------------------------------------------------------------------------


        static void sm_OnSend(byte[] data, SerialManager manager)
        {
            if (data.Length > 40)
            {
                Log(manager.PortName + " << " + data.Length);
            }
            else
            {
                var str = SerialPacket.SerializeData(data);
                Log(manager.PortName + " << " + str);
            }
            var sessions = GetSessions(manager.PortName);
            if (sessions == null) return;
            var segment = getMessage(manager.PortName, SerialDirection.ToUART, data);
            foreach (var session in sessions)
            {
                session.Send(segment);
            }
            if (tcpService != null)
            {
                tcpService.Send(segment);
            }
        }

        static void serial_OnError(Exception e, SerialManager sm)
        {
            var portName = "";
            if (sm != null)
            {
                portName = sm.PortName;
            }
            var segment = getMessage(portName, SerialDirection.Error, e.Message);
            if (socketServer != null && socketServer.State == SuperSocket.SocketBase.ServerState.Running)
            {
                Error(portName + " ERROR: " + e.Message);
                var sessions = socketServer.GetAllSessions();
                foreach (var session in sessions)
                {
                    session.Send(segment);
                }
            }
            if (tcpService != null)
            {
                tcpService.Send(segment);
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
                Log(portName + " >> " + data.Length);
            }
            else
            {
                var str = SerialPacket.SerializeData(data);
                Log(portName + " >> " + str);
            }
            var segment = getMessage(portName, SerialDirection.FromUART, data);
            if (socketServer != null && socketServer.State == SuperSocket.SocketBase.ServerState.Running)
            {
                var sessions = GetSessions(portName);
                if (sessions != null)
                {
                    foreach (var session in sessions)
                    {
                        session.Send(segment);
                    }
                }
            }
            if (tcpService != null)
            {
                tcpService.Send(segment);
            }
        }
        /*
        private static void serial_OnReceiveXRouting(XPacket packet, XRoutingManager manager)
        {
            serial_OnReceive(packet.ToBytes(), manager);
        }
        */
    }
}
