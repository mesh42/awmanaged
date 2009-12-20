using AW;
using AwManaged.SceneNodes;

namespace AwManaged.SceneNodes
{
    public class ZoneObject
    {
        private readonly Zone zone;
        private readonly Model model;

        public ZoneObject(Zone zone, Model model)
        {
            this.zone = zone;
            this.model = model;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ZoneObject"/> class.
        /// Use for serialization only.
        /// </summary>
        public ZoneObject()
        {
            
        }

        /// <summary>
        /// Gets the model.
        /// </summary>
        /// <value>The model.</value>
        public Model Model
        {
            get { return model; }
        }

        /// <summary>
        /// Gets the zone.
        /// </summary>
        /// <value>The zone.</value>
        public Zone Zone
        {
            get { return zone; }
        }
    }
}