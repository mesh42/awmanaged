using System;
using AwManaged.SceneNodes.Interfaces;

namespace AwManaged.SceneNodes
{
    public class World : IWorld
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
    }
}