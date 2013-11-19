using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Sockets;
using System.Threading;
using MRS.Hardware.UART;

namespace MRS.Hardware.Server
{

    public delegate void OnSendHandler(byte[] data);

    public delegate void OnClientStateHandler(string address);

    public class UartTcpService
    {
        protected Queue<byte[]> Messages;

        public event OnSendHandler OnSendData;

        public event OnClientStateHandler OnClientState;
        
        private BackgroundWorker worker;

        public UartTcpService()
        { 
           Messages = new Queue<byte[]>();
           worker = new BackgroundWorker();
           worker.DoWork += new DoWorkEventHandler(worker_DoWork);
           worker.WorkerSupportsCancellation = true;
           worker.RunWorkerAsync(12100);
           Uart.OnReceive += new OnReceiveHandler(UART_Server_OnReceive);
        }

        void UART_Server_OnReceive(byte[] data)
        {
            if (Messages.Count > 1000)
            {
                Messages.Dequeue();
            }
            Messages.Enqueue(data);
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            var worker = (BackgroundWorker) sender;
            System.Net.Sockets.TcpListener listener = new TcpListener((int)e.Argument);
            listener.Start();
            while (!worker.CancellationPending)
            {
                if (worker.CancellationPending)
                {
                    listener.Stop();
                    return;
                }
                if (OnClientState != null)
                {
                    OnClientState("Listening");
                }
                while (!listener.Pending())
                {
                    Thread.Sleep(300);
                    if (worker.CancellationPending)
                    {
                        listener.Stop();
                        return;
                    }
                }
                Socket socket = listener.AcceptSocket();
                socket.SendTimeout = 1000;
                if (OnClientState != null)
                {
                    OnClientState(socket.RemoteEndPoint.ToString());
                }
                var HeartBeat = DateTime.Now;
                var hbMaxTime = new TimeSpan(0, 0, 0, 10, 0);
                while (!worker.CancellationPending)
                {
                    if (!socket.Connected)
                    {
                        if (OnClientState != null)
                        {
                            OnClientState("");
                        } 
                        break;
                    }
                    byte[] buf = null;
                    if (socket.Available > 0)
                    {
                        try
                        {
                            buf = new byte[socket.Available];
                            socket.Receive(buf);
                            if (buf.Length == 1 && buf[0] == 0)
                            {
                                HeartBeat = DateTime.Now;
                                if (OnClientState != null)
                                {
                                    OnClientState(">: " + socket.RemoteEndPoint);
                                } 
                            }
                            else
                            {
                                if (OnSendData != null)
                                {
                                    OnSendData(buf);
                                }
                                Uart.Send(buf);
                            }
                        }
                        catch (TimeoutException)
                        {
                        }
                    }
                    if ((DateTime.Now - HeartBeat) > hbMaxTime)
                    {
                        break;
                    }
                    var result = true;
                    while (Messages.Count > 0 && !worker.CancellationPending)
                    {
                        try
                        {
                            var message = Messages.Dequeue();
                            socket.Send(message, message.Length, SocketFlags.None);
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
                socket.Close();
                if (OnClientState != null)
                {
                    OnClientState("Closing");
                } 
            }
            if (OnClientState != null)
            {
                OnClientState("STOP");
            } 
            listener.Stop();
        }

        public void Close()
        {
            Messages.Clear();
            worker.CancelAsync();
        }
    }
}