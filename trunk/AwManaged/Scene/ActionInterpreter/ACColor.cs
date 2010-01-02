using System.Drawing;

namespace AwManaged.Scene.ActionInterpreter
{
    /// <summary>
    /// The color command assigns a new color to every polygon on an object.
    /// </summary>
    class ACColor
    {
        private string _name;
        private Color _color;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionCommandColor"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="color">The color.</param>
        public ACColor(string name, Color color)
        {
            _name = name;
            _color = color;
        }

        /// <summary>
        /// The color argument specifies the color to apply. The color can either be specified as one of many preset word values or as a "raw" hexadecimal value giving the red/green/blue component values (the same format as used for the "BGCOLOR=" tag in HTML).
        /// </summary>
        /// <value>The color.</value>
        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }

        /// <summary>
        /// The optional name argument specifies the name of the object to color. Object names are assigned via the name command.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
    }
}
