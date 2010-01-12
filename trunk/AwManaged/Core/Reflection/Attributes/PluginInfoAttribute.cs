using System;

namespace AwManaged.Core.Reflection.Attributes
{
    public sealed class PluginInfoAttribute : Attribute
    {
        public string TechnicalName { get; private set; }
        public string Description { get; private set; }

        public PluginInfoAttribute(string technicalName, string description)
        {
            TechnicalName = technicalName;
            Description = description;
        }
    }
}
