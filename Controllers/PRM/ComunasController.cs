// ***********************************************************************
// Assembly         : metrogas.api
// Author           : Full Conectados Ltda <software@fullconectados.cl>
// Created          : 09-18-2021
//
// Last Modified By : ginom
// Last Modified On : 09-19-2021
// ***********************************************************************
// <copyright file="ComunasController.cs" company="Full Conectados Ltda">
//     Copyright (c) 2021. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using metrogas.api.Core;
using metrogas.api.Models.PRM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace metrogas.api.Controllers
{
    /// <summary>
    /// Clase controladora para API - ComunasController.
    /// </summary>
    [Produces("application/json")]
    [Route("api/prm/[controller]")]
    [ApiController]
    public class ComunasController : CoreController
    {
        /// <summary>
        /// The bs
        /// </summary>
        private readonly Business.PRM.Comunas _bs;

        /// <summary>
        /// Initializes a new instance of the <see cref="ComunasController" /> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public ComunasController(IConfiguration configuration)
        {
            _bs = new Business.PRM.Comunas(configuration);
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
            var opt = new CoreFilter<Comuna>();
            if (filter != null) {
                var tmp = new CoreRequest() { Data = filter };
                opt = tmp.GetObject<CoreFilter<Comuna>>();
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
            var tmp = value.GetObject<Comuna>();
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
            var tmp = value.GetObject<Comuna>();
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
