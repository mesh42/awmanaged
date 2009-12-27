using AwManaged.Core;
using AwManaged.SceneNodes.Interfaces;

namespace AwManaged.SceneNodes.Interfaces
{
    /// <summary>
    /// Managed scene nodes.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TCamera">The type of the camera.</typeparam>
    /// <typeparam name="TMover">The type of the mover.</typeparam>
    /// <typeparam name="TZone">The type of the zone.</typeparam>
    /// <typeparam name="THudBase">The type of the hud base.</typeparam>
    /// <typeparam name="TAvatar">The type of the avatar.</typeparam>
    public interface ISceneNodes<TModel,TCamera,TMover, TZone, THudBase,TAvatar>
        where TModel : IModel<TModel>
        where TCamera : ICamera<TCamera>
        where TMover : IMover<TMover>
        where THudBase : IHudBase<THudBase, TAvatar>
        where TAvatar : IAvatar<TAvatar>
        where TZone : IZone<TZone,TModel,TCamera>
    {
        /// <summary>
        /// Gets a shallow memberwise clone of the model cache.
        /// </summary>
        /// <value>The model.</value>
        ProtectedList<TModel> Model { get; }
        /// <summary>
        /// Gets a shallow memberwise clone of the camera cache.
        /// </summary>
        /// <value>The cameras.</value>
        ProtectedList<TCamera> Cameras { get; }
        /// <summary>
        /// Gets a shallow memberwise clone of the movers cache.
        /// </summary>
        /// <value>The movers.</value>
        ProtectedList<TMover> Movers { get; }
        /// <summary>
        /// Gets a shallow memberwise clone of the zones cache.
        /// </summary>
        /// <value>The zones.</value>
        ProtectedList<TZone> Zones { get; }
        /// <summary>
        /// Gets a shallow memberwise clone of the huds cache.
        /// </summary>
        /// <value>The huds.</value>
        ProtectedList<THudBase> Huds { get; }
    }
}