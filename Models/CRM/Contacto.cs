// ***********************************************************************
// Assembly         : metrogas.api
// Author           : ginom
// Created          : 06-13-2022
//
// Last Modified By : ginom
// Last Modified On : 06-13-2022
// ***********************************************************************
// <copyright file="Noticia.cs" company="Full Conectados Ltda">
//     @metrogas 2022
// </copyright>
// <summary></summary>
// ***********************************************************************
using metrogas.api.Models.CRM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace metrogas.api.Models.CRM
{
    /// <summary>
    /// Class Noticia.
    /// </summary>
    public class Contacto
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the fecha.
        /// </summary>
        /// <value>
        /// The fecha.
        /// </value>
        public DateTime Fecha { get; set; }
        /// <summary>
        /// Gets or sets the nombre.
        /// </summary>
        /// <value>
        /// The nombre.
        /// </value>
        public string Nombre { get; set; }
        /// <summary>
        /// Gets or sets the telefono.
        /// </summary>
        /// <value>
        /// The telefono.
        /// </value>
        public string Telefono { get; set; }
        /// <summary>
        /// Gets or sets the correo.
        /// </summary>
        /// <value>
        /// The correo.
        /// </value>
        public string Correo { get; set; }
        /// <summary>
        /// Gets or sets the atributos.
        /// </summary>
        /// <value>
        /// The atributos.
        /// </value>
        public string Atributos { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Contacto" /> is estado.
        /// </summary>
        /// <value>
        ///   <c>true</c> if estado; otherwise, <c>false</c>.
        /// </value>
        public bool Estado { get; set; }
        /// <summary>
        /// Gets or sets the plantilla identifier.
        /// </summary>
        /// <value>
        /// The plantilla identifier.
        /// </value>
        public int Plantilla_id { get; set; }
    }
}
