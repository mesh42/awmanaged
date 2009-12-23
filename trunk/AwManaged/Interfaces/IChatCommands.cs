using AwManaged.SceneNodes;
using AwManaged.SceneNodes.Interfaces;
using AWManaged.Security;

namespace AwManaged.Interfaces
{
    public interface IChatCommands
    {
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
        /// <param name="sessionArgumentType">Type of the session argument.</param>
        /// <param name="avatar">The avatar.</param>
        /// <param name="message">The message.</param>
        void Say(int delay, SessionArgumentType sessionArgumentType, IAvatar avatar, string message);

    }
}
