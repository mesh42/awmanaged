using System;
using System.Text.RegularExpressions;
using AwManaged.Math;

namespace AwManaged.Converters
{
    /// <summary>
    /// Serveral converters for AW types.
    /// </summary>
    internal class AwConvert
    {
        /// <summary>
        /// Converts an Active Worlds string based coordinate to vectors, afterwich the coordinates 
        /// can be used immediately for math calculations using vector algebra if so desired.
        /// 
        /// For example, an input coordinate could be: 5.73S 1.83E 0.1a 221
        /// </summary>
        /// <param name="coordinates">The coordinates.</param>
        /// <returns></returns>
        internal static CoordinateVectors CoordinatesToVector3(string coordinates)
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

        internal static DateTime ConvertFromUnixTimestamp(double timestamp)
        {
            var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(timestamp);
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
    }
}