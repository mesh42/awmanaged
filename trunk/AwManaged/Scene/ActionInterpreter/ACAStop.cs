namespace AwManaged.Scene.ActionInterpreter
{
    /// <summary>
    /// The astop command stops a running animation.
    /// </summary>
    class ACAStop
    {
        private string _name;
        private bool _isGlobal;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionCommandAStop"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="isGlobal">if set to <c>true</c> [is global].</param>
        public ACAStop(string name, bool isGlobal)
        {
            _name = name;
            _isGlobal = isGlobal;
        }

        /// <summary>
        /// The optional global argument will cause triggers to initiate the command for all users have the object in view. Without it, the command will be triggered exclusively for the user who activates the trigger (bump, activate, adone). By default, commands are not global.
        /// </summary>
        /// <value><c>true</c> if this instance is global; otherwise, <c>false</c>.</value>
        public bool IsGlobal
        {
            get { return _isGlobal; }
            set { _isGlobal = value; }
        }

        /// <summary>
        /// The optional name argument can be used to specify the name of the object containing the animation to be stopped. Object names are assigned via the name command.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
    }
}
