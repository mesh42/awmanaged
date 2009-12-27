using AwManaged.SceneNodes.Interfaces;

namespace AwManaged.EventHandling.Interfaces
{
    public interface IEventObjectAddArgs<TModel,TAvatar> where TModel : IModel<TModel> where TAvatar : IAvatar<TAvatar>
    {
        /// <summary>
        /// The object the user added.
        /// </summary>
        /// <value>The object.</value>
        TModel Model { get; }
        /// <summary>
        /// The user who added the object.
        /// </summary>
        /// <value>The avatar.</value>
        TAvatar Avatar { get; }
    }
}