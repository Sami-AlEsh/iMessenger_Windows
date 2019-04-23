﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;

namespace iMessenger.Scripts
{
    [Serializable]
    public class MainUser : User
    {
        #region Attributes

        //Main User Attributes:
        private string StartDate;
        private string AccessToken;

        //Main User Property:
        public bool verified { set; get; } = false;

        //Other Attributes:
        private List<User> Friends = new List<User>();

        #endregion

        #region Constructor
        public MainUser() : base ("","","") {}
        public MainUser(string name,string userName , string email , string AccessToken) : base(name, userName, email)
        {
            this.AccessToken = AccessToken;
            StartDate = DateTime.Now.ToFileTime().ToString();
            verified = true;
        }

        #endregion

        /// <summary>
        /// Loads Current Main User
        /// </summary>
        /// <returns></returns>
        public static MainUser LoadLocalMainUser()
        {
            //Quickly check for Local Main User:
            string ProjectBinPath = Environment.CurrentDirectory;
            string ProjectPath = Directory.GetParent(ProjectBinPath).Parent.FullName;
            string MainUserPath = ProjectPath + @"\MainUser\MainUser.binary";

            if (!File.Exists(ProjectPath + @"\MainUser\MainUser.binary"))
            {
                Console.WriteLine("No Local Main User Found !");
                return null;
            }
            else
            {
                Console.WriteLine("Local Main User Found !");

                BinaryFormatter BF = new BinaryFormatter();
                FileStream fs = new FileStream(MainUserPath , FileMode.Open, FileAccess.ReadWrite);
                //MainUser mainUser = null;

                try
                {
                    return (MainUser)BF.Deserialize(fs) as MainUser;
                    //Console.WriteLine("SH : " + mainUser.AccessToken);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Binary Formatter Deserialize Error => " + e.Message);
                    return null;
                }
                finally
                {
                    fs.Close();
                }
                //return new MainUser(mainUser.name, mainUser.userName, mainUser.email, mainUser.AccessToken);
            }

        }

        public static void SaveLocalMainUser(MainUser currentMainUser)
        {
            string ProjectBinPath = Environment.CurrentDirectory;
            string ProjectPath = Directory.GetParent(ProjectBinPath).Parent.FullName;


            BinaryFormatter BF = new BinaryFormatter();
            FileStream fs = new FileStream(ProjectPath + @"\MainUser\MainUser.binary", FileMode.Create);
            try
            {
                fs.Position = 0;
                BF.Serialize(fs , currentMainUser);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                fs.Close();
            }
            Console.WriteLine("Main User Saved to Local Storage");
        }





        public void UpdateFriendsList()
        {
            //TODO : create TASK to send Get Requet to server
        }
        public static void AddFriend()
        {

        }
    }
}
