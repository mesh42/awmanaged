namespace AwManaged.Scene.ActionInterpreter
{
    /// <summary>
    /// 
    /// </summary>
    public enum MediaPlayType
    {
        /// <summary>
        /// Default.
        /// </summary>
        Play,
        /// <summary>
        /// Stop allows named objects can be used to stop running media.
        /// </summary>
        Stop,
        /// <summary>
        /// Pause is used to pause running media. Triggering a subsequent pause causes the media to continue playing where it was paused before.
        /// Note: Live-feeds cannot be paused. The streaming will continue, but the video rendering and the sound will be switched off / muted during the pause. Also note that the set or pause argument should be used before other parameters are defined.
        /// </summary>
        Pause
    }
}