using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace MRS.Hardware.CommunicationsServices
{
    public enum HttpServerState
    {
        Listening,
        Connected,
        Free,
        Offline
    }

    public delegate void OnHttpDataHandler(HttpListenerContext context, string data);
    public delegate void OnHttpConnectHandler(HttpListenerContext context);
    public delegate void OnHttpServerStateHandler(HttpServerState state);

    public class HardwareHttpServer
    {
        protected Queue<string> Messages;

        public event OnHttpDataHandler OnData;
        public event OnHttpConnectHandler OnConnect;
        public event OnHttpServerStateHandler OnServerState;

        private HttpListener listener;
        
        private BackgroundWorker worker;

        private int port;
        
        public int Port
        {
            get
            {
                return port;
            }
        }

        private HttpServerState state;

        public HttpServerState State
        {
            get
            {
                return state;
            }
            set
            {
                state = value;
                if (OnServerState != null)
                {
                    OnServerState(value);
                }
            }
        }

        public string GetStateString()
        {
            return State + " " + port;
        }

        public HardwareHttpServer(int port)
        {
           this.port = port;
           state = HttpServerState.Offline;
           Messages = new Queue<string>();
           listener = new HttpListener();
           listener.Prefixes.Add("http://+:" + this.port + "/");
           listener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
           worker = new BackgroundWorker();
           worker.DoWork += new DoWorkEventHandler(worker_DoWork);
           worker.WorkerSupportsCancellation = true;
        }

        public void Start()
        {
            if (!listener.IsListening)
            {
                listener.Start();
                State = HttpServerState.Listening;
            }
            worker.RunWorkerAsync(port);
        }

        public void Stop()
        {
            if (listener.IsListening)
            {
                listener.Stop();
                State = HttpServerState.Offline;
            }
            worker.CancelAsync();
        }

        public void AcceptClient(HttpListenerContext context)
        {
            context.Response.AddHeader("Access-Control-Allow-Methods", "POST,GET,OPTIONS");
            context.Response.AddHeader("Access-Control-Request-Header", "X-Prototype-Version, x-requested-with");
            context.Response.AddHeader("Access-Control-Allow-Origin", "*");
            context.Response.ContentType = "text/json";
            context.Response.StatusCode = 200;
            context.Response.ContentEncoding = Encoding.UTF8;
            context.Response.SendChunked = false;
            context.Response.KeepAlive = false;
            context.Response.OutputStream.Flush();
            if (OnConnect != null){
                OnConnect(context);
            }
            if (context.Request.HttpMethod == "GET"){
                var client = new Thread(GetThreadFunction);
                client.Start(context);
                State = HttpServerState.Connected;
                return;
            }
            if (context.Request.HttpMethod == "POST"){
                ReceiveData(context);
                return;
            }
            context.Response.StatusCode = 401;
            context.Response.Close();
        }

        public void Send(string data)
        {
            if (Messages.Count < 100){
                Messages.Enqueue(data);
            }
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            var worker = (BackgroundWorker) sender;
            while (!worker.CancellationPending && listener.IsListening)
            {
                try
                {
                    var context = listener.GetContext();                    
                    AcceptClient(context);
                }
                catch (Exception err)
                {
                    Thread.Sleep(100);
                }
            }
        }

        private void GetThreadFunction(object obj)
        {
            HttpListenerContext context = (HttpListenerContext)obj;
            var hb = 0;
            while (Thread.CurrentThread.ThreadState == ThreadState.Running)
            {
                while (Messages.Count > 0)
                {
                    Thread.Sleep(50);
                    var message = Messages.Dequeue();
                    try
                    {
                        var data = Encoding.UTF8.GetBytes(message);
                        context.Response.OutputStream.Write(data, 0, data.Length);
                        context.Response.OutputStream.Flush();
                    }
                    catch (Exception)
                    {
                        break;
                    }
                }
                Thread.Sleep(100);
                hb++;
                if (hb >= 30)
                {
                    hb = 0;
                    try
                    {
                        context.Response.OutputStream.WriteByte(00);
                        context.Response.OutputStream.Flush();
                    }
                    catch (Exception)
                    {
                        State = HttpServerState.Free;
                        return;
                    }
                }
            }
            State = HttpServerState.Free;
            context.Response.Close();
        }

        private void ReceiveData(HttpListenerContext context)
        {
            var reader = new StreamReader(context.Request.InputStream, true);
            var data = reader.ReadToEnd();
            reader.Close();
            context.Response.Close();
            if (OnData != null)
            {
                OnData(context, data);
            }
        }

        public void Close()
        {
            Messages.Clear();
            worker.CancelAsync();
            listener.Close();
        }
    }
}