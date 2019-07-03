using iMessenger.Scripts;
using iMessenger.Scripts.Events;
using iMessenger.Scripts.RSA;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Windows;
using System.Windows.Controls;

namespace iMessenger
{
    /// <summary>
    /// Interaction logic for ChatListItemControl_search.xaml
    /// </summary>
    public partial class ChatListItemControl_search : UserControl
    {
        User thisUser;
        public ChatListItemControl_search()
        {
            InitializeComponent();
        }

        public ChatListItemControl_search(User user)
        {
            InitializeComponent();
            this.thisUser = user;

            //Alias
            this.FriendNameAlias.Text = user.name.ToUpper()[0].ToString();
            
            //Name
            this.FriendName.Text = user.name;

            //Buttons Management:
            if(user.userName == MainUser.mainUser.userName)
            {
                this.Add_Btn.IsEnabled = false;
                this.Delete_Btn.IsEnabled = false;
                this.Block_Btn.IsEnabled = false;
                this.UnBlock_Btn.IsEnabled = false;
                return;
            }
            var foundedUser = MainUser.mainUser.Friends.Find(p => p.userName == user.userName);
            if (foundedUser != null)
            {
                if (!foundedUser.blocked)
                {
                    this.Add_Btn.IsEnabled = false;
                    this.UnBlock_Btn.IsEnabled = false;

                }
                else
                {
                    this.UnBlock_Btn.IsEnabled = true;
                    this.Add_Btn.IsEnabled = false;
                    this.Block_Btn.IsEnabled = false;
                }
            }
            else
            {
                this.Delete_Btn.IsEnabled = false;
                this.UnBlock_Btn.IsEnabled = false;
            }
        }

        private void AddFriend(object sender, RoutedEventArgs e)
        {
            var ServerUri = new Uri("http://" + MyTcpSocket.ServerIp + ":" + "8080");

            var client = new RestClient(ServerUri);
            //HTTP Request Route & Method
            var request = new RestRequest("/user/addFriend", Method.POST);

            string jsonToSend = new JObject(new JProperty("current", MainUser.mainUser.userName),
                                             new JProperty("friend", thisUser.userName)
                                             ).ToString();

            request.AddParameter("application/json; charset=utf-8", jsonToSend, ParameterType.RequestBody);
            request.RequestFormat = RestSharp.DataFormat.Json;

            client.ExecuteAsync(request, response =>
            {
                try
                {
                    JObject JSResponse = JObject.Parse(response.Content);
                    if ((bool)JSResponse["status"])
                    {

                        /////////////////////////////////////////
                        //Add a new AES-Key for friend relation//
                        /////////////////////////////////////////

                        //Get MyFriend PublicKeys
                        var client2 = new RestClient(ServerUri);
                        var request2 = new RestRequest("/user/getPublicKey/" + thisUser.userName, Method.GET);

                        client2.ExecuteAsync(request2, response2 =>
                        {
                            //Json response
                            var JsonResponse = new JObject();
                            try
                            {
                                JsonResponse = JObject.Parse(response2.Content);

                                if ((bool)JsonResponse["status"])
                                {
                                    this.Dispatcher.Invoke(() =>
                                    {
                                        //Update MainUser Object & UI:
                                        MainUser.AddFriend(thisUser);

                                        //Buttons
                                        this.Add_Btn.IsEnabled = false;
                                        this.Delete_Btn.IsEnabled = true;
                                    });

                                    JArray Keys = (JArray)JsonResponse["data"];
                                    foreach (JObject key in Keys)
                                    {
                                        var platform = (string)key["platform"];

                                        if (platform == "windows")
                                        {
                                            var publicKey_XML = (string)key["publicKey"];
                                            var encryptedSecretKey = MainUser.mainUser.GetNewAESKeyEncryptedWith(thisUser.userName, publicKey_XML, Platform.Windows);
                                            new Event_UpdateSecretKey(thisUser.userName, Platform.Windows, encryptedSecretKey).SendMessage();
                                        }
                                        else if (platform == "android")
                                        {
                                            var publicKey_JXML = (string)key["publicKey"];
                                            var encryptedSecretKey = MainUser.mainUser.GetNewAESKeyEncryptedWith(thisUser.userName, publicKey_JXML, Platform.Android);
                                            new Event_UpdateSecretKey(thisUser.userName, Platform.Android, encryptedSecretKey).SendMessage();
                                        }
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("HTTP Request failed [GetFriendPublicKey]# RESPONSE => " + response.Content);
                                }
                            }
                            catch (JsonReaderException)
                            {
                                Console.WriteLine("Error parsing GetFriendPublicKey JSON Response " + JsonResponse.ToString());
                            }
                            finally
                            {
                                MainUser.mainUser.SaveSecretKeys();
                            }
                        });
                    }
                    else
                    { Console.WriteLine("Status False => RES : " + response.Content); }
                }
                catch (JsonReaderException error)
                {
                    Console.WriteLine("#ERROR in sending HTTP Request Method [Add New Friend]: " + error.Message);
                }
            });
        }

        private void DeleteFriend(object sender, RoutedEventArgs e)
        {
            var ServerUri = new Uri("http://" + MyTcpSocket.ServerIp + ":" + "8080");

            var client = new RestClient(ServerUri);
            //HTTP Request Route & Method
            var request = new RestRequest("/user/delete", Method.POST);

            string jsonToSend = new JObject(new JProperty("username", MainUser.mainUser.userName),
                                             new JProperty("delete", thisUser.userName)
                                             ).ToString();

            request.AddParameter("application/json; charset=utf-8", jsonToSend, ParameterType.RequestBody);
            request.RequestFormat = RestSharp.DataFormat.Json;

            client.ExecuteAsync(request, response =>
            {
                try
                {
                    JObject JSResponse = JObject.Parse(response.Content);
                    if ((bool)JSResponse["status"])
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            //Update MainUser Object & UI:
                            MainUser.Delete_Friend(thisUser);

                            //Buttons
                            this.Add_Btn.IsEnabled = true;
                            this.Delete_Btn.IsEnabled = false;
                        });
                    }
                    else
                    { Console.WriteLine("Status False => RES : " + response.Content); }
                }
                catch (JsonReaderException error)
                {
                    Console.WriteLine("#ERROR in sending HTTP Request Method [Delete Friend]: " + error.Message);
                }
            });
        }

        private void BlockFriend(object sender, RoutedEventArgs e)
        {
            var ServerUri = new Uri("http://" + MyTcpSocket.ServerIp + ":" + "8080");

            var client = new RestClient(ServerUri);
            //HTTP Request Route & Method
            var request = new RestRequest("/user/blockUser", Method.POST);

            string jsonToSend = new JObject(new JProperty("username", MainUser.mainUser.userName),
                                             new JProperty("block", thisUser.userName)
                                             ).ToString();

            request.AddParameter("application/json; charset=utf-8", jsonToSend, ParameterType.RequestBody);
            request.RequestFormat = RestSharp.DataFormat.Json;

            client.ExecuteAsync(request, response =>
            {
                try
                {
                    JObject JSResponse = JObject.Parse(response.Content);
                    if ((bool)JSResponse["status"])
                    {
                        //Update MainUser Object:
                        MainUser.Block_Friend(thisUser);

                        this.Dispatcher.Invoke(() =>
                        {
                            //Buttons
                            this.Add_Btn.IsEnabled = false;
                            this.Delete_Btn.IsEnabled = false;
                            this.Block_Btn.IsEnabled = false;
                            this.UnBlock_Btn.IsEnabled = true;
                        });
                    }
                    else
                    { Console.WriteLine("Status False => RES : " + response.Content); }
                }
                catch (JsonReaderException error)
                {
                    Console.WriteLine("#ERROR in sending HTTP Request Method [Block Friend]: " + error.Message);
                }
            });
        }

        private void UnBlockFriend(object sender, RoutedEventArgs e)
        {
            var ServerUri = new Uri("http://" + MyTcpSocket.ServerIp + ":" + "8080");

            var client = new RestClient(ServerUri);
            //HTTP Request Route & Method
            var request = new RestRequest("/user/unBlockUser", Method.POST);

            string jsonToSend = new JObject(new JProperty("username", MainUser.mainUser.userName),
                                             new JProperty("unblock", thisUser.userName)
                                             ).ToString();

            request.AddParameter("application/json; charset=utf-8", jsonToSend, ParameterType.RequestBody);
            request.RequestFormat = RestSharp.DataFormat.Json;

            client.ExecuteAsync(request, response =>
            {
                try
                {
                    JObject JSResponse = JObject.Parse(response.Content);
                    if ((bool)JSResponse["status"])
                    {
                        //Update MainUser Object:
                        MainUser.UnBlock_Friend(thisUser);

                        var foundedUser = MainUser.mainUser.Friends.Find(p => p.userName == thisUser.userName);
                        if (foundedUser != null)
                        {
                            this.Dispatcher.Invoke(() =>
                            {
                                this.Block_Btn.IsEnabled = true;
                                this.UnBlock_Btn.IsEnabled = false;

                                this.Delete_Btn.IsEnabled = true;
                                this.Add_Btn.IsEnabled = false;
                            });
                        }
                        else
                        {
                            this.Dispatcher.Invoke(() =>
                            {
                                this.Block_Btn.IsEnabled = true;
                                this.UnBlock_Btn.IsEnabled = false;

                                this.Delete_Btn.IsEnabled = false;
                                this.Add_Btn.IsEnabled = true;
                            });
                        }
                    }
                    else
                    { Console.WriteLine("Status False => RES : " + response.Content); }
                }
                catch (JsonReaderException error)
                {
                    Console.WriteLine("#ERROR in sending HTTP Request Method [UnBlock Friend]: " + error.Message);
                }
            });
        }
    }
}
