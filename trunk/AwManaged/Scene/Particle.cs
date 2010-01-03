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
using System.Drawing;
using AwManaged.Math;
using AwManaged.Scene.Interfaces;
using Db4objects.Db4o.Config.Attributes;

namespace AwManaged.Scene
{
    public class Particle : MarshalByRefObject, IParticle<Particle, ParticleFlags>
    {
        [Indexed]private Vector3 _accelerationMinimum;
        [Indexed]private Vector3 _accelerationMaximum;
        [Indexed]private Vector3 _angleMinimum;
        [Indexed]private Vector3 _angleMaximum;
        [Indexed]private string _assetList;
        [Indexed]private Color _colorEnd;
        [Indexed]private Color _colorStart;
        [Indexed]private uint _emitterLifespan;
        [Indexed]private uint _fadeIn;
        [Indexed]private uint _fadeOut;
        [Indexed]private IParticleFlags<ParticleFlags> _flags;
        [Indexed]private uint _lifespan;
        [Indexed]private string _name;
        [Indexed]private float _opacity;
        [Indexed]private uint _releaseMaximum;
        [Indexed]private uint _releaseMinimum;
        [Indexed]private ushort _releaseSize;
        [Indexed]private ParticleDrawStyle _renderStyle;
        [Indexed]private Vector3 _sizeMinimum;
        [Indexed]private Vector3 _sizeMaximum;
        [Indexed]private Vector3 _speedMinimum;
        [Indexed]private Vector3 _speedMaximum;
        [Indexed]private Vector3 _spinMinimum;
        [Indexed]private Vector3 _spinMaximum;
        [Indexed]private ParticleType _style;
        [Indexed]private Vector3 _volumeMinimum;
        [Indexed]private Vector3 _volumeMaximum;

        #region IParticle<Particle,ParticleFlags> Members

        public Vector3 AccelerationMinimum
        {
            get
            {
                return _accelerationMinimum;
            }
            set
            {
                _accelerationMinimum = value;
            }
        }
        public Vector3 AccelerationMaximum
        {
            get
            {
                return _accelerationMaximum;
            }
            set
            {
                _accelerationMaximum = value;
            }
        }
        public Vector3 AngleMinimum
        {
            get
            {
                return _angleMinimum;
            }
            set
            {
                _angleMinimum = value;
            }
        }
        public Vector3 AngleMaximum
        {
            get
            {
                return _angleMaximum;
            }
            set
            {
                _angleMaximum = value;
            }
        }
        public string AssetList
        {
            get
            {
                return _assetList;
            }
            set
            {
                _assetList = value;
            }
        }
        public Color ColorEnd
        {
            get
            {
                return _colorEnd;
            }
            set
            {
                _colorEnd = value;
            }
        }
        public Color ColorStart
        {
            get
            {
                return _colorStart;
            }
            set
            {
                _colorStart = value;
            }
        }
        public uint EmitterLifespan
        {
            get
            {
                return _emitterLifespan;
            }
            set
            {
                _emitterLifespan = value;
            }
        }
        public uint FadeIn
        {
            get
            {
                return _fadeIn;
            }
            set
            {
                _fadeIn = value;
            }
        }
        public uint FadeOut
        {
            get
            {
                return _fadeOut;
            }
            set
            {
                _fadeOut = value;
            }
        }
        public IParticleFlags<ParticleFlags> Flags
        {
            get
            {
                return _flags;
            }
            set
            {
                _flags = value;
            }
        }
        public uint Lifespan
        {
            get
            {
                return _lifespan;
            }
            set
            {
                _lifespan = value;
            }
        }
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }
        public float Opacity
        {
            get
            {
                return _opacity;
            }
            set
            {
                _opacity = value;
            }
        }
        public uint ReleaseMaximum
        {
            get
            {
                return _releaseMaximum;
            }
            set
            {
                _releaseMaximum = value;
            }
        }
        public uint ReleaseMinimum
        {
            get
            {
                return _releaseMinimum;
            }
            set
            {
                _releaseMinimum = value;
            }
        }
        public ushort ReleaseSize
        {
            get
            {
                return _releaseSize;
            }
            set
            {
                _releaseSize = value;
            }
        }
        public ParticleDrawStyle RenderStyle
        {
            get
            {
                return _renderStyle;
            }
            set
            {
                _renderStyle = value;
            }
        }
        public Vector3 SizeMinimum
        {
            get
            {
                return _sizeMinimum;
            }
            set
            {
                _sizeMinimum = value;
            }
        }
        public Vector3 SizeMaximum
        {
            get
            {
                return _sizeMaximum;
            }
            set
            {
                _sizeMaximum = value;
            }
        }
        public Vector3 SpeedMinimum
        {
            get
            {
                return _speedMinimum;
            }
            set
            {
                _speedMinimum = value;
            }

        }
        public Vector3 SpeedMaximum
        {
            get
            {
                return _speedMaximum;
            }
            set
            {
                _speedMaximum = value;
            }
        }
        public Vector3 SpinMinimum
        {
            get
            {
                return _spinMinimum;
            }
            set
            {
                _spinMinimum = value;
            }
        }
        public Vector3 SpinMaximum
        {
            get
            {
                return _spinMaximum;
            }
            set
            {
                _spinMaximum = value;
            }

        }
        public ParticleType Style
        {
            get
            {
                return _style;
            }
            set
            {
                _style = value;
            }
        }
        public Vector3 VolumeMinimum
        {
            get
            {
                return _volumeMinimum;
            }
            set
            {
                _volumeMinimum = value;
            }
        }
        public Vector3 VolumeMaximum
        {
            get
            {
                return _volumeMaximum;
            }
            set
            {
                _volumeMaximum = value;
            }
        }

        #endregion

        #region ICloneableT<Particle> Members

        public Particle Clone()
        {
            return (Particle) MemberwiseClone();
        }

        #endregion

        #region IChanged<Particle> Members

        public event AwManaged.Core.Interfaces.ChangedEventDelegate<Particle> OnChanged;

        public bool IsChanged
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}
