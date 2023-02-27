// ***********************************************************************
// Assembly         : metrogas.api
// Author           : Full Conectados Ltda <software@fullconectados.cl>
// Created          : 09-17-2021
//
// Last Modified By : ginom
// Last Modified On : 09-19-2021
// ***********************************************************************
// <copyright file="Usuarios.cs" company="Full Conectados Ltda">
//     Copyright (c) 2021. All rights reserved.
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
    /// Clase control de datos para Usuario
    /// </summary>
    public class Usuarios : CoreConnection
	{
        /// <summary>
        /// Se crea una instancia de la clase
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public Usuarios(IConfiguration configuration) : base(configuration) { }
        /// <summary>
        /// Gets the todos.
        /// </summary>
        /// <param name="opc">The opc.</param>
        /// <param name="filter">The filter.</param>
        /// <returns>List&lt;Usuario&gt;.</returns>
        public List<Usuario> GetTodos(int opc, Usuario filter)
		{
			var ds = Execute(1, opc, filter);
			var lst = SetModel(ds);
			return lst;
		}
        /// <summary>
        /// Gets the registro.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Usuario.</returns>
        public Usuario GetRegistro(int id)
		{
			var ds = Execute(1, 2, new Usuario() { Id = id });
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
        public bool SaveRegistro(int opc, Usuario data)
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
			Execute(3, 2, new Usuario() { Id = id });
			return true;
		}
        /// <summary>
        /// Executes the specified accion.
        /// </summary>
        /// <param name="accion">The accion.</param>
        /// <param name="opcion">The opcion.</param>
        /// <param name="data">The data.</param>
        /// <returns>DataSet.</returns>
        private DataSet Execute(int accion, int opcion = 1, Usuario data = null)
		{
			OpenCon();
			var p = new List<DataParam>(){
			new DataParam("ACCION", DataParam.ParamType.INT, accion),
			new DataParam("OPCION", DataParam.ParamType.INT, opcion),
		};
			if (data != null)
			{
				p.Add(new DataParam("id", DataParam.ParamType.INT, data.Id));
				p.Add(new DataParam("username", DataParam.ParamType.VARCHAR, data.Username));
				p.Add(new DataParam("nombre", DataParam.ParamType.VARCHAR, data.Nombre));
				p.Add(new DataParam("apellido", DataParam.ParamType.VARCHAR, data.Apellido));
				p.Add(new DataParam("telefono", DataParam.ParamType.VARCHAR, data.Telefono));
				p.Add(new DataParam("email", DataParam.ParamType.VARCHAR, data.Email));
				p.Add(new DataParam("cargo", DataParam.ParamType.VARCHAR, data.Cargo));
				p.Add(new DataParam("rol_id", DataParam.ParamType.INT, data.Rol?.Id));
				p.Add(new DataParam("password", DataParam.ParamType.VARCHAR, data.Password));
				p.Add(new DataParam("acceso", DataParam.ParamType.DATETIME, data.Acceso));
				p.Add(new DataParam("activo", DataParam.ParamType.BIT, data.Activo));

			}
			var ds = ExecuteProcedure("dbo.sp_usuario", p);
			CloseCon();
			return ds;
		}
        /// <summary>
        /// Sets the model.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns>List&lt;Usuario&gt;.</returns>
        private List<Usuario> SetModel(DataSet ds)
		{
			var lst = new List<Usuario>();
			lst.AddRange(from DataRow dr in ds.Tables[0].Rows
						 select new Usuario
						 {
							 Id = GetField(dr, "id").ValidateInt32() ?? 0,
							 Username = GetField(dr, "username").ValidateString() ?? "",
							 Nombre = GetField(dr, "nombre").ValidateString() ?? "",
							 Apellido = GetField(dr, "apellido").ValidateString() ?? "",
							 Telefono = GetField(dr, "telefono").ValidateString() ?? "",
							 Email = GetField(dr, "email").ValidateString() ?? "",
							 Cargo = GetField(dr, "cargo").ValidateString() ?? "",
							 Rol = SetBaseModel(dr, "rol"),
							 //Password = GetField(dr, "password").ValidateString() ?? "",
							 Acceso = GetField(dr, "acceso").ValidateDateTime() ?? System.DateTime.MinValue,
							 Activo = GetField(dr, "activo").ValidateBoolean() ?? false,
						 });
			return lst;
		}

	}}