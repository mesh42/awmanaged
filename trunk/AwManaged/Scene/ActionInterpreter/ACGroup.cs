namespace AwManaged.Scene.ActionInterpreter
{
    /// <summary>
    /// The group command is used to load an Object Group from the object path.
    /// </summary>
    class ACGroup
    {
        private string _name;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionCommandGroup"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public ACGroup(string name)
        {
            _name = name;
        }

        /// <summary>
        /// The name argument is the filename of a zipped AWG file located on the object path in the groups subfolder. Note that no encroachment will be determined for the group's children, nor do these children objects increase the cell data limit.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
    }
}
