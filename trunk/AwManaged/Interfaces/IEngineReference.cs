using System;
using AwManaged.Scene.Interfaces;

namespace AwManaged.Interfaces
{
    public interface IEngineReference<TAvatar,TModel,TCamera,TMover,TZone,THudBase>
        where TModel : MarshalByRefObject, IModel<TModel>
        where TAvatar : MarshalByRefObject, IAvatar<TAvatar>
        where TCamera : MarshalByRefObject, ICamera<TCamera>
        where TMover : MarshalByRefObject, IMover<TMover>
        where TZone : MarshalByRefObject, IZone<TZone, TModel, TCamera>
        where THudBase : MarshalByRefObject, IHudBase<THudBase, TAvatar>

    {
        IBotEngine<TAvatar,TModel,TCamera,TZone,TMover,THudBase> Engine { get; }
    }
}