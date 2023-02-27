// ***********************************************************************
// Assembly         : metrogas.api
// Author           : Full Conectados Ltda <software@fullconectados.cl>
// Created          : 09-17-2021
//
// Last Modified By : ginom
// Last Modified On : 09-18-2021
// ***********************************************************************
// <copyright file="Startup.cs" company="Full Conectados Ltda">
//     Copyright (c) 2021. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Text.Json;
using System;
using System.Reflection;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.Collections.Generic;

namespace metrogas.api
{
    /// <summary>
    /// Class Startup.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup" /> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <value>The configuration.</value>
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        /// <summary>
        /// Configures the services.
        /// </summary>
        /// <param name="services">The services.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));
            services.AddControllers();
            services.AddRazorPages();            

            // https://github.com/dotnet/AspNetCore.Docs/tree/main/aspnetcore/tutorials/web-api-help-pages-using-swagger/samples/3.0/TodoApi.Swashbuckle
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "API - Metrogas",
                    Description = "Servicios para operacion de portal - metrogas",
                    TermsOfService = new Uri("https://metrogas.cl"),                    
                    Contact = new OpenApiContact
                    {
                        Name = "Metrogas S.A.",
                        Email = "contacto@metrogas.cl",
                        Url = new Uri("https://metrogas.cl"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under LICX",
                        Url = new Uri("https://metrogas.cl"),
                    }
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
            services.Configure<ApiBehaviorOptions>(a =>
            {
                a.InvalidModelStateResponseFactory = context =>
                {
                    var response = Core.CoreResponse.SetError(context.ModelState);
                    return new JsonResult(response);
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// <summary>
        /// Configures the specified application.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="env">The env.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseExceptionHandler(c => c.Run(async context =>
            {                
                
                var exception = context.Features.Get<IExceptionHandlerPathFeature>().Error;
                try
                {
                    var hoy = DateTime.Now.AddHours(-4);
                    var pathBuilt = Path.Combine(Directory.GetCurrentDirectory(), "ERRORES");

                    if (!Directory.Exists(pathBuilt))
                    {
                        Directory.CreateDirectory(pathBuilt);
                    }
                    var fileName = DateTime.Now.ToString("yyyyMMdd") + ".txt";
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "ERRORES", fileName);

                    var _msg = new List<string>();
                    var x = exception?.InnerException;
                    while (x != null)
                    {
                        _msg.Add(x.StackTrace);
                        x = x?.InnerException;
                    }
                    var objeto = JsonSerializer.Serialize<dynamic>(new
                    {
                        trace = exception?.StackTrace,
                        mensaje = exception?.Message,
                        inner = _msg,
                        site = exception?.TargetSite?.Name
                    });

                    using (var stream = new FileStream(path, FileMode.Append))
                    {
                        byte[] dato1 = new UTF8Encoding(true).GetBytes(string.Format("\n------------------{0:HH:mm:ss}------------------ \n", hoy));
                        stream.Write(dato1, 0, dato1.Length);

                        byte[] dato2 = new UTF8Encoding(true).GetBytes(objeto);
                        stream.Write(dato2, 0, dato2.Length);

                        byte[] dato3 = new UTF8Encoding(true).GetBytes("\n=====================================================================================\n");
                        stream.Write(dato3, 0, dato3.Length);
                    }
                }
                catch { }
                var response = Core.CoreResponse.SetError(exception);
                context.Response.StatusCode = 200;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }));

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseSwagger(c =>
            {                
                c.SerializeAsV2 = true;
            });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API - Metrogas - v1");
            });

            app.UseRouting();
            app.UseCors("MyPolicy");
            app.UseAuthorization();
            // custom jwt auth middleware
            app.UseMiddleware<Core.JwtMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });

        }
    }
}
