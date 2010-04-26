using System.Collections.Generic;
using AwManaged.Core.Interfaces;

namespace AwManaged.Core.Patterns.Tree
{
    public class TreeNodeT
    {
        private IIdentifiable _parent;
        public List<IIdentifiable> Children;

        public TreeNodeT(IIdentifiable parent)
        {
            _parent = parent;
            Children = new List<IIdentifiable>();
        }

        public IIdentifiable Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }
    }

    public interface ITreeNode : IIdentifiable
    {
    }
}
