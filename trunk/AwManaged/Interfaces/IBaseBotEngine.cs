using System;
using System.Collections.Generic;
using AW;
using AwManaged;
using AwManaged.Configuration;
using AwManaged.Configuration.Interfaces;
using AwManaged.Huds.Interfaces;
using AwManaged.Interfaces;
using AwManaged.Math;
using AwManaged.SceneNodes;
using AwManaged.SceneNodes.Interfaces;
using AWManaged.Security;

namespace AwManaged.Interfaces
{

    public interface IBaseBotEngine : IConfigurable
    {
        List<Model> Model { get; set; }
        List<Camera> Cameras { get; set; }
        List<Mover> Movers { get; set; }
        List<ZoneObject> Zones { get; set; }
        List<IHudBase> Huds { get; set; }

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

        IBaseBotEngine BotEngine { get; }
        LoginConfiguration LoginConfiguration { get; }

        bool IsEchoChat { get; set; }
        void ScanObjects();
        void Start();

        /// <summary>
        /// Whispers a message to all user within a specified role.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <param name="message">The message.</param>
        void Whisper(RoleType role, string message);

        /// <summary>
        /// Whispers a message to the current avatar session.
        /// </summary>
        /// <param name="message">The message.</param>
        void Whisper(string message);

        /// <summary>
        /// Whispers a message to the a specific avatar
        /// </summary>
        /// <param name="avatar">The avatar.</param>
        /// <param name="message">The message.</param>
        void Whisper(IAvatar avatar, string message);

        /// <summary>
        /// Deletes the V3 object.
        /// </summary>
        /// <param name="o">The o.</param>
        void DeleteObject(Model o);
        /// <summary>
        /// Deletes the V3 object by its id.
        /// </summary>
        /// <param name="id">The id.</param>
        void DeleteObject(int id);
        /// <summary>
        /// Adds the V3 object.
        /// </summary>
        /// <param name="o">The o.</param>
        void AddObject(Model o);
        /// <summary>
        /// Adds a new object.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <param name="model">The model.</param>
        /// <param name="position">The position.</param>
        /// <param name="rotation">The rotation.</param>
        /// <param name="action">The action.</param>
        void AddObject(string description, string model, Vector3 position, Vector3 rotation, string action);
        /// <summary>
        /// Changes the object.
        /// </summary>
        /// <param name="o">The o.</param>
        void ChangeObject(Model o);
        /// <summary>
        /// Sends a specified message to the chatroom.
        /// </summary>
        /// <param name="message">The message.</param>
        void Say(string message);
        /// <summary>
        /// Sends a message to the chat room with a specified delay.
        /// If at the time the specified avatar session is not available,
        /// the message will be not be echoed to the chat room.
        /// Great for Greeter bots to prevent greeting message flooding.
        /// </summary>
        /// <param name="delay">The delay.</param>
        /// <param name="avatar">The avatar.</param>
        /// <param name="message">The message.</param>
        void Say(int delay, IAvatar avatar, string message);
        void ChangeObjectAction(Model o);
        Avatar GetAvatar(int session);
        void AwExceptionHandler(Exception ex);
        void Dispose();
    }
}