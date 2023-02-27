// ***********************************************************************
// Assembly         : metrogas.api
// Author           : Full Conectados Ltda <software@fullconectados.cl>
// Created          : 09-17-2021
//
// Last Modified By : ginom
// Last Modified On : 09-19-2021
// ***********************************************************************
// <copyright file="CoreRequest.cs" company="Full Conectados Ltda">
//     Copyright (c) 2021. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.ComponentModel;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace metrogas.api.Core
{
    /// <summary>
    /// Class CoreRequest.
    /// </summary>
    public class CoreRequest
    {
        /// <summary>
        /// Gets or sets the mode.
        /// </summary>
        /// <value>The mode.</value>
        [DefaultValue(1)] 
        public int Mode { get; set; } = 1;
        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>The data.</value>
        public string Data { get; set; }

        /// <summary>
        /// Desencriptars this instance.
        /// </summary>
        /// <returns>System.String.</returns>
        public string Desencriptar()
        {
            var numeros = "7865904312";
            var minuscula = "qwertyuiopasdfghjklzxcvbnm";
            var mayuscula = "ZXCVBNMASDFGHJKLQWERTYUIOP";
            var value = Data.Replace("=", "");
            var resultado = "";
            foreach (char x in value)
            {
                var ch = (int)x;
                if (ch >= 48 && ch <= 57)
                    resultado += (char)(numeros.IndexOf(x) + 48);
                else if (ch >= 65 && ch <= 90)
                    resultado += (char)(mayuscula.IndexOf(x) + 65);
                else if (ch >= 97 && ch <= 122)
                    resultado += (char)(minuscula.IndexOf(x) + 97);
                else if (ch == 64)
                    resultado += '=';
                else
                    resultado += x;
            }
            var base64Encoded = Encoding.UTF8.GetString(Convert.FromBase64String(resultado));
            //base64Encoded = base64Encoded.Replace("\"", "");
            base64Encoded = base64Encoded.Replace("\\", "\"");
            return base64Encoded;
        }

        /// <summary>
        /// Gets the object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>T.</returns>
        public T GetObject<T>()
        {
            T result;
            var base64Encoded = Desencriptar();
            var serializeOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                IgnoreNullValues =true,
                WriteIndented = true,
                Encoder = JavaScriptEncoder.Default,//JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            };
            result = JsonSerializer.Deserialize<T>(base64Encoded, serializeOptions);
            return result;
        }
    }
}
