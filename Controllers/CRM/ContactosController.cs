// ***********************************************************************
// Assembly         : metrogas.api
// Author           : Full Conectados Ltda <software@fullconectados.cl>
// Created          : 09-18-2021
//
// Last Modified By : ginom
// Last Modified On : 09-19-2021
// ***********************************************************************
// <copyright file="ClientesController.cs" company="Full Conectados Ltda">
//     Copyright (c) 2021. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using metrogas.api.Core;
using metrogas.api.Models.CRM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace metrogas.api.Controllers
{
    /// <summary>
    /// Clase controladora para API - ClientesController.
    /// </summary>
    [Produces("application/json")]
    [Route("api/crm/[controller]")]
    [ApiController]
    public class ContactosController : CoreController
    {
        /// <summary>
        /// The bs
        /// </summary>
        private readonly Business.CRM.Contactos _bs;
        private readonly Business.CMS.Plantillas _mail;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientesController" /> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public ContactosController(IConfiguration configuration)
        {
            Config = configuration;
            _bs = new Business.CRM.Contactos(configuration);
            _mail = new Business.CMS.Plantillas(configuration);
        }

        /// <summary>
        /// Selector general de datos - HTTP GET
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>CoreResponse.</returns>
        [HttpGet]
        public CoreResponse Get(string filter = null)
        {
            if (string.IsNullOrWhiteSpace(filter)) return CoreResponse.NoData();
            var opt = new CoreFilter<Contacto>();
            if (filter != null)
            {
                var tmp = new CoreRequest() { Data = filter };
                opt = tmp.GetObject<CoreFilter<Contacto>>();
            }
            var lst = _bs.GetTodos(opt.Mode, opt.Data);
            return CoreResponse.SetSuccess(lst);
        }

        /// <summary>
        /// Selector individual de datos - HTTP GET
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>CoreResponse.</returns>
        [HttpGet("{id}")]
        public CoreResponse Get(int id)
        {
            var tmp = _bs.GetRegistro(id);
            if (tmp != null)
                return CoreResponse.SetSuccess(tmp);
            return CoreResponse.SetNotFound();
        }

        /// <summary>
        /// Metodo para creacion de nuevo registro - HTTP POST
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>CoreResponse.</returns>
        [HttpPost]
        public CoreResponse Post([FromBody] CoreRequest value)
        {
            var tmp = value.GetObject<Contacto>();
            tmp.Id = 0;
            _bs.SaveRegistro(value.Mode, tmp);

            //SEND EMAIL
            var template = _mail.GetRegistro(tmp.Plantilla_id);
            var mensaje = System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(template.Contenido));
            var atributos = System.Net.WebUtility.UrlDecode(tmp.Atributos);
            var serializeOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            };
            var result = JsonSerializer.Deserialize<dynamic>(atributos, serializeOptions);

            mensaje = mensaje.Replace("${nombre}", tmp.Nombre);
            mensaje = mensaje.Replace("${telefono}", tmp.Telefono);
            mensaje = mensaje.Replace("${correo}", tmp.Correo);
            mensaje = mensaje.Replace("${direccion}", GetDataString(result, "direccion"));
            mensaje = mensaje.Replace("${comercio}", GetDataString(result, "comercio"));
            mensaje = mensaje.Replace("${comuna}", GetDataString(result, "comuna"));
            mensaje = mensaje.Replace("${calle}", GetDataString(result, "calle"));
            mensaje = mensaje.Replace("${numero_casa}", GetDataString(result, "numero_casa"));
            mensaje = mensaje.Replace("${tipo_domicilio}", GetDataString(result, "tipo_domicilio"));
            mensaje = mensaje.Replace("${numero_cliente}", GetDataString(result, "numero_cliente"));
            mensaje = mensaje.Replace("${relacion}", GetDataString(result, "relacion"));
            mensaje = mensaje.Replace("${motivo}", GetDataString(result, "motivo"));

            var asunto = template.Asunto;
            asunto = asunto.Replace("${correo}", tmp.Correo);
            asunto = asunto.Replace("${nombre}", tmp.Nombre);
            SendMail(
                template.Categoria,
                template.Destino,
                template.Destino,
                asunto,
                mensaje
            );

            return CoreResponse.SetSuccess("creado");
        }

        protected string GetDataString(JsonElement json, string name)
        {
            try
            {
                return json.GetProperty(name).GetString();
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// Metodo para actualización de registro - HTTP PUT
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="value">The value.</param>
        /// <returns>CoreResponse.</returns>
        [HttpPut("{id}")]
        public CoreResponse Put(int id, [FromBody] CoreRequest value)
        {
            var tmp = value.GetObject<Contacto>();
            tmp.Id = id;
            _bs.SaveRegistro(value.Mode, tmp);
            return CoreResponse.SetSuccess("actualizado");
        }

        /// <summary>
        /// Metodo para actualización de vigencia de registro - HTTP DELETE
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>CoreResponse.</returns>
        [HttpDelete("{id}")]
        public CoreResponse Delete(int id)
        {
            _bs.ActiveRegistro(id);
            return CoreResponse.SetSuccess("cambio vigencia");
        }

    }
}
