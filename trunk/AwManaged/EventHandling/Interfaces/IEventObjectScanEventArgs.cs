using AwManaged.Core;
using AwManaged.SceneNodes.Interfaces;

namespace AwManaged.EventHandling.Interfaces
{
    interface IEventObjectScanEventArgs<TModel> where TModel : IModel<TModel>
    {
        /// <summary>
        /// Gets a shallow memberwise cloned copy of the model cache.
        /// </summary>
        ProtectedList<TModel> Model { get; set; }
    }
}
