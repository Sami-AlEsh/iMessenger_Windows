using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iMessenger.Scripts
{
    public class User
    {
        private string name;
        private string userName;
        private string email;
        //private string profilePhotoPath; We Know it from his UserName !

        public User(string name, string userName, string email)
        {
            this.name = name;
            this.userName = userName;
            this.email = email;
        }
    }
}
