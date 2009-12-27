using AwManaged.Core.Interfaces;
using AwManaged.SceneNodes.Interfaces;

namespace AwManaged.SceneNodes
{
    public sealed class Camera : ICamera<Camera>
    {
        #region ICloneableT<Camera> Members

        public Camera Clone()
        {
            return (Camera) MemberwiseClone();
        }

        #endregion

        #region ICamera<Camera> Members

        public AW.CameraFlags Flags { get; set;}
        public string Name { get;set;}
        public float Zoom {get;set;}

        #endregion


        #region IChanged<Camera> Members

        public event ChangedEventDelegate<Camera> OnChanged;

        public bool IsChanged { get; internal set; }

        #endregion
    }
}
