using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using MRS.Hardware.Server;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MRS.Hardware.HTTPServer
{
    public class ProgramHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/json";
            context.Response.AddHeader("Access-Control-Allow-Methods", "POST,GET,OPTIONS");
            context.Response.AddHeader("Access-Control-Request-Header", "X-Prototype-Version, x-requested-with");
            //context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            if (context.Request.HttpMethod == "GET")
            {
                if (CncProgram.Exists)
                {
                    if (context.Request["command"] != null)
                    {
                        CncProgram.DebugMode = context.Request["debug"] != null;
                        var command = context.Request["command"];
                        if (command == "pause")
                        {
                            CncProgram.Pause();
                        }
                        if (command == "resume")
                        {
                            CncProgram.Resume();
                        }
                        if (command == "stop")
                        {
                            CncProgram.Stop();
                        }
                        if (command == "run")
                        {
                            CncProgram.Run();
                        }
                    }
                    context.Response.Write(JsonConvert.SerializeObject(CncProgram.Commands));
                }
            }
            if (context.Request.HttpMethod == "POST")
            {
                try{
                    if (context.Request.ContentLength == 0) return;
                    StreamReader reader = new StreamReader(context.Request.InputStream, true);
                    MotorCommand[] commands = JsonConvert.DeserializeObject<MotorCommand[]>(reader.ReadToEnd());
                    reader.Close();
                    if (commands == null || commands.Length == 0) return;
                    CncProgram.NewProgram(commands);
                    CncProgram.DebugMode = context.Request["debug"] != null;
                    CncProgram.Run();
                }
                catch(Exception e)
                {
                    context.Response.StatusCode = 500;
                    context.Response.Write(e.Message);
                }
            }
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
