using System.Drawing;
using AwManaged.SceneNodes.Interfaces;

namespace AwManaged.SceneNodes
{
    public sealed class Zone : IZone<Zone,Model,Camera>
    {
        #region IZone Members

        public IModel<Model> Model { get; set; }
        public string Ambient {get; set;}
        public ICamera<Camera> Camera { get; set;}
        public string CameraName { get; set; }
        public Color Color { get; set;}
        public AW.ZoneFlags Flags { get; set;}
        public ushort FogMaximum { get; set;}
        public ushort FogMinimum{ get; set;}
        public string Footstep{ get; set;}
        public float Friction{ get; set;}
        public float Gravity{ get; set;}
        public string Name{ get; set;}
        public byte Priority{ get; set;}
        public AW.ZoneType Shape{ get; set;}

        public AwManaged.Math.Vector3 Size{ get; set;}

        public string TargetCursor{ get; set;}

        public string VoipRights { get; set; }

        #endregion

        #region ICloneableT<IZone> Members

        public Zone Clone()
        {
            return (Zone) MemberwiseClone();
        }

        #endregion

        #region IChanged<Zone> Members

        public event AwManaged.Core.Interfaces.ChangedEventDelegate<Zone> OnChanged;

        public bool IsChanged { get; internal set;}

        #endregion
    }
}
