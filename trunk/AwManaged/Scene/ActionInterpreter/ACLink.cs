namespace AwManaged.Scene.ActionInterpreter
{
    /// <summary>
    /// The link command allows an object to be "attached" to a Mover with the specified name.
    /// 
    /// In order for the command to work correctly, the Mover must have "Linking Enabled" checked in the Mover options. There must also be a Name specified in the Mover, matching the movername specified in the link command.
    /// Note that the linked object and the mover must be built using the same object owner.
    /// </summary>
    public class ACLink
    {
        private string _moverName;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionCommandLink"/> class.
        /// </summary>
        /// <param name="moverName">Name of the mover.</param>
        public ACLink(string moverName)
        {
            _moverName = moverName;
        }

        /// <summary>
        /// Gets or sets the name of the mover to link to object to.
        /// </summary>
        /// <value>The name of the mover.</value>
        public string MoverName
        {
            get { return _moverName; }
            set { _moverName = value; }
        }
    }
}
