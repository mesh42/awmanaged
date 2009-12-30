using System;
using AwManaged.Core.Interfaces;
using AwManaged.Scene.Interfaces;

namespace AwManaged.Scene
{
    public sealed class Camera : MarshalByRefObject, ICamera<Camera>
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