using System;

namespace AwManaged.ExceptionHandling
{
    /// <summary>
    /// Attribute describing the RC enumeration, in a human readable format.
    /// </summary>
    public class AwExceptionAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the message in the human readable format.
        /// </summary>
        /// <value>The message.</value>
        public string Message { get; set; }
    }
}