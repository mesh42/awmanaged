using System;
using AwManaged.Core.Interfaces;
using AwManaged.SceneNodes.Interfaces;

namespace AwManaged.SceneNodes
{
    public sealed class World : IWorld<World>
    {
        private readonly Guid id;

        public World(string name)
        {
            Name = name;
        }

        public World(Guid id)
        {
            this.id = id;
        }

        public string Name { get; set; }

        public Guid Id
        {
            get { return id; }
        }

        #region ICloneableT<IWorld> Members

        public World Clone()
        {
            return (World) MemberwiseClone();
        }

        #endregion

        #region IChanged<World> Members

        public event ChangedEventDelegate<World> OnChanged;

        public bool IsChanged { get; internal set; }

        #endregion
    }
}