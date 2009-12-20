using AW;
using AwManaged.Interfaces;
using AwManaged.Math;
using AwManaged.SceneNodes.Interfaces;

namespace AwManaged.Huds.Interfaces
{
    /// <summary>
    /// Hud Base Interface
    /// </summary>
    public interface IHudBase : IEngineReference
    {
        /// <summary>
        /// Displays the hud to the specified avatar.
        /// </summary>
        /// <param name="avatar">The avatar.</param>
        void Display(IAvatar avatar);
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        int Id { get; set; }
        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>The content.</value>
        string Content { get; set; }
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        HudType Type { get; set; }
        /// <summary>
        /// Gets or sets the origin.
        /// </summary>
        /// <value>The origin.</value>
        HudOrigin Origin { get; set; }
        /// <summary>
        /// Gets or sets the opacity.
        /// </summary>
        /// <value>The opacity.</value>
        float Opacity { get; set; }
        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>The position.</value>
        Vector3 Position { get; set; }
        /// <summary>
        /// Gets or sets the flags.
        /// </summary>
        /// <value>The flags.</value>
        HudElementFlag Flags { get; set; }
        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        /// <value>The size.</value>
        Vector3 Size { get; set; }
        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        /// <value>The color.</value>
        int Color { get; set; }
    }
}