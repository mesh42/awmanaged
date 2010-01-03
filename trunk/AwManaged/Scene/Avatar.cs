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
using System;
using AwManaged.Math;
using AwManaged.Scene.Interfaces;

namespace AwManaged.Scene
{
    public sealed class Avatar : MarshalByRefObject, IAvatar<Avatar>
    {
        public int Session { get; set; }
        public string Name { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public int Gesture { get; set; }
        public int Citizen { get; set; }
        public int Privilege { get; set; }
        public int State { get; set; }

        public delegate void OnChangePositionDelegate(object sender, EventArgs args);

        public event OnChangePositionDelegate OnChangePosition;

        public Avatar()
        {
        }

        public void ChangedPosition()
        {
            if (OnChangePosition != null)
                OnChangePosition(this, null);
        }

        public Avatar(int session, string name, Vector3 position, Vector3 rotation, int gesture, int citizen, int privilege, int state)
        {
            Session = session;
            Name = name;
            Position = position;
            Rotation = rotation;
            Gesture = gesture;
            Citizen = citizen;
            Privilege = privilege;
            State = state;
        }

        public Avatar Clone()
        {
            return (Avatar)MemberwiseClone();
        }

        #region IChanged<Avatar> Members

        public event AwManaged.Core.Interfaces.ChangedEventDelegate<Avatar> OnChanged;

        public bool IsChanged{get; internal set;}

        #endregion
    }
}