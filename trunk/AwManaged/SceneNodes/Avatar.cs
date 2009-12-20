using System;
using AwManaged.Interfaces;
using AwManaged.Math;
using AwManaged.SceneNodes.Interfaces;

namespace AwManaged.SceneNodes
{
    public class Avatar : IAvatar
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
            else
            {
                //
            }
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

        #region IAvatar Members


        public IPropertyBag<IAvatar> Properties{get;set;}

        #endregion
    }
}