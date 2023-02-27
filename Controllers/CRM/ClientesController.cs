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

namespace metrogas.api.Controllers
{
    /// <summary>
    /// Clase controladora para API - ClientesController.
    /// </summary>
    [Produces("application/json")]
    [Route("api/crm/[controller]")]
    [ApiController]
    public class ClientesController : CoreController
    {
        /// <summary>
        /// The bs
        /// </summary>
        private readonly Business.CRM.Clientes _bs;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientesController" /> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public ClientesController(IConfiguration configuration)
        {
            _bs = new Business.CRM.Clientes(configuration);
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
            var opt = new CoreFilter<Cliente>();
            if (filter != null)
            {
                var tmp = new CoreRequest() { Data = filter };
                opt = tmp.GetObject<CoreFilter<Cliente>>();
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
            var tmp = value.GetObject<Cliente>();
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
            var tmp = value.GetObject<Cliente>();
            tmp.Id = id;
            _bs.SaveRegistro(value.Mode, tmp);
            return CoreResponse.SetSuccess("actualizado");
        }

    }
}
