namespace AwManaged.Scene.ActionInterpreter
{
    /// <summary>
    /// The fx argument specifies one of several optional lighting "effects" that can be applied to the light source. All of the effects cause the brightness of the object to vary over time.
    /// </summary>
    public enum LightFxType
    {
        /// <summary>
        /// blink - light alternates equally between on and off
        /// </summary>
        Blink,
        /// <summary>
        /// fadein - light fades in from dark to full brightness
        /// </summary>
        FadeIn,
        /// <summary>
        /// fadeout - light fades out from full brightness to dark (after which it deletes itself from the object)
        /// </summary>
        FadeOut,
        /// <summary>
        /// light flickers randomly like a flame
        /// </summary>
        Fire,
        /// <summary>
        /// light switches off for a brief period at random intervals
        /// </summary>
        Flicker,
        /// <summary>
        /// light switches on for a brief period at random intervals
        /// </summary>
        Flash,
        /// <summary>
        /// light fades in and then back out at regular intervals
        /// </summary>
        Pulse
    }
}