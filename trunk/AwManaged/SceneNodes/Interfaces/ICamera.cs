namespace AwManaged.SceneNodes.Interfaces
{
    public interface ICamera<T> : ISceneNode<T>
    {
        AW.CameraFlags Flags { set; get; }
        string Name { set; get; }
        float Zoom { set; get; }
    }
}
