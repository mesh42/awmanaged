using AwManaged.Interfaces;
using AwManaged.Math;
using AwManaged.SceneNodes;

namespace AwManaged.SceneNodes.Interfaces
{
    public interface IAvatar
    {
        int Session { get; set; }
        string Name { get; set; }
        Vector3 Position { get; set; }
        Vector3 Rotation { get; set; }
        int Gesture { get; set; }
        int Citizen { get; set; }
        int Privilege { get; set; }
        int State { get; set; }

        IPropertyBag<IAvatar> Properties { get; set; }

        /// <summary>
        /// For games... maybe need to expand on this.
        /// </summary>
        //int Score { get; set; }

        //int ScoreAgainst { get; set; }
        event Avatar.OnChangePositionDelegate OnChangePosition;
        void ChangedPosition();
    }
}