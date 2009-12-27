namespace AwManaged.Interfaces
{
    /// <summary>
    /// Interface which exposes the events of a bot instance
    /// </summary>
    public interface IBotEvents
    {
        /// <summary>
        /// Occurs when [bot enters world].
        /// </summary>
        event BotEngine.BotEventEntersWorldDelegate BotEventEntersWorld;
        /// <summary>
        /// Occurs when [bot logged in].
        /// </summary>
        event BotEngine.BotEventLoggedInDelegate BotEventLoggedIn;
        /// <summary>
        /// Occurs when [avatar is added].
        /// </summary>
        event BotEngine.AvatarEventAddDelegate AvatarEventAdd;
        /// <summary>
        /// Occurs when [avatar has changed].
        /// </summary>
        event BotEngine.AvatarEventChangeDelegate AvatarEventChange;
        /// <summary>
        /// Occurs when [avatar has been removed].
        /// </summary>
        event BotEngine.AvatarEventRemoveDelegate AvatarEventRemove;
        /// <summary>
        /// Occurs when [avatar clicks on an object].
        /// </summary>
        event BotEngine.ObjectEventClickDelegate ObjectEventClick;
        /// <summary>
        /// Occurs when [an object is added to the world].
        /// </summary>
        event BotEngine.ObjectEventAddDelegate ObjectEventAdd;
        /// <summary>
        /// Occurs when [an object is removed from the world].
        /// </summary>
        event BotEngine.ObjectEventRemoveDelegate ObjectEventRemove;
        /// <summary>
        /// Occurs when [object event scan has been completed].
        /// </summary>
        event BotEngine.ObjectEventScanCompletedDelegate ObjectEventScanCompleted;
        /// <summary>
        /// Occurs when [a chat event has been received].
        /// </summary>
        event BotEngine.ChatEventDelegate ChatEvent;
        /// <summary>
        /// Occurs when [an object in the world has changed].
        /// </summary>
        event BotEngine.ObjectEventChangeDelegate ObjectEventChange;
    }
}
