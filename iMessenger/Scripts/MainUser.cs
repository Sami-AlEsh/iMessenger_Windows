using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Runtime.Serialization.Formatters.Binary;
using iMessenger.Scripts.Events;
using RestSharp;
using Newtonsoft.Json.Linq;
using System.Windows;
using iMessenger.Scripts.RSA;
using iMessenger.Scripts.AES;
using System.Security.Cryptography;

namespace iMessenger.Scripts
{
    [Serializable]
    public class MainUser : User
    {
        public static MainUser mainUser = new MainUser();
        #region Attributes

        //Main User Attributes:
        private string StartDate;
        public string AccessToken;

        //Main User Property:
        public bool verified { set; get; } = false;

        //Other Attributes:
        public List<User> Friends = new List<User>();
        public Dictionary<string, List<Message>> FrindsChat = new Dictionary<string, List<Message>>();
        public Queue<Message> ChatsQueue = new Queue<Message>();

        //RSA Keys
        private RSA_keys keys_RSA;
        private Dictionary<string, string> keys_AES;
        #endregion

        #region Constructor
        public MainUser() : base("", "", "") { /*if(LoadLocalMainUser() != null ) mainUser = LoadLocalMainUser();*/ }
        public MainUser(string name, string userName, string email, string AccessToken) : base(name, userName, email)
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
                FileStream fs = new FileStream(MainUserPath, FileMode.Open, FileAccess.ReadWrite);

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
            Console.WriteLine("MainUser Friends when saving is " + mainUser.Friends);
            BinaryFormatter BF = new BinaryFormatter();
            FileStream fs = new FileStream(Project.Path + @"\MainUser\MainUser.binary", FileMode.Create);
            try
            {
                fs.Position = 0;
                BF.Serialize(fs, mainUser);
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

        public static MainUser LoadLocalMainUserJS()
        {
            //Quickly check for Local Main User:
            string MainUserPath = Project.Path + @"\MainUser\MainUser.json";

            if (!File.Exists(Project.Path + @"\MainUser\MainUser.json"))
            {
                Console.WriteLine("No Local Main User Found !");
                return null;
            }
            else
            {
                Console.WriteLine("Local Main User Found !");
                return JsonConvert.DeserializeObject<MainUser>(File.ReadAllText(Project.Path + @"\MainUser\MainUser.json"));
            }

        }

        public static void SaveLocalMainUserJS()
        {
            Console.WriteLine("MainUser Friends when saving is " + mainUser.Friends.Count);
            File.WriteAllText(Project.Path + @"\MainUser\MainUser.json", JsonConvert.SerializeObject(mainUser));
            Console.WriteLine("Main User Saved to Local Storage");
        }





        public static void UpdateFriendsList()
        {
            //Get MyFriends:
            var ServerUri = new Uri("http://" + MyTcpSocket.ServerIp + ":" + "8080");

            var client = new RestClient(ServerUri);
            //HTTP Request Route & Method
            var request1 = new RestRequest("/user/friends/" + mainUser.userName, Method.GET);

            //GET BLOCKED FRIENDS:
            var request2 = new RestRequest("/user/blockedUsers/" + mainUser.userName, Method.GET);

            try
            {
                client.ExecuteAsync(request1, response =>
                {
                    //Json response
                    var JsonResponse = new JObject();
                    try
                    {
                        JsonResponse = JObject.Parse(response.Content);

                        if ((bool)JsonResponse["status"])
                        {
                            JArray friends = (JArray)JsonResponse["data"];

                            for (int i = 0; i < friends.Count; i++)
                            {
                                User n_friend = new User((string)friends[i], (string)friends[i], (string)friends[i] + "@gmail.com");
                                Application.Current.Dispatcher.Invoke(() => MainUser.AddFriend(n_friend));
                            }
                            Console.WriteLine("Friends Count :: " + friends.Count);

                            /////////////////////
                            // Get Blocked User :
                            /////////////////////
                            client.ExecuteAsync(request2, response2 =>
                            {
                                //Json response
                                try
                                {
                                    JsonResponse = JObject.Parse(response2.Content);

                                    if ((bool)JsonResponse["status"])
                                    {
                                        JArray blockedFriends = (JArray)JsonResponse["data"];

                                        for (int i = 0; i < blockedFriends.Count; i++)
                                        {
                                            MainUser.mainUser.Friends.Find(p => p.userName == (string)blockedFriends[i]).blocked = true;
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("HTTP Request failed [GetBlockedFriends]# RESPONSE => " + response.Content);
                                    }
                                }
                                catch (JsonReaderException)
                                {
                                    Console.WriteLine("Error parsing BlockedUsers JSON Response");
                                }
                            });
                        }
                        else
                        {
                            Console.WriteLine("HTTP Request failed [GetFriends]# RESPONSE => " + response.Content);
                        }
                    }
                    catch (JsonReaderException)
                    {
                        Console.WriteLine("Error parsing GetFriends JSON Response "+ JsonResponse.ToString());
                    }
                });
            }
            catch (Exception error)
            {
                Console.WriteLine("#ERROR in sending HTTP Request [LastSeen]: " + error.Message);
            }
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
            SideMenu.friendsList.AddFriend_UI(new ChatListItemControl(user.userName, user.userName.ToUpper()[0].ToString(), "#New_friend!"));
        }
        public static void Delete_Friend(User user)
        {
            //Delete User and his Chat from MainUser Object:
            var index = mainUser.Friends.FindIndex(p => p.userName == user.userName);
            mainUser.FrindsChat.Remove(user.name);
            mainUser.Friends.RemoveAt(index);

            //Delete his Directory
            DeleteFriend_directories(user.userName);

            //delete friend from UI
            SideMenu.friendsList.DeleteFriend_UI(user.userName);
        }

        private static void DeleteFriend_directories(string userName)
        {
            Directory.Delete(Project.Path + @"\Database\" + userName, true);
        }

        private static void initFriend_directories(string username)
        {
            Directory.CreateDirectory(Project.Path + @"\Database\" + username);
            Directory.CreateDirectory(Project.Path + @"\Database\" + username + @"\images");
            Directory.CreateDirectory(Project.Path + @"\Database\" + username + @"\binaryfiles");
            File.Create(Project.Path + @"\Database\" + username + @"\chat.json");
        }

        /// <summary>
        /// Bolcks a Friend
        /// </summary>
        /// <param name="user"></param>
        public static void Block_Friend(User user)
        {
            //Block User:
            var index = mainUser.Friends.Find(p => p.userName == user.userName).blocked = true;
        }
        /// <summary>
        /// UnBlock a Friend
        /// </summary>
        /// <param name="user"></param>
        public static void UnBlock_Friend(User user)
        {
            //Block User:
            var index = mainUser.Friends.Find(p => p.userName == user.userName).blocked = false;
        }

        /// <summary>
        /// Init RSA-Keys & AES-Keys for MainUser
        /// </summary>
        public void InitializeAllKeys()
        {
            keys_RSA = new RSA_keys();
            keys_AES = new Dictionary<string, string>();

            //RSA:
            keys_RSA = RSA_keys.GetKeys();
            RSA_keys.StoreKeys(keys_RSA);

            //AES
            keys_AES = AESOperation.GetKey();
            AESOperation.StoreKeys(keys_AES);
        }
        public void UploadRSAPublicKey()
        {
            var ServerUri = new Uri("http://" + MyTcpSocket.ServerIp + ":" + "8080");

            var client = new RestClient(ServerUri);
            //HTTP Request Route & Method
            var request = new RestRequest("/user/setPublicKey/", Method.POST);

            //Get Public Key as XML:
            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
            RSA.ImportParameters(keys_RSA.publicKey);
            var XML = RSA.ToXmlString(false);

            string jsonToSend = new JObject(new JProperty("username", mainUser.userName),
                                                new JProperty("platform", "windows"),
                                                new JProperty("publicKey", XML)
                                                ).ToString();

            request.AddParameter("application/json; charset=utf-8", jsonToSend, ParameterType.RequestBody);
            request.RequestFormat = RestSharp.DataFormat.Json;
            try
            {
                client.ExecuteAsync(request, response => Console.WriteLine("PublicKey Sent !"));
            }
            catch (Exception e)
            {
                Console.Write("#ERROR in sending HTTP Request [SetPublicKey]: "+ e.Message);
            }
        }
    }
}
