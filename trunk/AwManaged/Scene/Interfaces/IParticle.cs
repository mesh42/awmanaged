using System;
using System.Drawing;
using AwManaged.Math;

namespace AwManaged.Scene.Interfaces
{
    public interface IParticle<TParticle, TPArticleFlags> : ISceneNode<TParticle> 
        where TParticle : MarshalByRefObject
        where TPArticleFlags : MarshalByRefObject, ISceneNode<TPArticleFlags>
    {
        Vector3 AccelerationMinimum { set; get; }
        Vector3 AccelerationMaximum { set; get; }
        Vector3 AngleMinimum { set; get; }
        Vector3 AngleMaximum { set; get; }
        string AssetList { set; get; }
        Color ColorEnd { set; get; }
        Color ColorStart { set; get; }
        uint EmitterLifespan { set; get; }
        uint FadeIn { set; get; }
        uint FadeOut { set; get; }
        IParticleFlags<TPArticleFlags> Flags { set; get; }
        uint Lifespan { set; get; }
        string Name { set; get; }
        float Opacity { set; get; }
        uint ReleaseMaximum { set; get; }
        uint ReleaseMinimum { set; get; }
        ushort ReleaseSize { set; get; }
        ParticleDrawStyle RenderStyle { set; get; }
        Vector3 SizeMinimum { set; get; }
        Vector3 SizeMaximum { set; get; }
        Vector3 SpeedMinimum { set; get; }
        Vector3 SpeedMaximum { set; get; }
        Vector3 SpinMinimum { set; get; }
        Vector3 SpinMaximum { set; get; }
        ParticleType Style { set; get; }
        Vector3 VolumeMinimum { set; get; }
        Vector3 VolumeMaximum { set; get; }
    }
}
