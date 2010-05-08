using SharedMemory;using System;
using System.ComponentModel;
using AwManaged.Core.Interfaces;

namespace AwManagedIde
{
    public class FolderNode : IIdentifiable
    {
        public FolderNode(string displayName)
        {
            IdentifyableId = Guid.NewGuid();
            IdentifyableDisplayName = displayName;
        }

        #region IIdentifiable Members

        [Browsable(false)]
        public string IdentifyableDisplayName
        {
            get; set;
        }

        [Browsable(false)]
        public Guid IdentifyableId
        {
            get; set;
        }

        [Browsable(false)]
        public string IdentifyableTechnicalName
        {
            get; set;
        }

        #endregion
    }
}
