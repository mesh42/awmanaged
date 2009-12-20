using AW;
using AwManaged.SceneNodes;

namespace AwManaged.SceneNodes
{
    public class MoverObject
    {
        private readonly Mover mover;
        private readonly Model object_;
        //public AW.Mover Mover { get; }
        //public  Bot.AWManaged.Object { get; }

        public MoverObject(AW.Mover mover, Model object_)
        {
            this.mover = mover;
            this.object_ = object_;
        }

        public Model Object_
        {
            get { return object_; }
        }

        public Mover Mover
        {
            get { return mover; }
        }
    }
}