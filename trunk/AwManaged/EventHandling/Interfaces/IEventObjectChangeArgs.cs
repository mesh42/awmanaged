using AwManaged.SceneNodes.Interfaces;

namespace AwManaged.EventHandling.Interfaces
{
    public interface IEventObjectChangeArgs<TModel,TAvatar> where TModel : IModel<TModel> where TAvatar : IAvatar<TAvatar>
    {
        /// <summary>
        /// The object the user changed.
        /// </summary>
        /// <value>The object.</value>
        TModel Model { get; }
        /// <summary>
        /// The old object before the user changed it.
        /// </summary>
        /// <value>The object.</value>
        TModel OldModel { get; }
        /// <summary>
        /// The user who changed the object.
        /// </summary>
        /// <value>The avatar.</value>
        TAvatar Avatar { get; }
    }
}