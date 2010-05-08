/* **********************************************************************************
 *
 * Copyright (c) TCPX. All rights reserved.
 *
 * This source code is subject to terms and conditions of the Microsoft Public
 * License (Ms-PL). A copy of the license can be found in the license.txt file
 * included in this distribution.
 *
 * You must not remove this notice, or any other, from this software.
 *
 * **********************************************************************************/
using SharedMemory;using System;
using AwManaged.Core.Interfaces;
using AwManaged.Core.ServicesManaging.Interfaces;
using AwManaged.Scene.Interfaces;

namespace AwManaged.Interfaces
{
    public interface IEngineReference<TAvatar, TModel, TCamera, TMover, TZone, THudBase, TParticle, TParticleFlags,
                                      TConnectionInterface, TLocalBotServicesManager>
        where TModel : MarshalIndefinite, IModel<TModel>
        where TAvatar : MarshalIndefinite, IAvatar<TAvatar>
        where TCamera : MarshalIndefinite, ICamera<TCamera>
        where TMover : MarshalIndefinite, IMover<TMover>
        where TZone : MarshalIndefinite, IZone<TZone, TModel, TCamera>
        where THudBase : MarshalIndefinite, IHudBase<THudBase, TAvatar>
        where TParticle : MarshalIndefinite, IParticle<TParticle, TParticleFlags>
        where TParticleFlags : MarshalIndefinite, IParticleFlags<TParticleFlags>
        where TConnectionInterface : IConnection<TConnectionInterface>
        where TLocalBotServicesManager : IServicesManager
    {
        IBotEngine<TAvatar,TModel,TCamera,TZone,TMover,THudBase,TParticle,TParticleFlags, TConnectionInterface,TLocalBotServicesManager> Engine { get; }
    }
}