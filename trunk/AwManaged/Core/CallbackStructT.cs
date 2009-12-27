using AwManaged.Core.Interfaces;

namespace AwManaged.Core
{
    public sealed class CallbackStructT<T> : ICallbackStructT<T> where T : ICloneableT<T>
    {
        #region ICallbackStructT<T> Members

        public T Clone
        {
            get; private set;
        }

        public System.Threading.Timer Timer
        {
            get;
            internal set;
        }

        public object[] Param
        {
            get; internal set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CallbackStructT&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="clone">The clone.</param>
        public CallbackStructT(T clone)
        {
            Clone = clone;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CallbackStructT&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="clone">The clone.</param>
        /// <param name="param">The param.</param>
        public CallbackStructT(T clone, object[] param)
        {
            Clone = clone;
            Param = param;
        }

        #endregion
    }
}
