using SharedMemory;using System;
using AwManaged.Scene;

namespace StandardBotPluginLibrary.AfkBot
{
    public class AvatarStatus
    {
        private Avatar _avatar;
        private DateTime _lastSeen;

        public AvatarStatus(Avatar avatar)
        {
            _avatar = avatar;
            _lastSeen = DateTime.Now;
        }

        public bool IsIdle
        { 
            get
            {
                return DateTime.Now.Subtract(_lastSeen).TotalMinutes > 10;
            }
        }

        public Avatar Avatar
        {
            get { return _avatar; }
            set { _avatar = value;}
        }

        public DateTime LastSeen
        {
            get { return _lastSeen; }
            set { _lastSeen = value; }
        }
    }
}
