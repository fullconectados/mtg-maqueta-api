// ***********************************************************************
// Assembly         : metrogas.api
// Author           : Full Conectados Ltda <software@fullconectados.cl>
// Created          : 09-17-2021
//
// Last Modified By : ginom
// Last Modified On : 09-19-2021
// ***********************************************************************
// <copyright file="HomeController.cs" company="Full Conectados Ltda">
//     Copyright (c) 2021. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using metrogas.api.Core;
using metrogas.api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;

namespace metrogas.api.Controllers
{
    /// <summary>
    /// Clase controladora para API - HomeController.
    /// </summary>
    [Produces("application/json")]
    [Route("/")]
    [ApiController]
    public class HomeController : CoreController
    {
       
        /// <summary>
        /// Selector general de datos - HTTP GET
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>CoreResponse.</returns>
        [HttpGet]
        public dynamic Get()
        {
            return new { message = "Metrogas - Api - " + DateTime.Today.Year } ;
        }
    }
}
