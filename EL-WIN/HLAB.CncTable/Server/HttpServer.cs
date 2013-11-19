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

namespace MRS.Hardware.Server
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

    public class HttpServer
    {
        public static event OnHttpClientStateHandler OnClientState;

        public static HttpClientState ClientState;

        private static HttpListener stateListener;
        private static HttpListener commandListener;
        private static BackgroundWorker stateWorker;
        private static BackgroundWorker commandWorker;
        private static string prefix = null;

        public static void Init(string prefix)
        {
            HttpServer.prefix = prefix;
            ClientState = HttpClientState.Noclient;
            OnClientState += OnOnClientState;
            stateListener = new HttpListener();
            stateListener.Prefixes.Add(prefix + "/state/");
            commandListener = new HttpListener();
            commandListener.Prefixes.Add(prefix + "/command/");
            //CncController.OnMessage += CncControllerOnOnMessage;
            if (stateWorker == null)
            {
                stateWorker = new BackgroundWorker();
                stateWorker.DoWork += stateWorker_DoWork;
                stateWorker.WorkerSupportsCancellation = true;
            }
            if (commandWorker == null)
            {
                commandWorker = new BackgroundWorker();
                commandWorker.DoWork += commandWorker_DoWork;
                commandWorker.WorkerSupportsCancellation = true;
            }
        }

        public static void Open()
        {
            if (stateListener == null)
            {
                stateListener = new HttpListener();
                stateListener.Prefixes.Add(prefix + "/state/");
            }
            if (!stateListener.IsListening) {
                stateListener.Start();
            }
            if (commandListener == null)
            {
                commandListener = new HttpListener();
                commandListener.Prefixes.Add(prefix + "/command/");
            }
            if (!commandListener.IsListening)
            {
                commandListener.Start();

            }
            if (stateWorker != null)
            {
                stateWorker.CancelAsync();
                while (stateWorker.IsBusy)
                {

                }
            }
            stateWorker.RunWorkerAsync();
            if (commandWorker != null)
            {
                commandWorker.CancelAsync();
                while (commandWorker.IsBusy)
                {

                }
            }
            commandWorker.RunWorkerAsync();
        }

        private static void OnOnClientState(HttpClientState state)
        {
            ClientState = state;
        }

        public static void Close()
        {
            if (commandWorker != null)
            {
                commandWorker.CancelAsync();
            }
            if (stateWorker != null)
            {
                stateWorker.CancelAsync();
            }
            if (stateListener != null)
            {
                stateListener.Close();
                stateListener = null;
            }
            if (commandListener != null)
            {
                commandListener.Close();
                commandListener = null;
            }
        }

        private static void stateWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var worker = (BackgroundWorker)sender;
            while (!worker.CancellationPending && stateListener.IsListening)
            {
                if (Uart.State < EDeviceState.Online)
                {
                    Thread.Sleep(100);
                    continue;
                }
                try
                {
                    var context = stateListener.GetContext();
                    if (worker.CancellationPending || !stateListener.IsListening)
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
                    while (timeout > 0 && !worker.CancellationPending && stateListener.IsListening)
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
            }
        }

        private static void WriteContext(HttpListenerContext context, string str)
        {
            var data = Encoding.UTF8.GetBytes(str);
            context.Response.OutputStream.Write(data, 0, data.Length);
        }


        static void commandWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var worker = (BackgroundWorker)sender;
            while (!worker.CancellationPending && commandListener.IsListening)
            {
                if (Uart.State < EDeviceState.Online)
                {
                    Thread.Sleep(100);
                    continue;
                }
                try
                {
                    var context = commandListener.GetContext();
                    if (worker.CancellationPending || !commandListener.IsListening)
                    {
                        context.Response.Close();
                        break;
                    }
                    context.Response.ContentType = "text/json";
                    context.Response.AddHeader("Access-Control-Allow-Methods", "POST,GET,OPTIONS");
                    context.Response.AddHeader("Access-Control-Request-Header", "X-Prototype-Version, x-requested-with");
                    context.Response.AddHeader("Access-Control-Allow-Origin", "*");
                    if (context.Request.HttpMethod == "GET")
                    {
                        if (context.Request.QueryString["command"] != null)
                        {
                            CncProgram.DebugMode = context.Request.QueryString["debug"] != null;
                            var command = context.Request.QueryString["command"];
                            if (command == "pause")
                            {
                                CncController.Pause();
                            }
                            if (command == "resume")
                            {
                                CncController.Resume();
                            }
                            if (command == "stop")
                            {
                                CncController.Stop();
                            }
                        }
                        if (CncProgram.Exists)
                        {
                            WriteContext(context, JsonConvert.SerializeObject(CncProgram.Commands));
                        }
                    }
                    if (context.Request.HttpMethod == "POST")
                    {
                        try
                        {
                            if (context.Request.ContentLength64 == 0) return;
                            StreamReader reader = new StreamReader(context.Request.InputStream, true);
                            var str = reader.ReadToEnd();
                            reader.Close();
                            var ctype = context.Request.QueryString["type"];
                            if (ctype == "code" || ctype == "program")
                            {
                                CoordMotorCommand[] scommands = JsonConvert.DeserializeObject<CoordMotorCommand[]>(str);
                                if (scommands == null || scommands.Length == 0) return;
                                List<MotorCommand> commands = new List<MotorCommand>();
                                foreach (var scommand in scommands)
                                {
                                    if (CoordMotorCommand.IsCoordCommand(scommand.Command))
                                    {
                                        commands.Add(scommand);
                                    }
                                    else
                                    {
                                        commands.Add(new MotorCommand(scommand.Command){ Line = scommand.Line });
                                    }
                                }
                                CncProgram.NewProgram(commands);
                                CncProgram.DebugMode = context.Request.QueryString["debug"] != null;
                                CncProgram.Run();
                            }
                            if (ctype == "single")
                            {
                                var command = JsonConvert.DeserializeObject<CoordMotorCommand>(str);
                                if (CoordMotorCommand.IsCoordCommand(command.Command))
                                {
                                    CncController.SendCommand(command);
                                }
                                else
                                {
                                    CncController.SendCommand(new MotorCommand(command.Command) { Line = command.Line });
                                }
                            }
                        }
                        catch (Exception err)
                        {
                            context.Response.StatusCode = 500;
                            WriteContext(context, err.Message);
                        }
                    }
                    context.Response.Close();
                }
                catch (Exception)
                {
                    break;
                }
            }
        }
    }
}