using AwManaged.Math;
using AwManaged.SceneNodes.Interfaces;

namespace AwManaged.SceneNodes.Interfaces
{
    /// <summary>
    /// Avatar commands.
    /// </summary>
    /// <typeparam name="TAvatar">The type of the avatar, which needs to implement IAvatar</typeparam>
    public interface IAvatarCommands<TAvatar> where TAvatar : IAvatar<TAvatar>
    {
        /// <summary>
        /// Sets the avatar's position and yaw.
        /// </summary>
        /// <param name="avatar">The avatar.</param>
        /// <param name="position">The position.</param>
        /// <param name="yaw">The yaw.</param>
        void Teleport(TAvatar avatar, Vector3 position, float yaw);
        /// <summary>
        /// Sets the avatar's position.
        /// </summary>
        /// <param name="avatar">The avatar.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="z">The z.</param>
        /// <param name="yaw">The yaw.</param>
        void Teleport(TAvatar avatar, float x, float y, float z, float yaw);
        /// <summary>
        /// Gets the ciziten name by number. This routine is very slow
        /// The Universe server does niet allow for rapid queries. Ther should be about 3 seconds between
        /// each query. Probably some hardcoded time interval by Active Worlds, to prevent
        /// high server load.
        /// </summary>
        /// <param name="citizen">The citizen.</param>
        /// <returns></returns>
        string GetCizitenNameByNumber(int citizen);
    }
}