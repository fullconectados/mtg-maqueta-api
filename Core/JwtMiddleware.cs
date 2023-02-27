using metrogas.api.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks; 


namespace metrogas.api.Core
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next; 

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next; 
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/api"))
            {
                if (!context.Request.Path.ToString().ToLower().Contains("informarlistadopromocionesnotification"))
                {

                    var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                    if (token == null)
                    {
                        await MensajeError("no autorizado", ref context);
                        return;
                    }
                    var tmp = new CoreRequest() { Data = token.Trim() };
                    try
                    {
                        var data = tmp.GetObject<_SecurityModel>();
                        if (data.Fecha.AddMinutes(2) < DateTime.Now)
                        {
                            await MensajeError("token no valido", ref context);
                            return;
                        }
                    }
                    catch
                    {
                        await MensajeError("token no valido", ref context);
                        return;
                    }
                }
            }
            await _next(context);
        }

        private Task MensajeError(string mensaje, ref HttpContext context)
        {
            var hoy = DateTime.Now.AddHours(-4);
            /* LOG DE MENSAJE DE ERROR */
            try
            {
                var pathBuilt = Path.Combine(Directory.GetCurrentDirectory(), "SIN_AUTORIZACION");

                if (!Directory.Exists(pathBuilt))
                {
                    Directory.CreateDirectory(pathBuilt);
                }
                var fileName = DateTime.Now.ToString("yyyyMMdd") + ".txt";
                var path = Path.Combine(Directory.GetCurrentDirectory(), "SIN_AUTORIZACION", fileName);

                using (var stream = new FileStream(path, FileMode.Append))
                {
                    byte[] dato1 = new UTF8Encoding(true).GetBytes(string.Format("\n------------------{0:HH:mm:ss}------------------ \n", hoy));
                    stream.Write(dato1, 0, dato1.Length);

                    byte[] dato2 = new UTF8Encoding(true).GetBytes(mensaje + "\n" + context.Request.Path + "\n" + context.Request.Method);
                    stream.Write(dato2, 0, dato2.Length);
                    try
                    {
                        byte[] dato2a = new UTF8Encoding(true).GetBytes("IP: " + context.Request.HttpContext.Connection.RemoteIpAddress.ToString());
                        stream.Write(dato2a, 0, dato2a.Length);
                    }
                    catch { }
                    byte[] dato3 = new UTF8Encoding(true).GetBytes("\n=====================================================================================\n");
                    stream.Write(dato3, 0, dato3.Length);
                }
            }
            catch { }

            context.Response.Clear();
            context.Response.ContentType = "text/json";
            context.Response.StatusCode = 200; // (int)HttpStatusCode.Unauthorized;
            return context.Response.WriteAsync(JsonSerializer.Serialize(CoreResponse.SetError(mensaje)));
        }

    }
}
