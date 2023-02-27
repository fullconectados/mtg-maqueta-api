// ***********************************************************************
// Assembly         : metrogas.api
// Author           : Full Conectados Ltda <software@fullconectados.cl>
// Created          : 09-18-2021
//
// Last Modified By : ginom
// Last Modified On : 09-19-2021
// ***********************************************************************
// <copyright file="Comunas.cs" company="Full Conectados Ltda">
//     Copyright (c) 2021. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using metrogas.api.Core;
using metrogas.api.Models.PRM;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace metrogas.api.Business.PRM
{

    /// <summary>
    /// Clase control de datos para Comuna
    /// </summary>
    public class Comunas : CoreConnection
	{
        /// <summary>
        /// Se crea una instancia de la clase
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public Comunas(IConfiguration configuration) : base(configuration) { }
        /// <summary>
        /// Gets the todos.
        /// </summary>
        /// <param name="opc">The opc.</param>
        /// <param name="filter">The filter.</param>
        /// <returns>List&lt;Comuna&gt;.</returns>
        public List<Comuna> GetTodos(int opc, Comuna filter)
		{
			var ds = Execute(1, opc, filter);
			var lst = SetModel(ds);
			return lst;
		}
        /// <summary>
        /// Gets the registro.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Comuna.</returns>
        public Comuna GetRegistro(int id)
		{
			var ds = Execute(1, 2, new Comuna() { Id = id });
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
        public bool SaveRegistro(int opc, Comuna data)
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
        /// Actives the registro.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool ActiveRegistro(int id)
		{
			Execute(3, 2, new Comuna() { Id = id });
			return true;
		}
        /// <summary>
        /// Executes the specified accion.
        /// </summary>
        /// <param name="accion">The accion.</param>
        /// <param name="opcion">The opcion.</param>
        /// <param name="data">The data.</param>
        /// <returns>DataSet.</returns>
        private DataSet Execute(int accion, int opcion = 1, Comuna data = null)
		{
			OpenCon();
			var p = new List<DataParam>(){
			new DataParam("ACCION", DataParam.ParamType.INT, accion),
			new DataParam("OPCION", DataParam.ParamType.INT, opcion),
		};
			if (data != null)
			{
				p.Add(new DataParam("id", DataParam.ParamType.INT, data.Id));
				p.Add(new DataParam("region_id", DataParam.ParamType.TINYINT, data.Region?.Id));
				p.Add(new DataParam("nombre", DataParam.ParamType.VARCHAR, data.Nombre));
				p.Add(new DataParam("activo", DataParam.ParamType.BIT, data.Activo));

			}
			var ds = ExecuteProcedure("dbo.sp_prm_comuna", p);
			CloseCon();
			return ds;
		}
        /// <summary>
        /// Sets the model.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns>List&lt;Comuna&gt;.</returns>
        private List<Comuna> SetModel(DataSet ds)
		{
			var lst = new List<Comuna>();
			lst.AddRange(from DataRow dr in ds.Tables[0].Rows
						 select new Comuna
						 {
							 Id = GetField(dr, "id").ValidateInt32() ?? 0,
							 Region = SetBaseModel(dr, "region"),
							 Nombre = GetField(dr, "nombre").ValidateString() ?? "",
							 Activo = GetField(dr, "activo").ValidateBoolean() ?? false,

						 });
			return lst;
		}

	}
}

