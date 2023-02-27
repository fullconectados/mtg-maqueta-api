// ***********************************************************************
// Assembly         : metrogas.api
// Author           : Full Conectados Ltda <software@fullconectados.cl>
// Created          : 09-17-2021
//
// Last Modified By : ginom
// Last Modified On : 10-25-2021
// ***********************************************************************
// <copyright file="Cliente.cs" company="Full Conectados Ltda">
//     Copyright (c) 2021. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace metrogas.api.Models.CRM
{
    /// <summary>
    /// Class Cliente.
    /// </summary>
    public class Cliente
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the ic.
        /// </summary>
        /// <value>The ic.</value>
        public int Ic { get; set; }
        /// <summary>
        /// Gets or sets the rut.
        /// </summary>
        /// <value>The rut.</value>
        public string Rut { get; set; }
        /// <summary>
        /// Gets or sets the nombre.
        /// </summary>
        /// <value>The nombre.</value>
        public string Nombre { get; set; }
        /// <summary>
        /// Gets or sets the paterno.
        /// </summary>
        /// <value>The paterno.</value>
        public string Paterno { get; set; }
        /// <summary>
        /// Gets or sets the materno.
        /// </summary>
        /// <value>The materno.</value>
        public string Materno { get; set; }
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
        /// Gets or sets the calle identifier.
        /// </summary>
        /// <value>The calle identifier.</value>
        public PRM.Calle Calle { get; set; }
        /// <summary>
        /// Gets or sets the numero.
        /// </summary>
        /// <value>The numero.</value>
        public string Numero { get; set; }
        /// <summary>
        /// Gets or sets the complemento.
        /// </summary>
        /// <value>The complemento.</value>
        public string Complemento { get; set; }
        /// <summary>
        /// Gets or sets the complemento.
        /// </summary>
        /// <value>The complemento.</value>
        public string Casa { get; set; }
        /// <summary>
        /// Gets or sets the block.
        /// </summary>
        /// <value>The block.</value>
        public string Block { get; set; }
        /// <summary>
        /// Gets or sets the adicional.
        /// </summary>
        /// <value>The adicional.</value>
        public string Adicional { get; set; }
        /// <summary>
        /// Gets or sets the lista.
        /// </summary>
        /// <value>The lista.</value>
        public string Lista { get; set; }

        /// <summary>
        /// Gets or sets the campanha.
        /// </summary>
        /// <value>The campanha.</value>
        public int Campanha { get; set; }
    }
}
