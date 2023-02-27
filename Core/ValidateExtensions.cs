// ***********************************************************************
// Assembly         : metrogas.api
// Author           : Full Conectados Ltda <software@fullconectados.cl>
// Created          : 09-17-2021
//
// Last Modified By : ginom
// Last Modified On : 09-18-2021
// ***********************************************************************
// <copyright file="ValidateExtensions.cs" company="Full Conectados Ltda">
//     Copyright (c) 2021. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace metrogas.api.Core
{
    /// <summary>
    /// Class ValidateExtensions.
    /// </summary>
    public static class ValidateExtensions
	{
        /// <summary>
        /// Validas the basico.
        /// </summary>
        /// <param name="valor">The valor.</param>
        /// <param name="nulos">if set to <c>true</c> [nulos].</param>
        /// <param name="campo">The campo.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        /// <exception cref="Exception"></exception>
        /// <exception cref="Exception"></exception>
        /// <exception cref="Exception"></exception>
        private static bool ValidaBasico(object valor, bool nulos, string campo)
		{
			if (valor == DBNull.Value)
			{
				if (nulos)
				{
					return false;
				}
				throw new Exception(campo + " no se permite vacio");
			}
			if (!string.IsNullOrEmpty(valor.ToString()))
			{
				return true;
			}
			if (nulos)
			{
				return true;
			}
			throw new Exception(campo + " no se permite vacio");
		}

        /// <summary>
        /// Validates the int32.
        /// </summary>
        /// <param name="valor">The valor.</param>
        /// <param name="nulos">if set to <c>true</c> [nulos].</param>
        /// <param name="campo">The campo.</param>
        /// <param name="minimo">The minimo.</param>
        /// <param name="maximo">The maximo.</param>
        /// <returns>System.Nullable&lt;System.Int32&gt;.</returns>
        /// <exception cref="Exception"></exception>
        /// <exception cref="Exception"></exception>
        /// <exception cref="Exception"></exception>
        /// <exception cref="Exception"></exception>
        /// <exception cref="Exception"></exception>
        public static int? ValidateInt32(this object valor, bool nulos = true, string campo = "-", int? minimo = null, int? maximo = null)
		{
			if (!ValidaBasico(valor, nulos, campo))
			{
				return null;
			}
			int num;
			try
			{
				num = Convert.ToInt32(valor.ToString().Trim());
			}
			catch
			{
				throw new Exception(campo + " no posee un valor entero valido");
			}
			if (minimo.HasValue && num < minimo)
			{
				throw new Exception(campo + " tiene que tener un valor minimo de: " + minimo);
			}
			if (maximo.HasValue && num > maximo)
			{
				throw new Exception(campo + " tiene que tener un valor maximo de: " + minimo);
			}
			return num;
		}

        /// <summary>
        /// Validates the string.
        /// </summary>
        /// <param name="valor">The valor.</param>
        /// <param name="nulos">if set to <c>true</c> [nulos].</param>
        /// <param name="campo">The campo.</param>
        /// <param name="maximo">The maximo.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="Exception"></exception>
        /// <exception cref="Exception"></exception>
        /// <exception cref="Exception"></exception>
        public static string ValidateString(this object valor, bool nulos = true, string campo = "-", int maximo = 0)
		{
			if (!ValidaBasico(valor, nulos, campo))
			{
				return null;
			}
			try
			{
				string text = valor.ToString().Trim();
				if (maximo <= 0)
				{
					return text;
				}
				if (text.Length > maximo)
				{
					throw new Exception($"{campo} tiene un largo ({text.Length}) mayor al permitido ({maximo})");
				}
				return text;
			}
			catch
			{
				throw new Exception(campo + " no posee un valor valido");
			}
		}

        /// <summary>
        /// Validates the decimal.
        /// </summary>
        /// <param name="valor">The valor.</param>
        /// <param name="nulos">if set to <c>true</c> [nulos].</param>
        /// <param name="campo">The campo.</param>
        /// <param name="minimo">The minimo.</param>
        /// <param name="maximo">The maximo.</param>
        /// <returns>System.Nullable&lt;System.Decimal&gt;.</returns>
        /// <exception cref="Exception"></exception>
        /// <exception cref="Exception"></exception>
        /// <exception cref="Exception"></exception>
        /// <exception cref="Exception"></exception>
        /// <exception cref="Exception"></exception>
        public static decimal? ValidateDecimal(this object valor, bool nulos = true, string campo = "-", decimal? minimo = null, decimal? maximo = null)
		{
			if (!ValidaBasico(valor, nulos, campo))
			{
				return null;
			}
			decimal num;
			try
			{
				num = Convert.ToDecimal(valor.ToString().Trim());
			}
			catch
			{
				throw new Exception(campo + " no posee un valor decimal valido");
			}
			int num4;
			if (minimo.HasValue)
			{
				decimal num2 = num;
				decimal? num3 = minimo;
				num4 = ((!(num2 < num3.GetValueOrDefault()) || !num3.HasValue) ? 1 : 0);
			}
			else
			{
				num4 = 1;
			}
			if (num4 == 0)
			{
				throw new Exception(campo + " tiene que tener un valor minimo de: " + minimo);
			}
			int num5;
			if (maximo.HasValue)
			{
				decimal num2 = num;
				decimal? num3 = maximo;
				num5 = ((!(num2 > num3.GetValueOrDefault()) || !num3.HasValue) ? 1 : 0);
			}
			else
			{
				num5 = 1;
			}
			if (num5 == 0)
			{
				throw new Exception(campo + " tiene que tener un valor maximo de: " + minimo);
			}
			return num;
		}

        /// <summary>
        /// Validates the date time.
        /// </summary>
        /// <param name="valor">The valor.</param>
        /// <param name="nulos">if set to <c>true</c> [nulos].</param>
        /// <param name="campo">The campo.</param>
        /// <returns>System.Nullable&lt;DateTime&gt;.</returns>
        /// <exception cref="Exception"></exception>
        public static DateTime? ValidateDateTime(this object valor, bool nulos = true, string campo = "-")
		{
			if (!ValidaBasico(valor, nulos, campo))
			{
				return null;
			}
			try
			{
				return (DateTime)valor;
				/*
				string text = valor.ToString().Trim().Replace("/", "-");
				text = text.Replace(".", "-");
				return DateTime.ParseExact(text, "MM-dd-yyyy H:mm:ss", new DateTimeFormatInfo());
				*/
			}
			catch
			{
				throw new Exception(valor.ToString() + ": no posee un valor de fecha valido");
			}
		}

        /// <summary>
        /// Validates the boolean.
        /// </summary>
        /// <param name="valor">The valor.</param>
        /// <param name="nulos">if set to <c>true</c> [nulos].</param>
        /// <param name="campo">The campo.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        /// <exception cref="Exception"></exception>
        public static bool? ValidateBoolean(this object valor, bool nulos = true, string campo = "-")
		{
			if (!ValidaBasico(valor, nulos, campo))
			{
				return null;
			}
			if (bool.TryParse(valor.ToString(), out var result))
			{
				return result;
			}
			if (nulos)
			{
				return null;
			}
			throw new Exception(campo + " no se permite null");
		}

        /// <summary>
        /// Exceptions the message.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns>System.String.</returns>
        public static string ExceptionMessage(Exception ex)
		{
			while (ex.InnerException != null)
			{
				ex = ex.InnerException;
			}
			return ex.Message;
		}
	}
}
