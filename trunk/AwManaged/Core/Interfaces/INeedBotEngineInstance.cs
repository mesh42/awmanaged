namespace AwManaged.Core.Interfaces
{
    public interface INeedBotEngineInstance<TBotEngine>
    {
        TBotEngine BotEngine { get; set; }
    }
}
