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
    public class SmallHttpServer
    {
        protected Queue<string> Messages;

        public event OnHttpDataHandler OnData;
        public event OnHttpConnectHandler OnConnect;

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
        
        public SmallHttpServer(int port)
        {
            this.port = port;
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
            }
            worker.RunWorkerAsync(port);
        }

        public void Stop()
        {
            if (listener.IsListening)
            {
                listener.Stop();
            }
            worker.CancelAsync();
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            var worker = (BackgroundWorker)sender;
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
            if (OnConnect != null)
            {
                OnConnect(context);
            }
            if (context.Request.HttpMethod == "GET")
            {
                SendData(context);
                context.Response.OutputStream.Flush();
                context.Response.StatusCode = 200;
                context.Response.Close();
                return;
            }
            if (context.Request.HttpMethod == "POST")
            {
                ReceiveData(context);
                context.Response.OutputStream.Flush();
                context.Response.StatusCode = 200;
                context.Response.Close();
                return;
            }
            context.Response.StatusCode = 401;
            context.Response.Close();
        }

        public void Send(string data)
        {
            if (Messages.Count < 100)
            {
                Messages.Enqueue(data);
            }
        }

        public byte CharToByte(char item)
        {
            return Encoding.UTF8.GetBytes(item + "")[0];
        }

        private void SendData(HttpListenerContext context)
        {
            if (Messages.Count > 0)
            {
                context.Response.OutputStream.WriteByte(CharToByte('['));
                string item;
                while((item = Messages.Dequeue()) != null){
                    var data = Encoding.UTF8.GetBytes(item);
                    context.Response.OutputStream.Write(data, 0, data.Length);
                    if (Messages.Count > 0) 
                        context.Response.OutputStream.WriteByte(CharToByte(','));
                }
                context.Response.OutputStream.WriteByte(CharToByte(']'));
            }
        }


        private void ReceiveData(HttpListenerContext context)
        {
            var reader = new StreamReader(context.Request.InputStream, true);
            var data = reader.ReadToEnd();
            reader.Close();
            if (OnData != null)
            {
                OnData(data);
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