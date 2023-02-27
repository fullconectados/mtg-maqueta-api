// ***********************************************************************
// Assembly         : metrogas.api
// Author           : ginom
// Created          : 10-18-2021
//
// Last Modified By : ginom
// Last Modified On : 10-18-2021
// ***********************************************************************
// <copyright file="Plantillas.cs" company="Full Conectados Ltda">
//     @metrogas 2021
// </copyright>
// <summary></summary>
// ***********************************************************************
using metrogas.api.Core;
using metrogas.api.Models.CMS;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace metrogas.api.Business.CMS
{

	/// <summary>
	/// Clase control de datos para Plantilla
	/// </summary>
	public class Plantillas : CoreConnection
	{
		public Plantillas(IConfiguration configuration) : base(configuration) { }
		public List<Plantilla> GetTodos(int opc, Plantilla filter)
		{
			var ds = Execute(1, opc, filter);
			var lst = SetModel(ds);
			return lst;
		}
		public Plantilla GetRegistro(int id)
		{
			var ds = Execute(1, 2, new Plantilla() { Id = id });
			var lst = SetModel(ds);
			if (lst.Count > 0)
				return lst[0];
			return null;
		}
		public bool SaveRegistro(int opc, Plantilla data)
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
		public bool ActiveRegistro(int id)
		{
			Execute(3, 2, new Plantilla() { Id = id });
			return true;
		}
		private DataSet Execute(int accion, int opcion = 1, Plantilla data = null)
		{
			OpenCon();
			var p = new List<DataParam>(){
			new DataParam("ACCION", DataParam.ParamType.INT, accion),
			new DataParam("OPCION", DataParam.ParamType.INT, opcion),
		};
			if (data != null)
			{
				p.Add(new DataParam("id", DataParam.ParamType.TINYINT, data.Id));
				p.Add(new DataParam("tipo", DataParam.ParamType.CHAR, data.Tipo));
				p.Add(new DataParam("nombre", DataParam.ParamType.VARCHAR, data.Nombre));
				p.Add(new DataParam("asunto", DataParam.ParamType.VARCHAR, data.Asunto));
				p.Add(new DataParam("contenido", DataParam.ParamType.VARCHAR, data.Contenido));
				p.Add(new DataParam("categoria", DataParam.ParamType.VARCHAR, data.Categoria));
				p.Add(new DataParam("activo", DataParam.ParamType.BIT, data.Activo));
				p.Add(new DataParam("campos", DataParam.ParamType.VARCHAR, data.Campos));
				p.Add(new DataParam("destino", DataParam.ParamType.VARCHAR, data.Destino));

			}
			var ds = ExecuteProcedure("dbo.sp_cms_plantilla", p);
			CloseCon();
			return ds;
		}
		private List<Plantilla> SetModel(DataSet ds)
		{
			var lst = new List<Plantilla>();
			lst.AddRange(from DataRow dr in ds.Tables[0].Rows
						 select new Plantilla
						 {
							 Id = GetField(dr, "id").ValidateInt32() ?? 0,
							 Tipo = GetField(dr, "tipo").ValidateString() ?? "",
							 Nombre = GetField(dr, "nombre").ValidateString() ?? "",
							 Asunto = GetField(dr, "asunto").ValidateString() ?? "",
							 Contenido = GetField(dr, "contenido").ToString(),
							 Categoria = GetField(dr, "categoria").ValidateString() ?? "",
							 Activo = GetField(dr, "activo").ValidateBoolean() ?? false,
							 Campos = GetField(dr, "campos").ValidateString() ?? "",
							 Destino = GetField(dr, "destino").ValidateString() ?? "",

						 });
			return lst;
		}

	}}
