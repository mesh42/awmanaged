namespace AwManaged.Interfaces
{
    /// <summary>
    /// Interface which exposes the events of a bot instance
    /// </summary>
    public interface IBotEvents
    {
        event BaseBotEngine.BotEventEntersWorldDelegate BotEventEntersWorld;
        event BaseBotEngine.BotEventLoggedInDelegate BotEventLoggedIn;
        event BaseBotEngine.AvatarEventAddDelegate AvatarEventAdd;
        event BaseBotEngine.AvatarEventChangeDelegate AvatarEventChange;
        event BaseBotEngine.AvatarEventRemoveDelegate AvatarEventRemove;

        event BaseBotEngine.ObjectEventClickDelegate ObjectEventClick;
        event BaseBotEngine.ObjectEventAddDelegate ObjectEventAdd;
        event BaseBotEngine.ObjectEventRemoveDelegate ObjectEventRemove;
        event BaseBotEngine.ObjectEventScanCompletedDelegate ObjectEventScanCompleted;
        event BaseBotEngine.ChatEventDelegate ChatEvent;

    }
}
