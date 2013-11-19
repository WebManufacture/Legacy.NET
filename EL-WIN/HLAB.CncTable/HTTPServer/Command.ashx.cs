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
    public class CommandsHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/json";
            context.Response.AddHeader("Access-Control-Allow-Methods", "POST,GET,OPTIONS");
            context.Response.AddHeader("Access-Control-Request-Header", "X-Prototype-Version, x-requested-with");
            context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            if (context.Request.HttpMethod == "GET")
            {
                if (context.Request["command"] != null)
                {
                    try
                    {
                        byte command = byte.Parse(context.Request["command"]);
                        if (CoordMotorCommand.IsCoordCommand(command))
                        {
                            var commandObj = new CoordMotorCommand();
                            ushort x = 0;
                            ushort y = 0;
                            ushort z = 0;
                            if (ushort.TryParse(context.Request["x"], out x))
                            {
                                commandObj.X = x;
                            }
                            if (ushort.TryParse(context.Request["y"], out y))
                            {
                                commandObj.Y = y;
                            }
                            if (ushort.TryParse(context.Request["z"], out z))
                            {
                                commandObj.Z = z;
                            }
                            CncController.SendCoordCommand(commandObj);
                        }
                        else
                        {
                            CncController.SendCommand(MotorCommand.GetCommand(command));
                        }
                    }
                    catch (Exception e)
                    {
                        context.Response.StatusCode = 500;
                        context.Response.Write(e.Message);
                        return;
                    }
                }
                if (CncController.LastCommand != null)
                {
                    context.Response.Write(CncController.LastCommand.ToString());
                }
            }
            if (context.Request.HttpMethod == "POST")
            {
                try{
                    if (context.Request.ContentLength == 0) return;
                    StreamReader reader = new StreamReader(context.Request.InputStream, true);
                    MotorCommand command = JsonConvert.DeserializeObject<MotorCommand>(reader.ReadToEnd());
                    reader.Close();
                    CncController.SendCommand(command);
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
                return true;
            }
        }
    }
}
