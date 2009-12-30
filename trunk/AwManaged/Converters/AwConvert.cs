using System;
using System.Text.RegularExpressions;
using AW;
using AwManaged.ExceptionHandling;
using AwManaged.Math;
using AwManaged.Scene;
using Mover=AwManaged.Scene.Mover;
using Zone=AwManaged.Scene.Zone;

namespace AwManaged.Converters
{
    /// <summary>
    /// Serveral converters for AW types.
    /// </summary>
    public sealed class AwConvert
    {
        #region Coordinate Castings (Teleport)

        /// <summary>
        /// Converts an Active Worlds string based coordinate to vectors, afterwich the coordinates 
        /// can be used immediately for math calculations using vector algebra if so desired.
        /// 
        /// For example, an input coordinate could be: 5.73S 1.83E 0.1a 221
        /// </summary>
        /// <param name="coordinates">The coordinates.</param>
        /// <returns></returns>
        public static CoordinateVectors CoordinatesToVector3(string coordinates)
        {
            var match =
                new Regex(
                    "^(?<ns>[0-9,.]{1,})(?<nsi>[N,S]{1}) (?<ew>[0-9,.]{1,})(?<ewi>[E,W]{1}) (?<a>[0-9,.,-]{1,})(?<ai>[a]{1}) (?<r>[0-9]{3})",
                    RegexOptions.IgnoreCase).Match(coordinates);
            if (match.Value == string.Empty)
                throw new Exception(string.Format("Given coordinates '{0}' are not in the correct format.",coordinates));

            return new CoordinateVectors(
                new Vector3(
                    float.Parse(match.Groups["ew"].Value)*(match.Groups["ewi"].Value == "E" ? -1000 : 1000),
                    float.Parse(match.Groups["a"].Value)*100,
                    float.Parse(match.Groups["ns"].Value)*(match.Groups["nsi"].Value == "N" ? -1000 : 1000)
                    ),
                new Vector3(0, float.Parse(match.Groups["r"].Value) * 10, 0)
                );
        }

        /// <summary>
        /// Converts vector and yaw to a coordinate string.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="yaw">The yaw.</param>
        /// <returns></returns>
        public static string Vector3ToCoordinates(Vector3 position, float yaw)
        {
            var z = System.Math.Abs(position.z/1000/100).ToString();
            var x = System.Math.Abs(position.x/1000/100).ToString();
            var zi = position.z < 0 ? "N" : "S";
            var xi = position.x > 0 ? "E" : "W";
            var a = position.y/100;
            return string.Format("{0}{1} {2}{3} {4}a {5}", new[] {z,zi,x,xi,a.ToString(),System.Math.Round(yaw/1000).ToString()});
        }

        /// <summary>
        /// This structure encapsulates 2 3D vector classes, one for indicating
        /// a position of an object and the other for indicating rotation of the object.
        /// </summary>
        public struct CoordinateVectors
        {
            public Vector3 Position;
            public Vector3 Rotation;

            public CoordinateVectors(Vector3 position, Vector3 rotation)
            {
                Position = position;
                Rotation = rotation;
            }
        }

        #endregion

        #region Date Castings

        internal static DateTime ConvertFromUnixTimestamp(double timestamp)
        {
            var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(timestamp);
        }

        #endregion

        #region Scene Node Castings

        /// <summary>
        /// Casts the avatar object.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <returns></returns>
        internal static Avatar CastAvatarObject(Instance sender)
        {
            lock (sender)
            {
                try
                {
                    var position = new Vector3
                    {
                        x = sender.GetInt(Attributes.AvatarX),
                        y = sender.GetInt(Attributes.AvatarY),
                        z = sender.GetInt(Attributes.AvatarZ)
                    };
                    var rotation = new Vector3
                    {
                        x = sender.GetInt(Attributes.AvatarPitch),
                        y = sender.GetInt(Attributes.AvatarYaw),
                        z = 0
                    };
                    var a = new Avatar(
                        sender.GetInt(Attributes.AvatarSession),
                        sender.GetString(Attributes.AvatarName),
                        position,
                        rotation,
                        sender.GetInt(Attributes.AvatarGesture),
                        sender.GetInt(Attributes.AvatarCitizen),
                        sender.GetInt(Attributes.AvatarPrivilege),
                        sender.GetInt(Attributes.AvatarState)
                        );
                    return a;
                }
                catch (InstanceException ex)
                {
                    throw new AwException(ex.ErrorCode);
                }
            }
        }

        /// <summary>
        /// Casts the model object.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <returns></returns>
        internal static Model CastModelObject(Instance sender)
        {
            lock (sender)
            {
                try
                {
                    var position = new Vector3
                                       {
                                           x = sender.GetInt(Attributes.ObjectX),
                                           y = sender.GetInt(Attributes.ObjectY),
                                           z = sender.GetInt(Attributes.ObjectZ)
                                       };

                    var rotation = new Vector3
                                       {
                                           x = sender.GetInt(Attributes.ObjectTilt),
                                           y = sender.GetInt(Attributes.ObjectYaw),
                                           z = sender.GetInt(Attributes.ObjectRoll)
                                       };

                    var o = new Model(sender.GetInt(Attributes.ObjectId),
                                      sender.GetInt(Attributes.ObjectOwner),
                                      AwConvert.ConvertFromUnixTimestamp(sender.GetInt(Attributes.ObjectBuildTimestamp)),
                                      (ObjectType) sender.GetInt(Attributes.ObjectType),
                                      sender.GetString(Attributes.ObjectModel),
                                      position, rotation, sender.GetString(Attributes.ObjectDescription),
                                      sender.GetString(Attributes.ObjectAction), sender.GetInt(Attributes.ObjectNumber),
                                      sender.GetString(Attributes.ObjectData));

                    return o;
                }
                catch (InstanceException ex)
                {
                    throw new AwException(ex.ErrorCode);
                }
            }
        }
        /// <summary>
        /// Casts the mover object.
        /// </summary>
        /// <param name="awMover">The aw mover.</param>
        /// <returns></returns>
        internal static Mover CastMoverObject(AW.Mover awMover)
        {
            return new Mover()
            {
                AccelerationTiltX = awMover.AccelerationTiltX,
                AccelerationTiltZ = awMover.AccelerationTiltZ,
                AvatarTag = awMover.AvatarTag,
                BumpName = awMover.BumpName,
                Flags = awMover.Flags,
                FrictionFactor = awMover.FrictionFactor,
                GlideFactor = awMover.GlideFactor,
                LockedPitch = awMover.LockedPitch,
                LockedPosition = new Vector3(awMover.LockedPositionX, awMover.LockedPositionY, awMover.LockedPositionZ),
                LockedYaw = awMover.LockedYaw,
                Name = awMover.Name,
                Script = awMover.Script,
                Sequence = awMover.Sequence,
                Sound = awMover.Sound,
                SpeedFactor = awMover.SpeedFactor,
                TurnFactor = awMover.TurnFactor,
                Type = awMover.Type,
                Waypoints = awMover.Waypoints
            };
        }

        /// <summary>
        /// Casts the zone object.
        /// </summary>
        /// <param name="awZone">The aw zone.</param>
        /// <returns></returns>
        internal static Zone CastZoneObject(AW.Zone awZone)
        {
            return new Zone()
            {
                Ambient = awZone.Ambient,
                CameraName = awZone.Camera,
                Color = AwConvert.CastColor(awZone.Color),
                Flags = awZone.Flags,
                FogMaximum = awZone.FogMaximum,
                FogMinimum = awZone.FogMinimum,
                Footstep = awZone.Footstep,
                Friction = awZone.Friction,
                Gravity = awZone.Gravity,
                Name = awZone.Name,
                Priority = awZone.Priority,
                Shape = awZone.Shape,
                Size = new Vector3(awZone.Size.XMagnitude, awZone.Size.YMagnitude, awZone.Size.ZMagnitude),
                VoipRights = awZone.VoipRights
            };
        }

        #endregion

        #region Visual Graphics Castings

        /// <summary>
        /// Casts the color.
        /// </summary>
        /// <param name="awColor">Color of the aw.</param>
        /// <returns></returns>
        internal static System.Drawing.Color CastColor(AW.Color awColor)
        {
            return System.Drawing.Color.FromArgb(awColor.R, awColor.G, awColor.B);
        }

        #endregion
    }
}