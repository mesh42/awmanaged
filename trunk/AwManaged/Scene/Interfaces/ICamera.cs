using System;

namespace AwManaged.Scene.Interfaces
{
    public interface ICamera<T> : ISceneNode<T>
        where T : MarshalByRefObject
    {
        AW.CameraFlags Flags { set; get; }
        string Name { set; get; }
        float Zoom { set; get; }
    }
}