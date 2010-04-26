//using System;
//using AwManaged.LocalServices.WebServer.Attributes;

//namespace AwManaged.WebTest
//{
//    public class DataTestClass
//    {
//        private readonly Guid _id;
//        private readonly string _userName;
//        private readonly string _password;
//        private readonly string _firstName;
//        private readonly string _lastName;

//        /// <summary>
//        /// 
//        /// </summary>
//        public DataTestClass(string userName, string password, string firstName, string lastName)
//        {
//            _userName = userName;
//            _password = password;
//            _firstName = firstName;
//            _lastName = lastName;
//        }

//        public DataTestClass(){}

//        public Guid Id
//        {
//            get { return _id;}
//        }

//        [WebFormField("[a-z,A-Z]{1,24}")]
//        public string LastName
//        {
//            get { return _lastName; }
//        }

//        [WebFormField("[a-z,A-Z]{1,24}")]
//        public string FirstName
//        {
//            get { return _firstName; }
//        }

//        [WebFormField("[a-z,A-Z]{1,24}")]
//        public string Password
//        {
//            get { return _password; }
//        }

//        [WebFormField("[a-z,A-Z]{1,24}")]
//        public string UserName
//        {
//            get { return _userName; }
//        }
//    }
//}
