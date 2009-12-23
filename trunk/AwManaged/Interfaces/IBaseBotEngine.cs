using System;
using AwManaged.Configuration;
using AwManaged.Configuration.Interfaces;
using AwManaged.Interfaces;
using AwManaged.SceneNodes;

namespace AwManaged.Interfaces
{
    /// <summary>
    /// Interface for a compiled bot engine.
    /// Note: this is not used for interaction scripts but for compiled bots. Runtime Scripted bots use this instance through the IBotInteraction script interface.
    /// </summary>
    public interface IBaseBotEngine : IBotEvents, ISceneNodeCommands, IChatCommands, ISceneNodes, IAvatarCommands, IConfigurable
    {
        IBaseBotEngine BotEngine { get; }
        LoginConfiguration LoginConfiguration { get; }

        bool IsEnterGlobal { get; }
        bool IsEchoChat { get; set; }
        void ScanObjects();
        void Start();

        void ChangeObjectAction(Model o);

        void AwExceptionHandler(Exception ex);
        void Dispose();
    }
}