using AwManaged.Core.Interfaces;
using AwManaged.EventHandling.Interfaces;
using AwManaged.SceneNodes;
using Model=AwManaged.SceneNodes.Model;

namespace AwManaged.EventHandling
{
    public sealed class EventObjectChangeArgs : IEventObjectChangeArgs<SceneNodes.Model, SceneNodes.Avatar>
    {
        public SceneNodes.Model Model { get; private set; }
        public SceneNodes.Avatar Avatar { get; private set; }
        public SceneNodes.Model OldModel { get; private set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="EventObjectChangeArgs"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="oldModel">The old model.</param>
        /// <param name="avatar">The avatar.</param>
        public EventObjectChangeArgs(ICloneableT<Model> model, ICloneableT<Model> oldModel, ICloneableT<Avatar> avatar)
        {
            Model = model.Clone();
            OldModel = oldModel.Clone();
            Avatar = avatar.Clone();
        }
    }
}
