namespace AwManaged.RwxTools
{
    using Math;

    public class RwxPolygon
    {
        private readonly Polygon _polygon;

        public RwxPolygon(Polygon polygon)
        {
            _polygon = polygon;
        }

        public Polygon Polygon
        {
            get { return _polygon; }
        }
    }
}
