using SharedMemory;using System;
using System.Collections.Generic;
using System.Text;

namespace AwManaged.Math
{
    /// <summary>
    /// Defines a plane in 3D space.
    /// </summary>
    /// <remarks>
    /// A plane is defined in 3D space by the equation
    /// Ax + By + Cz + D = 0
    ///
    /// This equates to a vector (the normal of the plane, whose x, y
    /// and z components equate to the coefficients A, B and C
    /// respectively), and a constant (D) which is the distance along
    /// the normal you have to go to move the plane back to the origin.
    /// </remarks>
    public struct Plane
    {
        #region Fields

        /// <summary>
        ///		Direction the plane is facing.
        /// </summary>
        public Vector3 Normal;
        /// <summary>
        ///		Distance from the origin.
        /// </summary>
        public float D;

        private static readonly Plane nullPlane = new Plane(Vector3.Zero, 0);
        public static Plane Null { get { return nullPlane; } }

        #endregion Fields

        #region Constructors

        public Plane(Plane plane)
        {
            this.Normal = plane.Normal;
            this.D = plane.D;
        }

        /// <summary>
        ///		Construct a plane through a normal, and a distance to move the plane along the normal.
        /// </summary>
        /// <param name="normal"></param>
        /// <param name="constant"></param>
        public Plane(Vector3 normal, float constant)
        {
            this.Normal = normal;
            this.D = -constant;
        }

        public Plane(Vector3 normal, Vector3 point)
        {
            this.Normal = normal;
            this.D = -normal.Dot(point);
        }

        /// <summary>
        ///		Construct a plane from 3 coplanar points.
        /// </summary>
        /// <param name="point0">First point.</param>
        /// <param name="point1">Second point.</param>
        /// <param name="point2">Third point.</param>
        public Plane(Vector3 point0, Vector3 point1, Vector3 point2)
        {
            Vector3 edge1 = point1 - point0;
            Vector3 edge2 = point2 - point0;
            Normal = edge1.Cross(edge2);
            Normal.Normalize();
            D = -Normal.Dot(point0);
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public AwManaged.Math.PlaneSide GetSide(Vector3 point)
        {
            float distance = GetDistance(point);

            if (distance < 0.0f)
                return AwManaged.Math.PlaneSide.Negative;

            if (distance > 0.0f)
                return AwManaged.Math.PlaneSide.Positive;

            return AwManaged.Math.PlaneSide.None;
        }

        /// <summary>
        /// This is a pseudodistance. The sign of the return value is
        /// positive if the point is on the positive side of the plane,
        /// negative if the point is on the negative side, and zero if the
        ///	 point is on the plane.
        /// The absolute value of the return value is the true distance only
        /// when the plane normal is a unit length vector.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public float GetDistance(Vector3 point)
        {
            return Normal.Dot(point) + D;
        }

        /// <summary>
        ///		Construct a plane from 3 coplanar points.
        /// </summary>
        /// <param name="point0">First point.</param>
        /// <param name="point1">Second point.</param>
        /// <param name="point2">Third point.</param>
        public void Redefine(Vector3 point0, Vector3 point1, Vector3 point2)
        {
            Vector3 edge1 = point1 - point0;
            Vector3 edge2 = point2 - point0;
            Normal = edge1.Cross(edge2);
            Normal.Normalize();
            D = -Normal.Dot(point0);
        }

        #endregion Methods

        #region Model overrides

        /// <summary>
        ///		Model method for testing equality.
        /// </summary>
        /// <param name="obj">Model to test.</param>
        /// <returns>True if the 2 planes are logically equal, false otherwise.</returns>
        public override bool Equals(object obj)
        {
            return obj is Plane && this == (Plane)obj;
        }

        /// <summary>
        ///		Gets the hashcode for this Plane.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return D.GetHashCode() ^ Normal.GetHashCode();
        }

        /// <summary>
        ///		Returns a string representation of this Plane.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("Distance: {0} Normal: {1}", D, Normal.ToString());
        }

        #endregion

        #region Operator Overloads

        /// <summary>
        ///		Compares 2 Planes for equality.
        /// </summary>
        /// <param name="left">First plane.</param>
        /// <param name="right">Second plane.</param>
        /// <returns>true if equal, false if not equal.</returns>
        public static bool operator ==(Plane left, Plane right)
        {
            return (left.D == right.D) && (left.Normal == right.Normal);
        }

        /// <summary>
        ///		Compares 2 Planes for inequality.
        /// </summary>
        /// <param name="left">First plane.</param>
        /// <param name="right">Second plane.</param>
        /// <returns>true if not equal, false if equal.</returns>
        public static bool operator !=(Plane left, Plane right)
        {
            return (left.D != right.D) || (left.Normal != right.Normal);
        }

        #endregion
    }
}