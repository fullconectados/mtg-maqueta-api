// ***********************************************************************
// Assembly         : metrogas.api
// Author           : Full Conectados Ltda <software@fullconectados.cl>
// Created          : 09-17-2021
//
// Last Modified By : ginom
// Last Modified On : 09-18-2021
// ***********************************************************************
// <copyright file="CoreResponse.cs" company="Full Conectados Ltda">
//     Copyright (c) 2021. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace metrogas.api.Core
{
    /// <summary>
    /// Class CoreResponse.
    /// </summary>
    public class CoreResponse
    {
        /// <summary>
        /// Gets or sets the status code.
        /// </summary>
        /// <value>The status code.</value>
        public int StatusCode { get; set; }
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        public string Message { get; set; }
        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>The data.</value>
        public string Data { get; set; }
        /// <summary>
        /// Gets or sets the error.
        /// </summary>
        /// <value>The error.</value>
        public string Error { get; set; }

        /// <summary>
        /// Sets the success.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>CoreResponse.</returns>
        public static CoreResponse SetSuccess(object data)
        {
            return SetSuccess(data, "Exito");
        }
        /// <summary>
        /// Sets the success.
        /// </summary>
        /// <param name="mensaje">The mensaje.</param>
        /// <returns>CoreResponse.</returns>
        public static CoreResponse SetSuccess(string mensaje)
        {
            return SetSuccess(null, mensaje);
        }
        /// <summary>
        /// Sets the success.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="mensaje">The mensaje.</param>
        /// <returns>CoreResponse.</returns>
        public static CoreResponse SetSuccess(object data, string mensaje)
        {
            var _data = "";
            if (data != null)
            {
                var serializeOptions = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase, 
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, 
                    IgnoreNullValues = true
                    //WriteIndented = true
                };
                var nueva = JsonSerializer.Serialize(data, serializeOptions);

                var numeros = "7865904312";
                var minuscula = "qwertyuiopasdfghjklzxcvbnm";
                var mayuscula = "ZXCVBNMASDFGHJKLQWERTYUIOP";
                byte[] p_byte = Encoding.UTF8.GetBytes(nueva);
                var p_base = Convert.ToBase64String(p_byte);
                foreach (char c in p_base)
                {
                    var ch = (int)c;
                    if (ch >= 48 && ch <= 57)
                        _data += numeros[ch - 48];
                    else if (ch >= 65 && ch <= 90)
                        _data += mayuscula[ch - 65];
                    else if (ch >= 97 && ch <= 122)
                        _data += minuscula[ch - 97];
                    else if (ch == 61)
                        _data += '@';
                    else
                        _data += c;
                }
                _data += "=";

            }
            return new CoreResponse() { StatusCode = 200, Data = _data, Message = mensaje };
        }
        /// <summary>
        /// Sets the not found.
        /// </summary>
        /// <returns>CoreResponse.</returns>
        public static CoreResponse SetNotFound()
        {
            return new CoreResponse() { StatusCode = 404, Message = "No encontrado" };
        }
        /// <summary>
        /// Sets the error.
        /// </summary>
        /// <param name="mensaje">The mensaje.</param>
        /// <returns>CoreResponse.</returns>
        public static CoreResponse SetError(string mensaje)
        {
            return new CoreResponse() { StatusCode = 500, Message = "Operación Anulada", Error = mensaje };
        }
        /// <summary>
        /// Sets the error.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns>CoreResponse.</returns>
        public static CoreResponse SetError(Exception ex)
        {
            var _msg = ex.Message;
#if DEBUG
            _msg += "\n" + "-------------------------------------------------------------";
            if (ex.StackTrace != null)
            {
                _msg += ex.StackTrace;
                var x = ex.InnerException;
                while (x != null)
                {
                    _msg += "\n" + "-------------------------------------------------------------";
                    _msg += "\n" + x.StackTrace;
                    x = x.InnerException;
                }
            }
            else
            {
                _msg += "Not Exception Trace";
            }
#endif
            return new CoreResponse() { StatusCode = 500, Message = "Excepción provocada", Error = _msg };
        }
        /// <summary>
        /// Sets the error.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns>CoreResponse.</returns>
        public static CoreResponse SetError(ModelStateDictionary ex)
        {
            var _msg = "";
#if DEBUG
            foreach (var keyModelStatePair in ex)
            {
                var key = keyModelStatePair.Key;
                var errors = keyModelStatePair.Value.Errors;
                if (errors != null && errors.Count > 0)
                {
                    var errorMessages = new string[errors.Count];
                    for (var i = 0; i < errors.Count; i++)
                    {
                        _msg += errors[i].ErrorMessage ?? "";
                        _msg += "\n" + "-------------------------------------------------------------" + "\n";
                    }
                }
            }
#endif
            return new CoreResponse() { StatusCode = 500, Message = "Bad Request", Error = _msg };
        }

        public static CoreResponse SetWarning(string mensaje)
        {
            return new CoreResponse() { StatusCode = 406, Message = "Operación Anulada", Error = mensaje };
        }

        public static CoreResponse NoData()
        {
            return new CoreResponse() { StatusCode = 405, Message = "Operación Rechazada", Error = "se requiere condición de datos" }; 
        }
    }
}
