using AwManaged.SceneNodes.Interfaces;

namespace AwManaged.Interfaces
{
    public interface IEngineReference<TAvatar,TModel,TCamera,TMover,TZone,THudBase>
        where TModel : IModel<TModel>
        where TAvatar : IAvatar<TAvatar>
        where TCamera : ICamera<TCamera>
        where TMover : IMover<TMover>
        where TZone : IZone<TZone, TModel, TCamera>
        where THudBase : IHudBase<THudBase, TAvatar>

    {
        IBotEngine<TAvatar,TModel,TCamera,TZone,TMover,THudBase> Engine { get; }
    }
}