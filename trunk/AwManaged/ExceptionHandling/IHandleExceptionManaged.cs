using AW;

namespace AwManaged.ExceptionHandling
{
    /// <summary>
    /// Manage exception handling.
    /// </summary>
    public interface IHandleExceptionManaged
    {
        /// <summary>
        /// Handles the exception managed.
        /// </summary>
        /// <param name="rc">The rc.</param>
        void HandleExceptionManaged(int rc);
        /// <summary>
        /// Handles the exception managed.
        /// </summary>
        /// <param name="instanceExcpetion">The instance excpetion.</param>
        void HandleExceptionManaged(InstanceException instanceExcpetion);
    }
}
