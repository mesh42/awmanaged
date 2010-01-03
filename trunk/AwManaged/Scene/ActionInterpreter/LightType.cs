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
namespace AwManaged.Scene.ActionInterpreter
{
    /// <summary>
    /// The type specifies the type of light source, which can be either "point" or "spot". "Point" light sources shine equally in all directions, and are the default if no type is specified. "Spot" light sources shine a "cone" of light in a particular direction.
    /// </summary>
    public enum LightType
    {
        /// <summary>
        /// "Point" light sources shine equally in all directions, and are the default if no type is specified.
        /// </summary>
        Point,
        /// <summary>
        /// "Spot" light sources shine a "cone" of light in a particular direction.
        /// </summary>
        Spot
    }
}