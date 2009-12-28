using AwManaged.Storage.Interfaces;

namespace AwManaged.Storage
{
    public class Db4OConnection : IDb4OConnection
    {
        #region IDb4OConnection Members

        public string HostAddress
        {
            get; internal set;
        }

        public int HostPort
        {
            get;
            internal set;
        }

        public string File
        {
            get;
            internal set;
        }

        public string User
        {
            get;
            internal set;
        }

        public string Password
        {
            get;
            internal set;
        }

        #endregion
    }
}
