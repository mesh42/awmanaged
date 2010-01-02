using System;
using AwManaged.Scene.Interfaces;

namespace AwManaged.Interfaces
{
    public interface IEngineReference<TAvatar,TModel,TCamera,TMover,TZone,THudBase,TParticle,TParticleFlags>
        where TModel : MarshalByRefObject, IModel<TModel>
        where TAvatar : MarshalByRefObject, IAvatar<TAvatar>
        where TCamera : MarshalByRefObject, ICamera<TCamera>
        where TMover : MarshalByRefObject, IMover<TMover>
        where TZone : MarshalByRefObject, IZone<TZone, TModel, TCamera>
        where THudBase : MarshalByRefObject, IHudBase<THudBase, TAvatar>
        where TParticle : MarshalByRefObject, IParticle<TParticle, TParticleFlags>
        where TParticleFlags : MarshalByRefObject, IParticleFlags<TParticleFlags>
    {
        IBotEngine<TAvatar,TModel,TCamera,TZone,TMover,THudBase,TParticle,TParticleFlags> Engine { get; }
    }
}