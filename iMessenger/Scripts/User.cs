﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iMessenger.Scripts
{
    [Serializable]
    public class User
    {
        protected string name;
        protected string userName;
        protected string email;
        protected string profilePhotoUrl;

        public User(string name, string userName, string email)
        {
            this.name = name;
            this.userName = userName;
            this.email = email;
        }

    }
}
