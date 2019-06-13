﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using iMessenger.Scripts.Events;

namespace iMessenger.Scripts
{
    [Serializable]
    public class MainUser : User
    {
        public static MainUser mainUser;
        #region Attributes

        //Main User Attributes:
        private string StartDate;
        public string AccessToken;

        //Main User Property:
        public bool verified { set; get; } = false;

        //Other Attributes:
        public List<User> Friends = new List<User>();
        public Dictionary<string,List<Message> > FrindsChat = new Dictionary<string,List<Message> >();
        public static Queue<Message> ChatsQueue = new Queue<Message>();
        #endregion

        #region Constructor
        public MainUser() : base ("","","") { /*if(LoadLocalMainUser() != null ) mainUser = LoadLocalMainUser();*/ }
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
            string MainUserPath = Project.Path + @"\MainUser\MainUser.binary";

            if (!File.Exists(Project.Path + @"\MainUser\MainUser.binary"))
            {
                Console.WriteLine("No Local Main User Found !");
                return null;
            }
            else
            {
                Console.WriteLine("Local Main User Found !");

                BinaryFormatter BF = new BinaryFormatter();
                FileStream fs = new FileStream(MainUserPath , FileMode.Open, FileAccess.ReadWrite);

                try
                {
                    return (MainUser)BF.Deserialize(fs) as MainUser;
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
            }

        }

        public static void SaveLocalMainUser()
        {
            BinaryFormatter BF = new BinaryFormatter();
            FileStream fs = new FileStream(Project.Path + @"\MainUser\MainUser.binary", FileMode.Create);
            try
            {
                fs.Position = 0;
                BF.Serialize(fs , mainUser);
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





        public static void UpdateFriendsList()
        {
            //return;

            //TODO Get Friends List:
            mainUser.Friends.Clear();
            mainUser.Friends.Add(new User("sami98", "sami98", "sami98@gmail.com"));
        }

        /// <summary>
        /// Add new Friend to MainUser Object & UI
        /// </summary>
        /// <param name="user"></param>
        public static void AddFriend(User user)
        {
            initFriend_directories(user.userName);
            mainUser.Friends.Add(user);
            mainUser.FrindsChat[user.userName] = new List<Message>();

            //add friend to UI
            SideMenu.friendsList.AddFriend_UI(new ChatListItemControl(user.userName, user.userName.ToUpper()[0].ToString() , "#New_friend!"));
        }
        public static void Delete_Block_Friend(User user)
        {
            DeleteFriend_directories(user.userName);
            mainUser.Friends.Remove(user);
            mainUser.FrindsChat.Remove(user.userName);

            //delete friend from UI
            SideMenu.friendsList.DeleteFriend_UI(user.userName);
        }

        private static void DeleteFriend_directories(string userName)
        {
            //TODO : delete paths and folders and chat.json file
        }

        private static void initFriend_directories(string username)
        {
            //TODO : create paths and folders and chat.json file
        }
    }
}
