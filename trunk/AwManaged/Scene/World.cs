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
using AwManaged.Scene.Interfaces;

namespace AwManaged.Scene
{
    public sealed class World : MarshalIndefinite, IWorld<World>
    {
        private readonly Guid id;

        public World(string name)
        {
            Name = name;
        }

        public World(Guid id)
        {
            this.id = id;
        }

        public string Name { get; set; }

        public Guid Id
        {
            get { return id; }
        }

        #region ICloneableT<IWorld> Members

        public World Clone()
        {
            return (World) MemberwiseClone();
        }

        #endregion

        #region IChanged<World> Members

        public event ChangedEventDelegate<World> OnChanged;

        public bool IsChanged { get; internal set; }

        #endregion
    }
}