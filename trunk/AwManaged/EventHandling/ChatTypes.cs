namespace AwManaged.EventHandling
{
    /// <summary>
    /// Enumerates the different chat types that are available in the aw managed library.
    /// </summary>
    public enum ChatType
    {
        /// <summary>
        /// A normal chat message from a certain avatar.
        /// </summary>
        Normal = 0,
        /// <summary>
        /// A chat message received from a public speaker.
        /// </summary>
        Broadcast = 1,
        /// <summary>
        /// A chat message sent only to you a certain avatar.
        /// </summary>
        Whisper = 2
    }
}
