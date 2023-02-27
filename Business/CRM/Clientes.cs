// ***********************************************************************
// Assembly         : metrogas.api
// Author           : Full Conectados Ltda <software@fullconectados.cl>
// Created          : 09-18-2021
//
// Last Modified By : ginom
// Last Modified On : 09-19-2021
// ***********************************************************************
// <copyright file="Clientes.cs" company="Full Conectados Ltda">
//     Copyright (c) 2021. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using metrogas.api.Core;
using metrogas.api.Models.CRM;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace metrogas.api.Business.CRM
{

    /// <summary>
    /// Clase control de datos para Cliente
    /// </summary>
    public class Clientes : CoreConnection
	{
        /// <summary>
        /// Se crea una instancia de la clase
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public Clientes(IConfiguration configuration) : base(configuration) { }
        /// <summary>
        /// Gets the todos.
        /// </summary>
        /// <param name="opc">The opc.</param>
        /// <param name="filter">The filter.</param>
        /// <returns>List&lt;Cliente&gt;.</returns>
        public List<Cliente> GetTodos(int opc, Cliente filter)
		{
			var ds = Execute(1, opc, filter);
			var lst = SetModel(ds);
			return lst;
		}
        /// <summary>
        /// Gets the registro.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Cliente.</returns>
        public Cliente GetRegistro(int id)
		{
			var ds = Execute(1, 2, new Cliente() { Id = id });
			var lst = SetModel(ds);
			if (lst.Count > 0)
				return lst[0];
			return null;
		}
        /// <summary>
        /// Saves the registro.
        /// </summary>
        /// <param name="opc">The opc.</param>
        /// <param name="data">The data.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool SaveRegistro(int opc, Cliente data)
		{
			if (data.Id == 0)
			{
				Execute(2, opc, data);
			}
			else
			{
				Execute(3, opc, data);
			}
			return true;
		}
        /// <summary>
        /// Executes the specified accion.
        /// </summary>
        /// <param name="accion">The accion.</param>
        /// <param name="opcion">The opcion.</param>
        /// <param name="data">The data.</param>
        /// <returns>DataSet.</returns>
        private DataSet Execute(int accion, int opcion = 1, Cliente data = null)
		{
			OpenCon();
			var p = new List<DataParam>(){
			new DataParam("ACCION", DataParam.ParamType.INT, accion),
			new DataParam("OPCION", DataParam.ParamType.INT, opcion),
		};
			if (data != null)
			{
				p.Add(new DataParam("id", DataParam.ParamType.INT, data.Id));
				p.Add(new DataParam("ic", DataParam.ParamType.INT, data.Ic));
				p.Add(new DataParam("rut", DataParam.ParamType.VARCHAR, data.Rut));
				p.Add(new DataParam("nombre", DataParam.ParamType.VARCHAR, data.Nombre));
				p.Add(new DataParam("paterno", DataParam.ParamType.VARCHAR, data.Paterno));
				p.Add(new DataParam("materno", DataParam.ParamType.VARCHAR, data.Materno));
				p.Add(new DataParam("telefono", DataParam.ParamType.VARCHAR, data.Telefono));
				p.Add(new DataParam("email", DataParam.ParamType.VARCHAR, data.Email));
				p.Add(new DataParam("calle_id", DataParam.ParamType.INT, data.Calle?.Id));
				p.Add(new DataParam("numero", DataParam.ParamType.VARCHAR, data.Numero));
				p.Add(new DataParam("complemento", DataParam.ParamType.VARCHAR, data.Complemento));
				p.Add(new DataParam("block", DataParam.ParamType.VARCHAR, data.Block));
				p.Add(new DataParam("adicional", DataParam.ParamType.VARCHAR, data.Adicional));
				p.Add(new DataParam("lista", DataParam.ParamType.CHAR, data.Lista));

			}
			var ds = ExecuteProcedure("dbo.sp_crm_cliente", p);
			CloseCon();
			return ds;
		}
        /// <summary>
        /// Sets the model.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns>List&lt;Cliente&gt;.</returns>
        private List<Cliente> SetModel(DataSet ds)
		{
			var lst = new List<Cliente>();
			lst.AddRange(from DataRow dr in ds.Tables[0].Rows
						 select new Cliente
						 {
							 Id = GetField(dr, "id").ValidateInt32() ?? 0,
							 Ic = GetField(dr, "ic").ValidateInt32() ?? 0,
							 Rut = GetField(dr, "rut").ValidateString() ?? "",
							 Nombre = GetField(dr, "nombre").ValidateString() ?? "",
							 Paterno = GetField(dr, "paterno").ValidateString() ?? "",
							 Materno = GetField(dr, "materno").ValidateString() ?? "",
							 Telefono = GetField(dr, "telefono").ValidateString() ?? "",
							 Email = GetField(dr, "email").ValidateString() ?? "",
							 Calle = new Models.PRM.Calle()
							 {
								 Id = GetField(dr, "calle_id").ValidateInt32() ?? 0,
								 Nombre = GetField(dr, "calle_nombre").ValidateString() ?? "",
								 Comuna = SetBaseModel(dr, "comuna"),
								 Region = SetBaseModel(dr, "region")
							 },
							 Numero = GetField(dr, "numero").ValidateString() ?? "",
							 Complemento = GetField(dr, "complemento").ValidateString() ?? "",
							 Block = GetField(dr, "block").ValidateString() ?? "",
							 Adicional = GetField(dr, "adicional").ValidateString() ?? "",
							 Casa = GetField(dr, "cliente_casa").ValidateString() ?? "",
							 Lista = GetField(dr, "lista").ValidateString(),
							 Campanha = GetField(dr, "campanha").ValidateInt32() ?? 0,

						 });
			return lst;
		}

	}
}
