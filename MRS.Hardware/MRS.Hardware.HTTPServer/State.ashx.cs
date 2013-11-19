using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using MRS.Hardware.Server;

namespace MRS.Hardware.HTTPServer
{
    public class State : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/json";
            context.Response.AddHeader("Access-Control-Allow-Methods", "POST,GET,OPTIONS");
            context.Response.AddHeader("Access-Control-Request-Header", "X-Prototype-Version, x-requested-with");
            //context.Response.Headers.Add("Access-Control-Allow-Origin", "*");

            var dt = DateTime.Now.AddSeconds(1);
            foreach (var source in UART_Server.ReceiveBuffer.Where(pair => pair.Key > dt))
            {
                context.Response.Write(source + ",");    
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
