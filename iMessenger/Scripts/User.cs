using System;

namespace iMessenger.Scripts
{
    [Serializable]
    public class User
    {
        public string name;
        public string userName;
        protected string email;
        //protected string profilePhotoUrl;

        public User(string name, string userName, string email)
        {
            this.name = name;
            this.userName = userName;
            this.email = email;
        }

    }
}
