using metrogas.api.Core;
using metrogas.api.Models.CRM;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace metrogas.api.Business.CRM
{

    /// <summary>
    /// Clase control de datos para Contacto
    /// </summary>
    /// <seealso cref="metrogas.api.Core.CoreConnection" />
    public class Contactos : CoreConnection
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="Contactos"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public Contactos(IConfiguration configuration) : base(configuration) { }
        /// <summary>
        /// Gets the todos.
        /// </summary>
        /// <param name="opc">The opc.</param>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public List<Contacto> GetTodos(int opc, Contacto filter)
		{
			var ds = Execute(1, opc, filter);
			var lst = SetModel(ds);
			return lst;
		}
        /// <summary>
        /// Gets the registro.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public Contacto GetRegistro(int id)
		{
			var ds = Execute(1, 1, new Contacto() { Id = id });
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
        /// <returns></returns>
        public bool SaveRegistro(int opc, Contacto data)
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
        /// <returns></returns>
        public bool ActiveRegistro(int id)
		{
			Execute(3, 2, new Contacto() { Id = id });
			return true;
		}
        /// <summary>
        /// Executes the specified accion.
        /// </summary>
        /// <param name="accion">The accion.</param>
        /// <param name="opcion">The opcion.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        private DataSet Execute(int accion, int opcion = 1, Contacto data = null)
		{
			OpenCon();
			var p = new List<DataParam>(){
			new DataParam("ACCION", DataParam.ParamType.INT, accion),
			new DataParam("OPCION", DataParam.ParamType.INT, opcion),
		};
			if (data != null)
			{
				p.Add(new DataParam("id", DataParam.ParamType.INT, data.Id));
				p.Add(new DataParam("fecha", DataParam.ParamType.DATETIME, data.Fecha));
				p.Add(new DataParam("nombre", DataParam.ParamType.VARCHAR, data.Nombre));
				p.Add(new DataParam("telefono", DataParam.ParamType.VARCHAR, data.Telefono));
				p.Add(new DataParam("correo", DataParam.ParamType.VARCHAR, data.Correo));
				p.Add(new DataParam("atributos", DataParam.ParamType.VARCHAR, data.Atributos));
				p.Add(new DataParam("estado", DataParam.ParamType.BIT, data.Estado));
				p.Add(new DataParam("plantilla_id", DataParam.ParamType.TINYINT, data.Plantilla_id));

			}
			var ds = ExecuteProcedure("dbo.sp_crm_contacto", p);
			CloseCon();
			return ds;
		}
        /// <summary>
        /// Sets the model.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        private List<Contacto> SetModel(DataSet ds)
		{
			var lst = new List<Contacto>();
			lst.AddRange(from DataRow dr in ds.Tables[0].Rows
						 select new Contacto
						 {
							 Id = GetField(dr, "id").ValidateInt32() ?? 0,
							 Fecha = GetField(dr, "fecha").ValidateDateTime() ?? System.DateTime.MinValue,
							 Nombre = GetField(dr, "nombre").ValidateString() ?? "",
							 Telefono = GetField(dr, "telefono").ValidateString() ?? "",
							 Correo = GetField(dr, "correo").ValidateString() ?? "",
							 Atributos = GetField(dr, "atributos").ValidateString() ?? "",
							 Estado = GetField(dr, "estado").ValidateBoolean() ?? false,
							 Plantilla_id = GetField(dr, "plantilla_id").ValidateInt32() ?? 0,

						 });
			return lst;
		}

	}}
