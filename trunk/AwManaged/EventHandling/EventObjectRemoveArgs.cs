using AwManaged.Core.Interfaces;
using AwManaged.EventHandling.Interfaces;
using AwManaged.SceneNodes;

namespace AwManaged.EventHandling
{
    public sealed class EventObjectRemoveArgs : IEventObjectRemoveArgs<Model,Avatar>
    {
        #region IEventObjectRemoveArgs<Model,Avatar> Members

        public Model Model { get; private set;}
        public Avatar Avatar { get; private set;}

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="EventObjectRemoveArgs"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="avatar">The avatar.</param>
        public EventObjectRemoveArgs(ICloneableT<Model> model, ICloneableT<Avatar> avatar )
        {
            Model = model.Clone();
            Avatar = avatar.Clone();
        }
    }
}
