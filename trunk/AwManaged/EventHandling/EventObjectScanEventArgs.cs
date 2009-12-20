using System.Collections.Generic;
using AwManaged;
using AwManaged.SceneNodes;

namespace AwManaged.EventHandling
{
    public class EventObjectScanEventArgs : IEventObjectScanEventArgs
    {
        #region IEventObjectScanEventArgs Members

        public List<Model> Model { get; set; }

        public EventObjectScanEventArgs(List<Model> model)
        {
            Model = model;
        }

        #endregion
    }
}