// ***********************************************************************
// Assembly         : metrogas.api
// Author           : Full Conectados Ltda <software@fullconectados.cl>
// Created          : 09-18-2021
//
// Last Modified By : ginom
// Last Modified On : 09-19-2021
// ***********************************************************************
// <copyright file="Calle.cs" company="Full Conectados Ltda">
//     Copyright (c) 2021. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace metrogas.api.Models.PRM
{
    /// <summary>
    /// Class Calle.
    /// </summary>
    public class Calle
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the comuna identifier.
        /// </summary>
        /// <value>The comuna identifier.</value>
        public _BaseModel Comuna { get; set; }
        /// <summary>
        /// Gets or sets the region identifier.
        /// </summary>
        /// <value>The comuna identifier.</value>
        public _BaseModel Region { get; set; }
        /// <summary>
        /// Gets or sets the nombre.
        /// </summary>
        /// <value>The nombre.</value>
        public string Nombre { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Calle" /> is activo.
        /// </summary>
        /// <value><c>true</c> if activo; otherwise, <c>false</c>.</value>
        public bool Activo { get; set; }

        /// <summary>
        /// Gets or sets the adicional.
        /// </summary>
        /// <value>The adicional.</value>
        public string Adicional { get; set; }
    }
}
