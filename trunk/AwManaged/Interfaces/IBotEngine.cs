using AwManaged.Configuration;
using AwManaged.Configuration.Interfaces;
using AwManaged.ExceptionHandling;
using AwManaged.Interfaces;
using AwManaged.SceneNodes.Interfaces;

namespace AwManaged.Interfaces
{
    /// <summary>
    /// Main Interface for a bot engine. The interface is build up as such, that a bot implementation can
    /// use different types of implementations in order to support a factory pattern.
    /// </summary>
    public interface IBotEngine<TAvatar,TModel,TCamera,TZone, TMover, THudBase> : 
        IHandleExceptionManaged,
        IBotEvents, ISceneNodeCommands<TModel,TAvatar,THudBase>,
        IChatCommands<TAvatar>,
        ISceneNodes<TModel,TCamera,TMover,TZone,THudBase,TAvatar>,
        IAvatarCommands<TAvatar>, IConfigurable
        where TModel : IModel<TModel>
        where TAvatar : IAvatar<TAvatar>
        where TCamera : ICamera<TCamera>
        where TMover : IMover<TMover>
        where TZone : IZone<TZone,TModel,TCamera>
        where THudBase : IHudBase<THudBase, TAvatar>
    {
        LoginConfiguration LoginConfiguration { get; }
        /// <summary>
        /// Indicates if the privileged user account is logged on in global mode (Caretaker capable).
        /// </summary>
        bool IsEnterGlobal { get; }
        /// <summary>
        /// Indicates to echo all chat messages that are received. Helpfull for color bots, when the world attributes
        /// are set to not echo the chat.
        /// </summary>
        bool IsEchoChat { get; set; }
        /// <summary>
        /// Scan's objects in either global or non global mode. (auto sensing mode)
        /// </summary>
        void ScanObjects();
        /// <summary>
        /// Starts this bot instance by logging in to the world under
        /// Prefered Care Taker privileges (enter global mode). If the privileged user account
        /// does not have caretaker capabilities for the world, we will automatically
        /// downgrade the privileges so the user can log on non-global.
        /// </summary>
        void Start();
        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        void Dispose();
    }
}