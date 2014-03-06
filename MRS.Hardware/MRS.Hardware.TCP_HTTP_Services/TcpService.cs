using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Sockets;
using System.Threading;

namespace MRS.Hardware.CommunicationsServices
{
    public class TcpMessage{
        public int sendingCount;
        public int clientId;
        public byte[] data;
    }

    public enum TcpServerState
    {
        Listening,
        Connected,
        Free,
        Offline
    }

    public delegate void OnTcpDataHandler(int clientId, byte[] data);

    public delegate void OnClientStateHandler(int clientId, string message);

    public delegate void OnServerStateHandler(TcpServerState state);

    public class HardwareTcpServer
    {
        protected Queue<TcpMessage> Messages;
        protected List<Thread> clients;
        protected List<Socket> sockets;

        public event OnTcpDataHandler OnData;
        public event OnClientStateHandler OnClientState;
        public event OnServerStateHandler OnServerState;
        
        private BackgroundWorker worker;

        private int tcpPort;
        
        public int Port
        {
            get
            {
                return tcpPort;
            }
        }

        public TcpServerState State;

        public string GetStateString()
        {
            return State + " " + tcpPort;
        }

        public HardwareTcpServer(int port)
        {
           this.tcpPort = port;
           State = TcpServerState.Offline;
           Messages = new Queue<TcpMessage>();
           clients = new List<Thread>();
           sockets = new List<Socket>();
           worker = new BackgroundWorker();
           worker.DoWork += new DoWorkEventHandler(worker_DoWork);
           worker.WorkerSupportsCancellation = true;
        }

        public void Start()
        {
            worker.RunWorkerAsync(tcpPort);
        }

        public string GetClientIP(int clientId)
        {
            if (clientId > 0 && clientId <= sockets.Count)
            {
                return sockets[clientId - 1].RemoteEndPoint.ToString();
            }
            return null;
        }

        public int AcceptTcpClient(Socket socket)
        {
            socket.SendTimeout = 100;
            int clientId = sockets.Count;
            sockets.Add(socket);
            var client = new Thread(ClientThreadFunction);
            client.Start(clientId);
            clients.Add(client);
            State = TcpServerState.Connected;
            if (OnServerState != null)
            {
                OnServerState(State);
            } 
            if (OnClientState != null)
            {
                OnClientState(clientId + 1, "connected");
            }
            return clientId;
        }

        public void Send(int clientId, byte[] data)
        {
            if (clientId >= 0 && clientId <= sockets.Count)
            {
                var message = new TcpMessage();
                message.clientId = clientId;
                message.data = data;
                message.sendingCount = sockets.Count;
                Messages.Enqueue(message);
            }
        }

        public void Send(byte[] data)
        {
            this.Send(0, data);
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            var worker = (BackgroundWorker) sender;
            System.Net.Sockets.TcpListener listener = new TcpListener((int)e.Argument);
            listener.Start();
            State = TcpServerState.Listening;
            if (OnServerState != null)
            {
                OnServerState(State);
            }
            while (!worker.CancellationPending)
            {
                while (!listener.Pending())
                {
                    Thread.Sleep(300);
                    if (worker.CancellationPending)
                    {
                        listener.Stop();
                        State = TcpServerState.Free;
                        if (OnServerState != null)
                        {
                            OnServerState(State);
                        }
                        return;
                    }
                }
                AcceptTcpClient(listener.AcceptSocket());
            }
            State = TcpServerState.Offline;
            if (OnServerState != null)
            {
                OnServerState(State);
            } 
            listener.Stop();
        }

        private void ClientThreadFunction(object obj)
        {
            var clientId = (int)obj;
            Socket socket = sockets[clientId];
            TcpMessage lastMessage = null;
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
                var result = true;
                while (Messages.Count > 0 && !worker.CancellationPending)
                {
                    try
                    {
                        var message = Messages.Peek();
                        if (lastMessage != message)
                        {
                            lock (message)
                            {
                                lastMessage = message;
                                socket.Send(message.data, message.data.Length, SocketFlags.None);
                                message.sendingCount++;
                                if (message.sendingCount >= sockets.Count)
                                {
                                    Messages.Dequeue();
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                        result = false;
                        break;
                    }
                }
                if (!result)
                {
                    break;
                }
                Thread.Sleep(100);
            }
            if (OnClientState != null)
            {
                OnClientState(clientId + 1, "disconnect");
            }
            socket.Close();
            sockets.Remove(socket);
            clients.RemoveAt(clientId);
        }

        public void Close()
        {
            Messages.Clear();
            worker.CancelAsync();
            for (var i = 0; i < clients.Count; i++)
            {
                clients[i].Abort();
            }
        }
    }
}