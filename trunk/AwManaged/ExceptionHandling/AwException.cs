using System;
using AwManaged.ExceptionHandling;

namespace AwManaged.ExceptionHandling
{
    /// <summary>
    /// AW SDK to native .NET Exception wrapper.
    /// </summary>
    public class AwException : Exception
    {
        /// <summary>
        /// Gets or sets the return code.
        /// </summary>
        /// <value>The return code number.</value>
        public int Rc { get; private set; }
        /// <summary>
        /// Gets or sets the enumerated rc.
        /// </summary>
        /// <value>The enumerated rc.</value>
        public ReasonCodeReturnType RcEnumerated { get; private set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="AwException"/> class.
        /// </summary>
        /// <param name="rc">The rc.</param>
        public AwException(int rc) : base(((AwExceptionAttribute)typeof (ReasonCodeReturnType).GetField(((ReasonCodeReturnType) rc).ToString()).GetCustomAttributes(
                                                                     typeof (AwExceptionAttribute), false)[0]).Message)
        {
            Rc = rc;
            RcEnumerated = (ReasonCodeReturnType) Rc;
        }
    }
}