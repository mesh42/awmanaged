namespace AwManaged.Scene.ActionInterpreter
{
    /// <summary>
    /// Matrial Effects.
    /// </summary>
    public enum MatFxType
    {
        /// <summary>
        /// 0 ... no material effect / switch off all material effects
        /// </summary>
        None = 0,
        /// <summary>
        /// 1 ... environment mapping using camera matrix as projection viewpoint
        /// </summary>
        EnvironmentMappingCamera = 1,
        /// <summary>
        /// 2 ... bump mapping using camera matrix as projection viewpoint
        /// </summary>
        BumpMappingCamera = 2,
        /// <summary>
        /// 3 ... environment and bump mapping using camera matrix as projection viewpoint
        /// </summary>
        EnvironmentBumpMappingCamera = 3,
        /// <summary>
        /// 4 ... dual texturing using camera matrix as projection viewpoint
        /// </summary>
        DualTexturingCamera = 4,
        /// <summary>
        /// 10 ... no material effect - same as 0
        /// </summary>
        NoMaterialEffect = 10,
        /// <summary>
        /// 11 ... environment mapping using the directional light matrix as projection viewpoint
        /// </summary>
        EnvironmentMappingLight = 11,
        /// <summary>
        /// 12 ... bump mapping using the directional light matrix as projection viewpoint
        /// </summary>
        BumpMappingLight = 12,
        /// <summary>
        /// 13 ... environment and bump mapping using the directional light matrix as projection viewpoint
        /// </summary>
        EnvironmentBumpMappingLight = 13,
        /// <summary>
        /// 14 ... dual texturing using the directional light matrix as projection viewpoint
        /// </summary>
        DualTexturingLight = 14
    }
}