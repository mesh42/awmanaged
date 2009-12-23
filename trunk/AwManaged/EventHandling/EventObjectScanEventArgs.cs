using System.Collections.Generic;
using AwManaged;
using AwManaged.Core;
using AwManaged.SceneNodes;

namespace AwManaged.EventHandling
{
    public class EventObjectScanEventArgs : IEventObjectScanEventArgs
    {
        #region IEventObjectScanEventArgs Members

        public ProtectedList<Model> Model { get; set; }

        public EventObjectScanEventArgs(ProtectedList<Model> model)
        {
            Model = model;
        }

        #endregion
    }
}