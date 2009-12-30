using AwManaged.Core.Interfaces;

namespace AwManaged.Core
{
    public sealed class DependendObject<TParent, TChild> : ICloneableT<DependendObject<TParent, TChild>>
        where TParent : ICloneableT<TParent>
        where TChild : ICloneableT<TChild> 
    {
        private TParent Parent { get; set;}
        private TChild Child { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DependendObject&lt;TParent, TChild&gt;"/> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="child">The child.</param>
        public DependendObject(TParent parent, TChild child)
        {
            Parent = parent;
            Child = child;
        }

        #region ICloneableT<DependendObject<TParent,TChild>> Members

        DependendObject<TParent, TChild> ICloneableT<DependendObject<TParent, TChild>>.Clone()
        {
            return (DependendObject<TParent, TChild>) MemberwiseClone();
        }

        #endregion
    }
}
