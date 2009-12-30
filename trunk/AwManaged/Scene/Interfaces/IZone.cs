using System;
using AW;
using AwManaged.Math;
using AwManaged.Scene.Interfaces;
using Color=System.Drawing.Color;

namespace AwManaged.Scene.Interfaces
{
    /// <summary>
    /// Zone Scene Node
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TCamera">The type of the camera.</typeparam>
    public interface IZone<T,TModel,TCamera> : ISceneNode<T>
        where TModel : MarshalByRefObject, IModel<TModel>
        where TCamera : MarshalByRefObject, ICamera<TCamera>
        where T : MarshalByRefObject
    {
        /// <summary>
        /// Gets or sets the model object atttached to this zone.
        /// </summary>
        /// <value>The model.</value>
        TModel Model {get;set;}
        /// <summary>
        /// Gets or sets the ambient.
        /// </summary>
        /// <value>The ambient.</value>
        string Ambient { set; get; }
        /// <summary>
        /// Gets or sets the camera object attached to this zone.
        /// </summary>
        /// <value>The camera.</value>
        TCamera Camera { set; get; }
        /// <summary>
        /// Gets or sets the name of the camera.
        /// </summary>
        /// <value>The name of the camera.</value>
        string CameraName { get; set; }
        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        /// <value>The color.</value>
        Color Color { set; get; }
        /// <summary>
        /// Gets or sets the flags.
        /// </summary>
        /// <value>The flags.</value>
        ZoneFlags Flags { set; get; }
        /// <summary>
        /// Gets or sets the fog maximum.
        /// </summary>
        /// <value>The fog maximum.</value>
        ushort FogMaximum { set; get; }
        /// <summary>
        /// Gets or sets the fog minimum.
        /// </summary>
        /// <value>The fog minimum.</value>
        ushort FogMinimum { set; get; }
        /// <summary>
        /// Gets or sets the footstep.
        /// </summary>
        /// <value>The footstep.</value>
        string Footstep { set; get; }
        /// <summary>
        /// Gets or sets the friction.
        /// </summary>
        /// <value>The friction.</value>
        float Friction { set; get; }
        /// <summary>
        /// Gets or sets the gravity.
        /// </summary>
        /// <value>The gravity.</value>
        float Gravity { set; get; }
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        string Name { set; get; }
        /// <summary>
        /// Gets or sets the priority.
        /// </summary>
        /// <value>The priority.</value>
        byte Priority { set; get; }
        /// <summary>
        /// Gets or sets the shape.
        /// </summary>
        /// <value>The shape.</value>
        ZoneType Shape { set; get; }
        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        /// <value>The size.</value>
        Vector3 Size { set; get; }
        /// <summary>
        /// Gets or sets the target cursor.
        /// </summary>
        /// <value>The target cursor.</value>
        string TargetCursor { set; get; }
        /// <summary>
        /// Gets or sets the voip rights.
        /// </summary>
        /// <value>The voip rights.</value>
        string VoipRights { set; get; }
    }
}