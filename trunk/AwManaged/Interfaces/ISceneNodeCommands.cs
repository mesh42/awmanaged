using AwManaged.Math;
using AwManaged.SceneNodes;

namespace AwManaged.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISceneNodeCommands
    {
        /// <summary>
        /// Deletes the V3 object.
        /// </summary>
        /// <param name="o">The o.</param>
        void DeleteObject(Model o);
        /// <summary>
        /// Deletes the V3 object by its id.
        /// </summary>
        /// <param name="id">The id.</param>
        void DeleteObject(int id);
        /// <summary>
        /// Adds the V3 object.
        /// </summary>
        /// <param name="o">The o.</param>
        void AddObject(Model o);
        /// <summary>
        /// Adds a new object.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <param name="model">The model.</param>
        /// <param name="position">The position.</param>
        /// <param name="rotation">The rotation.</param>
        /// <param name="action">The action.</param>
        void AddObject(string description, string model, Vector3 position, Vector3 rotation, string action);
        /// <summary>
        /// Changes the object.
        /// </summary>
        /// <param name="o">The o.</param>
        void ChangeObject(Model o);
    }
}
