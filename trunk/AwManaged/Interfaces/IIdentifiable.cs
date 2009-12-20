using System;
using System.ComponentModel;

namespace Aw.Common
{
    public interface IIdentifiable
    {
        [Description("Displayed name of the object for user interface purposes.")]
        [Category("Identification")]
        [ReadOnly(true)]
        string DisplayName { get; set; }
        [Description("Globally unique identification of the object.")]
        [Category("Identifiaction")]
        [ReadOnly(true)]
        Guid Id { get; set; }
    }
}