/* **********************************************************************************
 *
 * Copyright (c) TCPX. All rights reserved.
 *
 * This source code is subject to terms and conditions of the Microsoft Public
 * License (Ms-PL). A copy of the license can be found in the license.txt file
 * included in this distribution.
 *
 * You must not remove this notice, or any other, from this software.
 *
 * **********************************************************************************/
using System;
using System.ComponentModel;

namespace AwManaged.Core.Interfaces
{
    public interface IIdentifiable
    {
        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        /// <value>The display name.</value>
        [Description("Displayed name of the object for user interface purposes.")]
        [Category("Identification")]
        [ReadOnly(true)]
        string DisplayName { get; }
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        [Description("Globally unique identification of the object.")]
        [Category("Identifiaction")]
        [ReadOnly(true)]
        Guid Id { get; }
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        [Description("Technical name of the object.")]
        [Category("Identifiaction")]
        [ReadOnly(true)]
        string TechnicalName { get; }


    }
}