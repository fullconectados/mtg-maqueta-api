// ***********************************************************************
// Assembly         : metrogas.api
// Author           : Full Conectados Ltda <software@fullconectados.cl>
// Created          : 09-18-2021
//
// Last Modified By : ginom
// Last Modified On : 09-18-2021
// ***********************************************************************
// <copyright file="Comuna.cs" company="Full Conectados Ltda">
//     Copyright (c) 2021. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace metrogas.api.Models.PRM
{
    /// <summary>
    /// Class Comuna.
    /// </summary>
    public class Comuna
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the region identifier.
        /// </summary>
        /// <value>The region identifier.</value>
        public _BaseModel Region { get; set; }
        /// <summary>
        /// Gets or sets the nombre.
        /// </summary>
        /// <value>The nombre.</value>
        public string Nombre { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Comuna" /> is activo.
        /// </summary>
        /// <value><c>true</c> if activo; otherwise, <c>false</c>.</value>
        public bool Activo { get; set; }
    }
}
