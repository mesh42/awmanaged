using AwManaged.Core.Interfaces;
using AwManaged.EventHandling.Interfaces;
using AwManaged.SceneNodes;

namespace AwManaged.EventHandling
{
    public sealed class EventObjectAddArgs : IEventObjectAddArgs<Model,Avatar>
    {
        public Model Model { get; private set; }
        public Avatar Avatar { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventObjectAddArgs"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="avatar">The avatar.</param>
        public EventObjectAddArgs(ICloneableT<Model> model, ICloneableT<Avatar> avatar)
        {
            Model = model.Clone();
            Avatar = avatar.Clone();
        }
    }
}