// ***********************************************************************
// Assembly         : metrogas.api
// Author           : Full Conectados Ltda <software@fullconectados.cl>
// Created          : 09-17-2021
//
// Last Modified By : ginom
// Last Modified On : 09-19-2021
// ***********************************************************************
// <copyright file="UsuariosController.cs" company="Full Conectados Ltda">
//     Copyright (c) 2021. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using metrogas.api.Core;
using metrogas.api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace metrogas.api.Controllers
{
    /// <summary>
    /// Clase controladora para API - UsuariosController.
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : CoreController
    {
        /// <summary>
        /// The bs
        /// </summary>
        private readonly Business.Usuarios _bs;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsuariosController" /> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public UsuariosController(IConfiguration configuration)
        {
            _bs = new Business.Usuarios(configuration);
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
            var opt = new CoreFilter<Usuario>();
            if (filter != null)
            {
                var tmp = new CoreRequest() { Data = filter };
                opt = tmp.GetObject<CoreFilter<Usuario>>();
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
            if(tmp!=null)
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
            var tmp = value.GetObject<Usuario>();
            tmp.Id = 0;
            _bs.SaveRegistro(value.Mode, tmp);
            return CoreResponse.SetSuccess("creado");
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
            var tmp = value.GetObject<Usuario>();
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
