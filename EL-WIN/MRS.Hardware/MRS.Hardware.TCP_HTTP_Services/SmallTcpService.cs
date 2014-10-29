using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Sockets;
using System.Threading;

namespace MRS.Hardware.CommunicationsServices
{    
    public class SmallTcpService
    {
        protected List<Socket> sockets;
        protected List<System.Net.IPEndPoint> endPoints = new List<System.Net.IPEndPoint>();

        public event OnTcpDataHandler OnData;
        public event OnClientStateHandler OnClientState;

        public SmallTcpService()
        {
           sockets = new List<Socket>();           
        }

        public void Start(string[] connectionPool)
        {
            for (var i = 0; i < connectionPool.Length; i++)
            {
                var client = connectionPool[i];
                var parts = client.Split(':');
                try
                {
                    Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    if (OnClientState != null)
                    {
                        OnClientState(-1, client + " connecting");
                    }
                    ConnectTcpClient(socket, parts[0], Convert.ToInt32(parts[1]));
                }
                catch (Exception error)
                {
                    Console.WriteLine(error);
                }
            }
        }

        public int ConnectTcpClient(Socket socket, string host, int port)
        {
            socket.SendTimeout = 100;
            var ips = System.Net.Dns.GetHostAddresses(host);
                for (var i = 0; i < ips.Length; i++)
                {
                    if (ips[i].AddressFamily == AddressFamily.InterNetwork)
                    {
                        endPoints.Add(new System.Net.IPEndPoint(ips[i], port));
                        int clientId = sockets.Count;
                        sockets.Add(socket);
                        var client = new Thread(ClientThreadFunction);
                        client.Start(clientId);
                        return clientId;
                    }
                }
            return -1;
        }

        public void Send(int clientId, byte[] data)
        {
            if (clientId >= 0 && clientId <= sockets.Count)
            {
                for (var i = 0; i < sockets.Count; i++)
                {
                    var socket = sockets[i];
                    socket.Poll(1000, SelectMode.SelectWrite);
                    if (socket.Connected)
                    {
                        socket.Send(data);
                    }
                    else
                    {
                        sockets.Remove(socket);
                    }
                }
            }
        }

        public void Send(byte[] data)
        {
            this.Send(0, data);
        }
        
        private void ClientThreadFunction(object obj)
        {
            var clientId = (int)obj;
            var ep = endPoints[clientId];
            Socket socket = sockets[clientId];
            try
            {
                socket.Connect(ep);
            }
            catch (Exception error)
            {
                if (OnClientState != null)
                {
                    OnClientState(clientId + 1, error.ToString());
                    endPoints.Remove(ep);
                    sockets.Remove(socket);
                    return;
                }
            }
            if (OnClientState != null)
            {
                OnClientState(clientId + 1, "connected");
            }
            while (Thread.CurrentThread.ThreadState == ThreadState.Running && socket.Connected)
            {
                byte[] buf = null;
                if (socket.Available > 0)
                {
                    try
                    {
                        
                        buf = new byte[socket.Available];
                        socket.Receive(buf);
                        if (OnData != null)
                        {
                            OnData(clientId + 1, buf);
                        }
                    }
                    catch (TimeoutException)
                    {
                    }
                }
                socket.Poll(100, SelectMode.SelectRead);
                Thread.Sleep(20);
            }
            if (OnClientState != null)
            {
                OnClientState(clientId + 1, "disconnect");
            }
            socket.Close();
            endPoints.Remove(ep);
            sockets.Remove(socket);
        }

        public void Close()
        {
            for (var i = 0; i < sockets.Count; i++)
            {
                sockets[i].Close();
            }
        }
    }
}