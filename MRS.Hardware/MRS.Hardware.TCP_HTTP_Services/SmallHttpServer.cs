using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Security.Authentication.ExtendedProtection;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using MRS.Hardware.UART;

namespace MRS.Hardware.CommunicationsServices
{
    public enum HttpClientState
    {
        Noclient = 0,
        Connected = 1,
        Online = 2,
        Disconnected = 10,
        Offline = 100
    }

    public delegate void OnHttpClientStateHandler(HttpClientState state);

    public class HttpToUartBridge
    {
        public event OnHttpClientStateHandler OnClientState;

        public HttpClientState ClientState;

        private HttpListener uartListener;
        private BackgroundWorker worker;
        private int port;

        public HttpToUartBridge(int portNum, Serial uart)
        {
            this.port = portNum;
            ClientState = HttpClientState.Noclient;
            OnClientState += OnOnClientState;
            //CncController.OnMessage += CncControllerOnOnMessage;
            if (worker == null)
            {
                worker = new BackgroundWorker();
                worker.DoWork += worker_DoWork;
                worker.WorkerSupportsCancellation = true;
            }
            Open();
        }

        public void Open()
        {
            if (uartListener == null)
            {
                uartListener = new HttpListener();
                uartListener.Prefixes.Add("http://+:" + this.port + "/");
            }
            if (!uartListener.IsListening) {
                uartListener.Start();
            }
            if (worker != null)
            {
                worker.CancelAsync();
                while (worker.IsBusy)
                {

                }
            }
            worker.RunWorkerAsync();
        }

        private void OnOnClientState(HttpClientState state)
        {
            ClientState = state;
        }

        public void Close()
        {
            if (worker != null)
            {
                worker.CancelAsync();
            }
            if (uartListener != null)
            {
                uartListener.Close();
                uartListener = null;
            }
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
          /*  var worker = (BackgroundWorker)sender;
            while (!worker.CancellationPending && uartListener != null && uartListener.IsListening)
            {
                var context = uartListener.GetContext();
                if (worker.CancellationPending || !uartListener.IsListening)
                {
                    context.Response.Close();
                    break;
                }
                if (OnClientState != null)
                {
                    OnClientState(HttpClientState.Connected);
                }
                if (Uart.State < EDeviceState.Online)
                {
                    Thread.Sleep(100);
                    continue;
                }
                try
                {
                    var context = uartListener.GetContext();
                    if (worker.CancellationPending || !uartListener.IsListening)
                    {
                        context.Response.Close();
                        break;
                    }
                    if (OnClientState != null)
                    {
                        OnClientState(HttpClientState.Connected);
                    }
                    context.Response.AddHeader("Access-Control-Allow-Methods", "POST,GET,OPTIONS");
                    context.Response.AddHeader("Access-Control-Request-Header", "X-Prototype-Version, x-requested-with");
                    context.Response.AddHeader("Access-Control-Allow-Origin", "*");
                    context.Response.ContentType = "text/json";
                    context.Response.ContentEncoding = Encoding.UTF8;
                    context.Response.SendChunked = true;
                    context.Response.KeepAlive = false;
                    context.Response.OutputStream.Flush();
                    if (OnClientState != null)
                    {
                        OnClientState(HttpClientState.Online);
                    }
                    CncController.GetStateAsync();
                    MotorCommand outCommand = null;
                    CncProgramState? programState = null;
                    MotorState stateMessage = null;
                    var timeout = 1000;
                    while (timeout > 0 && !worker.CancellationPending && uartListener.IsListening)
                    {
                        //Thread.Sleep(100);
                        if (worker.CancellationPending || Uart.State < EDeviceState.Online)
                        {
                            //context.Response.Close();
                            break;
                        }
                        //timeout--;
                        List<string> states = new List<string>();
                        if (CncController.LastCommand != outCommand && CncController.LastCommand != null)
                        {
                            outCommand = CncController.LastCommand;
                            states.Add(outCommand.ToString());
                        }
                        if (!programState.HasValue || CncProgram.State != programState)
                        {
                            programState = CncProgram.State;
                            states.Add("{\"state\":\"" + programState.Value.ToString() + "\", \"line\":" +
                                       CncProgram.CurrentLine + ",\"type\" : \"program-state\"}");
                        }
                        if (CncController.LastState != null && CncController.LastState != stateMessage)
                        {
                            stateMessage = CncController.LastState;
                            states.Add(stateMessage.ToString());
                        }
                        if (states.Count > 0)
                        {
                            WriteContext(context, "[" + String.Join(",", states.ToArray()) + "]");
                        }
                        context.Response.OutputStream.Flush();
                    }
                    context.Response.Close();
                    if (OnClientState != null)
                    {
                        OnClientState(HttpClientState.Disconnected);
                    }

                }
                catch (Exception error)
                {
                    break;
                }
            }
            if (OnClientState != null)
            {
                OnClientState(HttpClientState.Offline);
            }*/
        }

        private static void WriteContext(HttpListenerContext context, string str)
        {
            var data = Encoding.UTF8.GetBytes(str);
            context.Response.OutputStream.Write(data, 0, data.Length);
        }
    }
}