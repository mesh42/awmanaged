namespace AwManaged.EventHandling.RemotingBotEngine.Interfaces
{
    public interface IRemotingBotEngineEvents
    {
        event BotEventLoggedInDelegate BotEventLoggedIn;
        event BotEventEntersWorldDelegate BotEventEntersWorld;
        event AvatarEventAddDelegate AvatarEventAdd;
        event AvatarEventChangeDelegate AvatarEventChange;
        event AvatarEventRemoveDelegate AvatarEventRemove;
        event ObjectEventClickDelegate ObjectEventClick;
        event ObjectEventAddDelegate ObjectEventAdd;
        event ObjectEventRemoveDelegate ObjectEventRemove;
        event ObjectEventScanCompletedDelegate ObjectEventScanCompleted;
        event ChatEventDelegate ChatEvent;
        event ObjectEventChangeDelegate ObjectEventChange;
    }
}
