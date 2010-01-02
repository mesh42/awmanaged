using System.Collections.Generic;

namespace AwManaged.Scene.ActionInterpreter
{
    /// <summary>
    /// Using the lock command to the owner of the object. Other people that try and use the object command will not be able to. An example of a use of the lock command would be for locking doors that only you and a few of your friends can open.
    /// The lock action command can be used to lock all actions listed after it, to be used by the object owner and/or to a list of citizen numbers. It works in combination with create, activate and bump triggers.
    /// Tourists cannot take advantage of this command.
    /// 
    /// Actions listed before the lock statement are executed for everyone. All actions after the lock statement are only executed for the allowed citizens or privilege used by a citizen.
    /// Only object owners and citizens listed can execute the action command global, even if the global statement is listed before the lock statement.
    /// </summary>
    public class ACLock
    {
        private List<int> _owners;

        public ACLock(List<int> owners)
        {
            _owners = owners;
        }

        /// <summary>
        /// The owners argument allows the user to define a list of citizen numbers (separated by colons) that will be able to use the following command.
        /// </summary>
        /// <value>The owners.</value>
        public List<int> Owners
        {
            get { return _owners; }
            set { _owners = value; }
        }
    }
}
