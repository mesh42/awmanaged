using System;
using AwManaged.Core.Interfaces;
using AwManaged.Scene.Interfaces;

namespace AwManaged.Scene
{
    public class ParticleFlags : MarshalByRefObject, IParticleFlags<ParticleFlags> 
    {
        public bool CameraEmit { get; set; }
        public bool DrawInFront { get; set; }
        public bool Gravity { get; set; }
        public bool Interpolate { get; set; }
        public bool LinkToMover { get; set; }
        public bool ZoneCollision { get; set; }
        public bool ZoneExclusive { get; set;}
        
        public ParticleFlags Clone()
        {
            return (ParticleFlags) MemberwiseClone();
        }

        public event ChangedEventDelegate<ParticleFlags> OnChanged;
        public bool IsChanged
        {
            get { throw new NotImplementedException(); }
        }
    }
}
