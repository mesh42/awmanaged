namespace AwManaged.EventHandling.BotEngine.Interfaces
{
    public interface IBotEngineEvents
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
