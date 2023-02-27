// ***********************************************************************
// Assembly         : metrogas.api
// Author           : Full Conectados Ltda <software@fullconectados.cl>
// Created          : 09-19-2021
//
// Last Modified By : ginom
// Last Modified On : 09-19-2021
// ***********************************************************************
// <copyright file="CoreFilter.cs" company="Full Conectados Ltda">
//     @metrogas 2021
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.ComponentModel;

namespace metrogas.api.Core
{
    /// <summary>
    /// Class CoreFilter.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CoreFilter<T> where T:class
    {
        /// <summary>
        /// Gets or sets the mode.
        /// </summary>
        /// <value>The mode.</value>
        [DefaultValue(1)]
        public int Mode { get; set; } = 1;

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>The data.</value>
        public T Data { get; set; }
    }
}
