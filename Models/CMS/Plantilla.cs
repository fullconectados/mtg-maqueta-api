// ***********************************************************************
// Assembly         : metrogas.api
// Author           : Full Conectados Ltda <software@fullconectados.cl>
// Created          : 09-17-2021
//
// Last Modified By : ginom
// Last Modified On : 09-18-2021
// ***********************************************************************
// <copyright file="Plantilla.cs" company="Full Conectados Ltda">
//     Copyright (c) 2021. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;

namespace metrogas.api.Models.CMS
{
    /// <summary>
    /// Class Plantilla.
    /// </summary>
    public class Plantilla
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the tipo.
        /// </summary>
        /// <value>
        /// The tipo.
        /// </value>
        public string Tipo { get; set; }
        /// <summary>
        /// Gets or sets the nombre.
        /// </summary>
        /// <value>
        /// The nombre.
        /// </value>
        public string Nombre { get; set; }
        /// <summary>
        /// Gets or sets the asunto.
        /// </summary>
        /// <value>
        /// The asunto.
        /// </value>
        public string Asunto { get; set; }
        /// <summary>
        /// Gets or sets the contenido.
        /// </summary>
        /// <value>
        /// The contenido.
        /// </value>
        public string Contenido { get; set; }
        /// <summary>
        /// Gets or sets the categoria.
        /// </summary>
        /// <value>
        /// The categoria.
        /// </value>
        public string Categoria { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Plantilla"/> is activo.
        /// </summary>
        /// <value>
        ///   <c>true</c> if activo; otherwise, <c>false</c>.
        /// </value>
        public bool Activo { get; set; }
        /// <summary>
        /// Gets or sets the campos.
        /// </summary>
        /// <value>
        /// The campos.
        /// </value>
        public string Campos { get; set; }
        /// <summary>
        /// Gets or sets the destino.
        /// </summary>
        /// <value>
        /// The destino.
        /// </value>
        public string Destino { get; set; }

    }
}
