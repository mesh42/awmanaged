using AW;
using AwManaged.Huds.Interfaces;
using AwManaged.Interfaces;
using AwManaged.Math;
using AwManaged.SceneNodes.Interfaces;

namespace AwManaged.Huds
{
    public abstract class HudBase : IHudBase
    {
        private readonly Instance _aw;

        /// <summary>
        /// Initializes a new instance of the <see cref="HudBase"/> class.
        /// </summary>
        /// <param name="aw">The aw.</param>
        /// <param name="engine">The engine.</param>
        /// <param name="id">The id.</param>
        /// <param name="content">The content.</param>
        /// <param name="type">The type.</param>
        /// <param name="origin">The origin.</param>
        /// <param name="opacity">The opacity.</param>
        /// <param name="position">The position.</param>
        /// <param name="flags">The flags.</param>
        /// <param name="size">The size.</param>
        /// <param name="color">The color.</param>
        protected HudBase(Instance aw, IBaseBotEngine engine, int id, string content, HudType type, HudOrigin origin, float opacity, Vector3 position, HudElementFlag flags, Vector3 size, int color)
        {
            this._aw = aw;
            Engine = engine;
            Id = id;
            Content = content;
            Type = type;
            Origin = origin;
            Opacity = opacity;
            Position = position;
            Flags = flags;
            Size = size;
            Color = color;
        }

        /// <summary>
        /// Displays the hud to the specified avatar.
        /// </summary>
        /// <param name="avatar">The avatar.</param>
        public void Display(IAvatar avatar)
        {
            _aw.SetInt(Attributes.HudElementType, (int)Type);
            _aw.SetInt(Attributes.HudElementId, Id);
            _aw.SetInt(Attributes.HudElementSession, avatar.Session);
            _aw.SetInt(Attributes.HudElementOrigin, (int)Origin);
            _aw.SetFloat(Attributes.HudElementOpacity, Opacity);
            _aw.SetInt(Attributes.HudElementX, (int)Position.x);
            _aw.SetInt(Attributes.HudElementY, (int)Position.y);
            _aw.SetInt(Attributes.HudElementZ, (int)Position.z);
            _aw.SetInt(Attributes.HudElementFlags, (int)Flags);
            _aw.SetInt(Attributes.HudElementColor, Color);
            _aw.SetInt(Attributes.HudElementSizeX, (int)Size.x);
            _aw.SetInt(Attributes.HudElementSizeY, (int)Size.y);
            _aw.SetInt(Attributes.HudElementSizeZ, (int)Size.z);
            _aw.HudCreate();
        }

        #region IEngineReference Members

        public IBaseBotEngine Engine
        {
            get; private set;
        }

        public int Id { get; set; }
        public string Content { get; set; }
        public HudType Type { get; set; }
        public HudOrigin Origin { get; set; }
        public float Opacity { get; set; }
        public Vector3 Position { get; set; }
        public HudElementFlag Flags { get; set; }
        public Vector3 Size { get; set; }
        public int Color { get; set; }

        #endregion
    }
}