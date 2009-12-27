using AW;
using AwManaged.Math;
using AwManaged.SceneNodes.Interfaces;

namespace AwManaged.SceneNodes
{
    public sealed class HudBase<TAvatar> : IHudBase<HudBase<TAvatar>,TAvatar> where TAvatar : IAvatar<TAvatar>
    {
        private readonly Instance _aw;

        /// <summary>
        /// Initializes a new instance of the <see cref="HudBase&lt;TAvatar&gt;"/> class.
        /// </summary>
        /// <param name="aw">The aw.</param>
        /// <param name="id">The id.</param>
        /// <param name="content">The content.</param>
        /// <param name="type">The type.</param>
        /// <param name="origin">The origin.</param>
        /// <param name="opacity">The opacity.</param>
        /// <param name="position">The position.</param>
        /// <param name="flags">The flags.</param>
        /// <param name="size">The size.</param>
        /// <param name="color">The color.</param>
        protected HudBase(Instance aw, int id, string content, HudType type, HudOrigin origin, float opacity, Vector3 position, HudElementFlag flags, Vector3 size, int color)
        {
            this._aw = aw;
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
        public void Display(TAvatar avatar)
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

        public int Id { get; set; }
        public string Content { get; set; }
        public HudType Type { get; set; }
        public HudOrigin Origin { get; set; }
        public float Opacity { get; set; }
        public Vector3 Position { get; set; }
        public HudElementFlag Flags { get; set; }
        public Vector3 Size { get; set; }
        public int Color { get; set; }

        #region ICloneableT<IHudBase> Members

        public HudBase<TAvatar> Clone()
        {
            return (HudBase<TAvatar>) MemberwiseClone();
        }
        #endregion

        #region IChanged<HudBase<TAvatar>> Members

        public event AwManaged.Core.Interfaces.ChangedEventDelegate<HudBase<TAvatar>> OnChanged;

        public bool IsChanged { get; internal set; }

        #endregion
    }
}