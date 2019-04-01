using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;

namespace iMessenger.Scripts
{
    public class MainUser : User
    {
        #region Attributes

        //Main User Attributes:
        private string startDate;
        private string accessToken;

        //Main User Property:
        public bool verified { set; get; } = false;

        //Other Attributes:
        private List<User> Friends;

        #endregion

        #region Constructor
        public MainUser(string name,string userName , string email) : base(name, userName, email)
        {
            startDate = DateTime.Now.ToFileTime().ToString();
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
            if (!File.Exists( ProjectPath + @"\MainUser\MainUser.binary")) return null;
            //if (!File.Exists(@"\MainUser\MainUser.binary")) return null;
            else
            {
                Console.WriteLine("File Founded");
                MainUser mainUser = null ;
                BinaryFormatter BF = new BinaryFormatter();
                FileStream fs = new FileStream(ProjectPath + @"\MainUser\MainUser.binary", FileMode.Open);
                try
                {
                    //fs.Position = 0;
                    mainUser = (MainUser)BF.Deserialize(fs);
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    fs.Close();
                }

                return mainUser;
            }

        }
        public static void SaveLocalMainUser(MainUser mainUser)
        {
            string ProjectBinPath = Environment.CurrentDirectory;
            string ProjectPath = Directory.GetParent(ProjectBinPath).Parent.FullName;

            if (File.Exists(ProjectPath + @"\MainUser\MainUser.binary"))
            {
                Console.WriteLine("Old MainUser File Deleted !");
                File.Delete(ProjectPath + @"\MainUser\MainUser.binary");
            }
            else
            {
                Console.WriteLine("File Founded");
                BinaryFormatter BF = new BinaryFormatter();
                FileStream fs = new FileStream(ProjectPath + @"\MainUser\MainUser.binary", FileMode.CreateNew);
                try
                {
                    //fs.Position = 0;
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
            }

        }

        public static void UpdateFriendsList()
        {
            //TODO : create TASK to send Get Requet
        }
        public static void AddFriend()
        {

        }
        //public bool GetUserAuthentication()
        //{
        //    string ProjectBinPath = Environment.CurrentDirectory;
        //    string ProjectPath = Directory.GetParent(ProjectBinPath).Parent.FullName;
        //    if (File.Exists(ProjectPath + @"\AccessToken\AT.bin"))
        //    {
        //        accessToken = File.ReadAllText(ProjectPath + @"\AccessToken\AT.bin");
        //        Console.WriteLine("Your AccessToken -> "+accessToken);
        //        return true;
        //    }
        //    else
        //    {
        //        Console.WriteLine("No Current User to Auto-Login !");
        //        return false;
        //    }
        //}
    }
}
