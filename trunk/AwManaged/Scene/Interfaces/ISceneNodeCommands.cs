using System;
using AwManaged.Scene.Interfaces;

namespace AwManaged.Scene.Interfaces
{
    /// <summary>
    /// Scene node commands, allow for interacting with objects such as Camera's, Movers and Models (objects) in a managed fashion.
    /// </summary>
    public interface ISceneNodeCommands<TModel,TAvatar,THud>
        where TModel : MarshalByRefObject, IModel<TModel>
        where TAvatar : MarshalByRefObject, IAvatar<TAvatar>
        where THud : MarshalByRefObject, IHudBase<THud,TAvatar>
    {
        /// <summary>
        /// Displays a hud to the specified avatar.
        /// </summary>
        /// <param name="hud">The hud.</param>
        /// <param name="avatar">The avatar.</param>
        void HudDisplay(THud hud, TAvatar avatar);
        /// <summary>
        /// Deletes the V3 object.
        /// </summary>
        /// <param name="o">The o.</param>
        void DeleteObject(TModel o);
        /// <summary>
        /// Adds the V3 object.
        /// </summary>
        /// <param name="o">The o.</param>
        void AddObject(TModel o);
        /// <summary>
        /// Changes the object.
        /// </summary>
        /// <param name="model">The model.</param>
        void ChangeObject(TModel model);
        /// <summary>
        /// Changes the object with a specified delay.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="delay">The delay.</param>
        void ChangeObject(TModel model, int delay);
    }
}