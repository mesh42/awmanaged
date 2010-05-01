namespace AwManaged.RwxTools
{
    using Math;

    public class RwxTriangle
    {
        private readonly Vector3 _triangle;

        public RwxTriangle(Vector3 triangle)
        {
            _triangle = triangle;
        }

        public Vector3 Vector
        {
            get { return _triangle; }
        }
    }
}
