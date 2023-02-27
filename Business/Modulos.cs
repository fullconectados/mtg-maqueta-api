// ***********************************************************************
// Assembly         : metrogas.api
// Author           : Full Conectados Ltda <software@fullconectados.cl>
// Created          : 09-24-2021
//
// Last Modified By : ginom
// Last Modified On : 09-24-2021
// ***********************************************************************
// <copyright file="Modulos.cs" company="Full Conectados Ltda">
//     @metrogas 2021
// </copyright>
// <summary></summary>
// ***********************************************************************
using metrogas.api.Core;
using metrogas.api.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace metrogas.api.Business
{

    /// <summary>
    /// Clase control de datos para Modulo
    /// </summary>
    public class Modulos : CoreConnection
	{
        /// <summary>
        /// Se crea una instancia de la clase
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public Modulos(IConfiguration configuration) : base(configuration) { }
        /// <summary>
        /// Gets the todos.
        /// </summary>
        /// <param name="opc">The opc.</param>
        /// <param name="filter">The filter.</param>
        /// <returns>List&lt;Modulo&gt;.</returns>
        public List<Modulo> GetTodos(int opc, Modulo filter)
		{
			var ds = Execute(1, opc, filter);
			var lst = SetModel(ds);
			return lst;
		}
        /// <summary>
        /// Gets the registro.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Modulo.</returns>
        public Modulo GetRegistro(int id)
		{
			var ds = Execute(1, 2, new Modulo() { Id = id });
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
        public bool SaveRegistro(int opc, Modulo data)
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
			Execute(3, 2, new Modulo() { Id = id });
			return true;
		}
        /// <summary>
        /// Executes the specified accion.
        /// </summary>
        /// <param name="accion">The accion.</param>
        /// <param name="opcion">The opcion.</param>
        /// <param name="data">The data.</param>
        /// <returns>DataSet.</returns>
        private DataSet Execute(int accion, int opcion = 1, Modulo data = null)
		{
			OpenCon();
			var p = new List<DataParam>(){
			new DataParam("ACCION", DataParam.ParamType.INT, accion),
			new DataParam("OPCION", DataParam.ParamType.INT, opcion),
		};
			if (data != null)
			{
				p.Add(new DataParam("id", DataParam.ParamType.INT, data.Id));
				p.Add(new DataParam("nombre", DataParam.ParamType.VARCHAR, data.Nombre));
				p.Add(new DataParam("titulo", DataParam.ParamType.VARCHAR, data.Titulo));
				p.Add(new DataParam("icono", DataParam.ParamType.VARCHAR, data.Icono));
				p.Add(new DataParam("url", DataParam.ParamType.VARCHAR, data.Url));
				p.Add(new DataParam("padre", DataParam.ParamType.INT, data.Padre?.Id));
				p.Add(new DataParam("activo", DataParam.ParamType.BIT, data.Activo));

			}
			var ds = ExecuteProcedure("dbo.sp_modulo", p);
			CloseCon();
			return ds;
		}
        /// <summary>
        /// Sets the model.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns>List&lt;Modulo&gt;.</returns>
        private List<Modulo> SetModel(DataSet ds)
		{
			var lst = new List<Modulo>();
			lst.AddRange(from DataRow dr in ds.Tables[0].Rows
						 select new Modulo
						 {
							 Id = GetField(dr, "id").ValidateInt32() ?? 0,
							 Nombre = GetField(dr, "nombre").ValidateString() ?? "",
							 Titulo = GetField(dr, "titulo").ValidateString() ?? "",
							 Icono = GetField(dr, "icono").ValidateString() ?? "",
							 Url = GetField(dr, "url").ValidateString() ?? "",
							 Padre = SetBaseModel(dr, "padre"),
							 Activo = GetField(dr, "activo").ValidateBoolean() ?? false,

						 });

			if (ds.Tables.Count == 2)
			{
				var temp = new List<Modulo>();
				temp.AddRange(from DataRow dr in ds.Tables[1].Rows
							  select new Modulo
							  {
								  Id = GetField(dr, "rol_id").ValidateInt32() ?? 0, 
								  Padre = new _BaseModel() { Id = GetField(dr, "modulo_id").ValidateInt32() ?? 0 }, 

							  });
				foreach (var item in lst)
                {
					item.Roles = (from Modulo dr in temp
										where dr.Padre.Id == item.Id
										select dr.Id).ToList();
				}
            }
			return lst;
		}

	}
}
