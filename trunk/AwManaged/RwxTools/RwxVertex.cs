namespace AwManaged.RwxTools
{
    using Math;

    public class RwxVertex
    {
        private readonly Vector3 _position;
        private readonly Vector2 _uv;

        public RwxVertex(Vector3 position, Vector2 uv)
        {
            _position = position;
            _uv = uv;
        }

        public Vector2 Uv
        {
            get { return _uv; }
        }

        public Vector3 Position
        {
            get { return _position; }
        }
    }
}
