using AwManaged.Core;
using AwManaged.Core.Interfaces;
using AwManaged.EventHandling.Interfaces;
using AwManaged.SceneNodes;

namespace AwManaged.EventHandling
{
    /// <summary>
    /// Raised when object scanning of a world has been completed.
    /// </summary>
    public sealed class EventObjectScanCompletedEventArgs : IEventObjectScanEventArgs<Model>
    {
        #region IEventObjectScanEventArgs Members

        public ProtectedList<Model> Model { get; set; }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="EventObjectScanCompletedEventArgs"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        public EventObjectScanCompletedEventArgs(ICloneableListT<ProtectedList<Model>, Model> model)
        {
            Model = model.Clone();
        }
    }
}