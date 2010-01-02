using System;
using System.ComponentModel;

namespace AwManaged.Interfaces
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
    }
}