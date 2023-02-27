// ***********************************************************************
// Assembly         : metrogas.api
// Author           : Full Conectados Ltda <software@fullconectados.cl>
// Created          : 09-17-2021
//
// Last Modified By : ginom
// Last Modified On : 10-04-2021
// ***********************************************************************
// <copyright file="CoreController.cs" company="Full Conectados Ltda">
//     Copyright (c) 2021. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Collections.Generic;

namespace metrogas.api.Core
{
    /// <summary>
    /// Class CoreController.
    /// Implements the <see cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    public class CoreController : ControllerBase
    {
        /// <summary>
        /// Gets or sets the configuration.
        /// </summary>
        /// <value>The configuration.</value>
        private protected IConfiguration Config { get; set; }

        /// <summary>
        /// Processes the filter.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">The data.</param>
        /// <returns>CoreFilter&lt;T&gt;.</returns>
        protected CoreFilter<T> ProcessFilter<T>(string data) where T:class
        {
            var opt = new CoreFilter<T>();
            if (data != null)
            {
                var tmp = new CoreRequest() { Data = data };
                opt = tmp.GetObject<CoreFilter<T>>();
            }
            return opt;
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>System.String.</returns>
        protected string GetConfig(string key)
        {
            var tmp = Environment.GetEnvironmentVariable("METROBOLSAS_" + key.Replace("-", "_"));
            if (string.IsNullOrWhiteSpace(tmp))
            {
                tmp = Config[key];
            }
            return tmp;            
        }

        /// <summary>
        /// Sends the mail.
        /// </summary>
        /// <param name="categoria">The categoria.</param>
        /// <param name="nombre">The nombre.</param>
        /// <param name="email">The email.</param>
        /// <param name="asunto">The asunto.</param>
        /// <param name="mensaje">The mensaje.</param>
        /// <param name="adjuntos"></param>
        protected void SendMail(string categoria, string nombre, string email, string asunto, string mensaje, List<KeyValuePair<string, byte[]>> adjuntos = null)
        {
            MimeMessage message = new MimeMessage();
            MailboxAddress from = new MailboxAddress("Metrogas", GetConfig("EMAIL_FROM"));
            message.From.Add(from);

            MailboxAddress to = new MailboxAddress(nombre, email);
            message.To.Add(to);

            message.Subject = asunto;

            BodyBuilder bodyBuilder = new BodyBuilder
            {
                HtmlBody = mensaje
            };
            if (adjuntos != null)
            {
                foreach (var item in adjuntos)
                {
                    bodyBuilder.Attachments.Add(item.Key, item.Value, new ContentType("application", "pdf"));
                }
            }
            message.Body = bodyBuilder.ToMessageBody();
            message.Body.Headers.Add("X-FD-Code", GetConfig("EMAIL_TAGCODE"));
            message.Body.Headers.Add("X-FD-Groupcode", categoria);

            SmtpClient client = new SmtpClient();
            var username = GetConfig("EMAIL_USER");
            var password = GetConfig("EMAIL_PASS");
            var smtp = GetConfig("EMAIL_SMTP");
            var port = GetConfig("EMAIL_PORT").ValidateInt32() ?? 0;

            client.Connect(smtp, port, SecureSocketOptions.StartTls);
            client.Authenticate(username, password);
            client.Send(message);
            client.Disconnect(true);
            client.Dispose();
        }

    }
}
