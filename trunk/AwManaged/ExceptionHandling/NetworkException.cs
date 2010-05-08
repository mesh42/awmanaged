using SharedMemory;using System;

namespace AwManaged.ExceptionHandling
{
    public class NetworkException : Exception
    {
        private readonly string _message;

        public NetworkException(string message)
        {
            _message = message;
        }

        public string Message1
        {
            get { return _message; }
        }
    }
}
