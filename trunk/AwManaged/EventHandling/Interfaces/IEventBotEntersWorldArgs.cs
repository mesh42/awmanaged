﻿using System;
using AwManaged.Configuration.Interfaces;

namespace AwManaged.EventHandling.Interfaces
{
    public  interface IEventBotEntersWorldArgs<TConnectionProperties>
        where TConnectionProperties : MarshalByRefObject, IUniverseConnectionProperties<TConnectionProperties>
    {
        TConnectionProperties ConnectionProperties { get; }
    }
}