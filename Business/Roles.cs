// ***********************************************************************
// Assembly         : metrogas.api
// Author           : ginom
// Created          : 09-24-2021
//
// Last Modified By : ginom
// Last Modified On : 10-19-2021
// ***********************************************************************
// <copyright file="Roles.cs" company="Full Conectados Ltda">
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
    /// Clase control de datos para Rol
    /// </summary>
    public class Roles : CoreConnection
	{
        /// <summary>
        /// Se crea una instancia de la clase
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public Roles(IConfiguration configuration) : base(configuration) { }
        /// <summary>
        /// Gets the todos.
        /// </summary>
        /// <param name="opc">The opc.</param>
        /// <param name="filter">The filter.</param>
        /// <returns>List&lt;Rol&gt;.</returns>
        public List<Rol> GetTodos(int opc, Rol filter)
		{
			var ds = Execute(1, opc, filter);
			var lst = SetModel(ds);
			return lst;
		}
        /// <summary>
        /// Gets the registro.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Rol.</returns>
        public Rol GetRegistro(int id)
		{
			var ds = Execute(1, 2, new Rol() { Id = id });
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
        public bool SaveRegistro(int opc, Rol data)
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
			Execute(3, 2, new Rol() { Id = id });
			return true;
		}
        /// <summary>
        /// Executes the specified accion.
        /// </summary>
        /// <param name="accion">The accion.</param>
        /// <param name="opcion">The opcion.</param>
        /// <param name="data">The data.</param>
        /// <returns>DataSet.</returns>
        private DataSet Execute(int accion, int opcion = 1, Rol data = null)
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
				p.Add(new DataParam("activo", DataParam.ParamType.BIT, data.Activo));

			}
			var ds = ExecuteProcedure("dbo.sp_rol", p);
			CloseCon();
			return ds;
		}
        /// <summary>
        /// Executes the specified accion.
        /// </summary>
        /// <param name="accion">The accion.</param>
        /// <param name="opcion">The opcion.</param>
        /// <param name="rol">The rol.</param>
        /// <param name="modulo">The modulo.</param>
        /// <returns>DataSet.</returns>
        public DataSet ExecuteRolModule(int accion, int opcion = 1, int rol = 0, int modulo = 0)
		{
			OpenCon();
			var p = new List<DataParam>(){
				new DataParam("ACCION", DataParam.ParamType.INT, accion),
				new DataParam("OPCION", DataParam.ParamType.INT, opcion),
			};
			p.Add(new DataParam("rol_id", DataParam.ParamType.INT, rol));
			p.Add(new DataParam("modulo_id", DataParam.ParamType.INT, modulo));
			var ds = ExecuteProcedure("dbo.sp_rol_modulo", p);
			CloseCon();
			return ds;
		}
        /// <summary>
        /// Sets the model.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns>List&lt;Rol&gt;.</returns>
        private List<Rol> SetModel(DataSet ds)
		{
			var lst = new List<Rol>();
			lst.AddRange(from DataRow dr in ds.Tables[0].Rows
						 select new Rol
						 {
							 Id = GetField(dr, "id").ValidateInt32() ?? 0,
							 Nombre = GetField(dr, "nombre").ValidateString() ?? "",
							 Activo = GetField(dr, "activo").ValidateBoolean() ?? false,

						 });
			if (ds.Tables.Count > 1)
            {
				foreach (var rol in lst)
				{
					rol.Modulos = new List<Modulo>();
					rol.Modulos.AddRange(from DataRow dr in ds.Tables[1].Rows
										where dr["rol_id"].ValidateInt32() == rol.Id
											select new Modulo
											{
												Id = GetField(dr, "id").ValidateInt32() ?? 0,
												Nombre = GetField(dr, "nombre").ValidateString() ?? "",
												Titulo = GetField(dr, "titulo").ValidateString() ?? "",
												Icono = GetField(dr, "icono").ValidateString() ?? "",
												Url = GetField(dr, "url").ValidateString() ?? "",
												Padre = SetBaseModel(dr, "padre"),
												Activo = GetField(dr, "activo").ValidateBoolean() ?? false,
											}											
										);
				}
			}
			return lst;
		}

	}
}
