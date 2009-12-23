using AwManaged.Math;
using AwManaged.SceneNodes.Interfaces;

namespace AwManaged.Interfaces
{
    /// <summary>
    /// Avatar commands.
    /// </summary>
    public interface IAvatarCommands
    {
        IAvatar GetAvatar(int session);
        /// <summary>
        /// Sets the avatar's position and yaw.
        /// </summary>
        /// <param name="avatar">The avatar.</param>
        /// <param name="position">The position.</param>
        /// <param name="yaw">The yaw.</param>
        void SetPosition(IAvatar avatar, Vector3 position, float yaw);
        /// <summary>
        /// Sets the avatar's position.
        /// </summary>
        /// <param name="avatar">The avatar.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="z">The z.</param>
        /// <param name="yaw">The yaw.</param>
        void SetPosition(IAvatar avatar, float x, float y, float z, float yaw);
    }
}
