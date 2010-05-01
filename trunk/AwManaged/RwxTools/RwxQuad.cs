namespace AwManaged.RwxTools
{
    using Math;

    public class RwxQuad
    {
        private readonly Vector4 _quad;

        public RwxQuad(Vector4 quad)
        {
            _quad = quad;
        }

        public Vector4 Quad
        {
            get { return _quad; }
        }
    }
}
