// ***********************************************************************
// Assembly         : metrogas.api
// Author           : Full Conectados Ltda <software@fullconectados.cl>
// Created          : 09-17-2021
//
// Last Modified By : ginom
// Last Modified On : 09-19-2021
// ***********************************************************************
// <copyright file="Usuario.cs" company="Full Conectados Ltda">
//     Copyright (c) 2021. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;

namespace metrogas.api.Models
{
    /// <summary>
    /// Class Usuario.
    /// </summary>
    public class Usuario
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        /// <value>The username.</value>
        public string Username { get; set; }
        /// <summary>
        /// Gets or sets the nombre.
        /// </summary>
        /// <value>The nombre.</value>
        public string Nombre { get; set; }
        /// <summary>
        /// Gets or sets the apellido.
        /// </summary>
        /// <value>The apellido.</value>
        public string Apellido { get; set; }
        /// <summary>
        /// Gets or sets the telefono.
        /// </summary>
        /// <value>The telefono.</value>
        public string Telefono { get; set; }
        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        public string Email { get; set; }
        /// <summary>
        /// Gets or sets the cargo.
        /// </summary>
        /// <value>The cargo.</value>
        public string Cargo { get; set; }
        /// <summary>
        /// Gets or sets the rol identifier.
        /// </summary>
        /// <value>The rol identifier.</value>
        public _BaseModel Rol { get; set; }
        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>The password.</value>
        public string Password { get; set; }
        /// <summary>
        /// Gets or sets the acceso.
        /// </summary>
        /// <value>The acceso.</value>
        public DateTime? Acceso { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Usuario" /> is activo.
        /// </summary>
        /// <value><c>true</c> if activo; otherwise, <c>false</c>.</value>
        public bool Activo { get; set; }

    }
}
