using SharedMemory;using System;
using AwManaged.Core.Interfaces;

namespace AwManaged.Core.Patterns.Tree
{
    public sealed class Tree : IIdentifiable
    {
        public TreeNodeT Root;

        public Tree()
        {
            Root = new TreeNodeT(null);
        }

        #region IIdentifiable Members

        public string IdentifyableDisplayName
        {
            get { return "root"; }
        }

        public System.Guid IdentifyableId
        {
            get { return Guid.Empty; }
        }

        public string IdentifyableTechnicalName
        {
            get { return "tree"; }
        }

        #endregion
    }
}
