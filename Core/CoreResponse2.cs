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
using System.Collections.Generic;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace metrogas.api.Core
{
    /// <summary>
    /// Class CoreResponse.
    /// </summary>
    public class CoreResponse2<T>
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
        public List<T> Data { get; set; }
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
        public static CoreResponse2<T> SetSuccess<T>(List<T> data)
        {
            return SetSuccess<T>(data, "Exito");
        }
        /// <summary>
        /// Sets the success.
        /// </summary>
        /// <param name="mensaje">The mensaje.</param>
        /// <returns>CoreResponse.</returns>
        public static CoreResponse2<string> SetSuccess(string mensaje)
        {
            return SetSuccess<string>(null, mensaje);
        }
        /// <summary>
        /// Sets the success.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="mensaje">The mensaje.</param>
        /// <returns>CoreResponse.</returns>
        public static CoreResponse2<T> SetSuccess<T>(List<T> data, string mensaje)
        {
            
            return new CoreResponse2<T>() { StatusCode = 200, Data = data, Message = mensaje };
        }
        /// <summary>
        /// Sets the not found.
        /// </summary>
        /// <returns>CoreResponse.</returns>
        public static CoreResponse2<T> SetNotFound()
        {
            return new CoreResponse2<T>() { StatusCode = 404, Message = "No encontrado" };
        }
        /// <summary>
        /// Sets the error.
        /// </summary>
        /// <param name="mensaje">The mensaje.</param>
        /// <returns>CoreResponse.</returns>
        public static CoreResponse2<T> SetError(string mensaje)
        {
            return new CoreResponse2<T>() { StatusCode = 500, Message = "Operación Anulada", Error = mensaje };
        }
        /// <summary>
        /// Sets the error.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns>CoreResponse.</returns>
        public static CoreResponse2<T> SetError(Exception ex)
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
            return new CoreResponse2<T>() { StatusCode = 500, Message = "Excepción provocada", Error = _msg };
        }
        /// <summary>
        /// Sets the error.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns>CoreResponse.</returns>
        public static CoreResponse2<T> SetError(ModelStateDictionary ex)
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
            return new CoreResponse2<T>() { StatusCode = 500, Message = "Bad Request", Error = _msg };
        }

        public static CoreResponse2<T> NoData()
        {
            return SetError("se requiere condición de datos");
        }
    }
}
