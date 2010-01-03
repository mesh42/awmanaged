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
    /// Texture Blending
    /// </summary>
    public enum TextureBlendType
    {
        /// <summary>
        /// 0 ... no blending / switches the effect off
        /// </summary>
        None = 0,
        /// <summary>
        /// 1 ... blend zero - no blending on either source or destination
        /// </summary>
        Zero = 1,
        /// <summary>
        /// 2 ... blend one - full range of either source or destination
        /// </summary>
        One = 2,
        /// <summary>
        /// 3 ... blend source color - use the source color
        /// </summary>
        SourceColor = 3,
        /// <summary>
        /// 4 ... blend source inverse color - use the negative color of the source
        /// </summary>
        SourceInverseColor = 4,
        /// <summary>
        /// 5 ... blend source alpha - use the source as alpha channel
        /// </summary>
        SourceAlpha = 5,
        /// <summary>
        /// 6 ... blend source inverse alpha - use the negative of the source as alpha channel
        /// </summary>
        SourceInverseAlpha = 6,
        /// <summary>
        /// 7 ... blend destination alpha - use the negative of the destination as alpha channel
        /// </summary>
        DestinationAlpha = 7,
        /// <summary>
        /// 8 ... blend destination inverse alpha - use the negative of the destination as alpha channel
        /// </summary>
        DesinationInverseAlpha = 8,
        /// <summary>
        /// 9 ... blend destination color - use the color of the destination
        /// </summary>
        DestinationColor = 9,
        /// <summary>
        /// 10 ... blend destination inverse color - use the negative color of the destination
        /// </summary>
        DestinationInverseColor=10,
        /// <summary>
        /// 11 ... blend source alpha saturation - use the saturation of the source as alpha channel
        /// </summary>
        SourceAlphaSaturation = 11
    }
}