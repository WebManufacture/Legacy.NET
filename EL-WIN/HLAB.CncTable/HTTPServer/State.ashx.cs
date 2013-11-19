using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using MRS.Hardware.Server;

namespace MRS.Hardware.HTTPServer
{
    public class LogsHandler : IHttpHandler
    {
        public MotorState StateMessage = null;
        public MotorCommand OutCommand = null;
        public CncProgramState? ProgramState = null;

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.AddHeader("Access-Control-Allow-Methods", "POST,GET,OPTIONS");
            context.Response.AddHeader("Access-Control-Request-Header", "X-Prototype-Version, x-requested-with");
            context.Response.AddHeader("Access-Control-Allow-Origin", "*");
            //context.Response.Headers.Remove("content-length");
            context.Response.Flush();
            CncController.OnMessage += Uart_OnMessage;
            CncProgram.OnCommand += Uart_OnCommand;
            CncProgram.OnStateChange += new ProgramStateHandler(Program_OnStateChange);
            CncController.GetStateAsync();
            if (CncController.LastCommand != null) OutCommand = CncController.LastCommand;
            ProgramState = CncProgram.State;
            var timeout = 10000;
            while (timeout > 0 && context.Response.IsClientConnected)
            {
                Thread.Sleep(100);
                timeout--;
                List<string> states = new List<string>();
                if (OutCommand != null)
                {
                    states.Add(OutCommand.ToString());
                    OutCommand = null;
                }
                if (ProgramState != null)
                {
                    states.Add("{\"state\":\"" + ProgramState.Value.ToString() + "\", \"line\":" + CncProgram.CurrentLine + ",\"type\" : \"program-state\"}");
                    ProgramState = null;
                }
                if (StateMessage != null)
                {
                    states.Add(StateMessage.ToString());
                    StateMessage = null;
                }
                if (states.Count > 0)
                {
                    context.Response.Write("[" + String.Join(",", states.ToArray()) + "]");
                    context.Response.Flush();
                }
            }
            /*            while (Uart.LastState == null)
                        {
                            var dt = DateTime.Now;
                            Uart.SendStateCommand();
                            var timeout = 2000;
                            while ((Uart.LastState == null || Uart.LastState.date < dt) && timeout > 0)
                            {
                                Thread.Sleep(200);
                                timeout -= 200;
                            }
                            if (Uart.LastState != null && timeout > 0)
                            {
                                context.Response.Write(Uart.LastState.ToString());
                            }
                            else
                            {
                                context.Response.StatusCode = 504;
                                context.Response.Write(" ");
                            }
                            return;
                        }
                        if (context.Request.HttpMethod == "GET")
                        {
                            var ld = DateTime.Now;
                            if (context.Request["lastdate"] != null)
                            {
                                ld = DateTime.Parse(context.Request["lastdate"]);
                                if (ld >= Uart.LastState.date && context.Request["wait"] == null)
                                {
                                    return;
                                }
                            }
                            if (context.Request["wait"] != null)
                            {
                                if (context.Request["ping"] != null)
                                {
                                    Uart.SendStateCommand();
                                }
                                var timeout = 2000;
                                while ((Uart.LastState == null || Uart.LastState.date < ld) && timeout > 0)
                                {
                                    Thread.Sleep(200);
                                    timeout -= 200;
                                }
                                if (Uart.LastState != null && timeout > 0)
                                {
                                    context.Response.Write(Uart.LastState.ToString());
                                }
                                else
                                {
                                    context.Response.StatusCode = 504;
                                }
                                return;
                            }
                            context.Response.Write(Uart.LastState.ToString());
                        }
                        if (context.Request.HttpMethod == "POST")
                        {

                        }*/
        }

        void Program_OnStateChange(CncProgramState state)
        {
            ProgramState = state;
        }

        private void Uart_OnCommand(MotorCommand command)
        {
            OutCommand = command;
        }

        void Uart_OnMessage(MotorState state)
        {
            StateMessage = state;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
