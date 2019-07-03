using System;
using RestSharp;
using Newtonsoft.Json;
using iMessenger.Scripts;
using Newtonsoft.Json.Linq;
using System.Windows.Controls;
using iMessenger.Scripts.RSA;
using iMessenger.Scripts.Events;

namespace iMessenger
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        #region private Members

        private bool LoginState = false; //SignUp:0 , Login:1 
        private bool Running = false; //SignUp_Login Running:1 , Nothing is Happening:0 

        #endregion

        public LoginPage()
        {
            InitializeComponent();
        }
        

        private void SignUp_Login_Button(object sender, System.Windows.RoutedEventArgs e)
        {
            if( LoginState == true) Login();
            else SignUp();
        }

        private void Login()
        {
            if (Running) return;
            Running = true;
            Signup_Login_Btn.IsEnabled = false;
            Signup_Login_Btn.Content = "Loading";

            if (CheckUserName() && CheckPassword())
            {
                var ServerUri = new Uri("http://" + MyTcpSocket.ServerIp + ":" + "8080");

                var client = new RestClient(ServerUri);
                //HTTP Request Route & Method
                var request = new RestRequest("/index/login/", Method.POST);

                string jsonToSend = new JObject( new JProperty("username", UserName.Text),
                                                 new JProperty("password", Password.Text),
                                                 new JProperty("platform", "windows")
                                                 ).ToString();

                request.AddParameter("application/json; charset=utf-8", jsonToSend, ParameterType.RequestBody);
                request.RequestFormat = DataFormat.Json;
                try
                {
                    client.ExecuteAsync(request, response =>
                    {
                        //Json response
                        var JsonResponse = new JObject();
                        try
                        {
                            JsonResponse = JObject.Parse(response.Content);

                            if ((bool)JsonResponse["status"])
                            {
                                Running = false;
                                var token = (string)JsonResponse["data"];

                                this.Dispatcher.Invoke(() => {
                                    MainUser.mainUser = new MainUser("sami1", UserName.Text, "LoginNoEmailSAMI1", token);
                                });

                                MainUser.UpdateFriendsList();

                                this.Dispatcher.Invoke(() => Signup_LoginWindow.SwitchPage(ApplicationPage.chat));
                                Console.WriteLine("Server Response Token ==> " + token);

                                //init all MainUser Keys
                                GenerateRSAKeys_LoadAESKeys();

                                //Upload RSA-Public Key to Server
                                UploadRSAPublicKey();

                                //Foreach friend update SecretKey
                                UpdateSecretKeysForAllFriends();
                            }
                            else
                            {
                                Running = false;
                                this.Dispatcher.Invoke(() => { Signup_Login_Btn.IsEnabled = true; Signup_Login_Btn.Content = "Log in"; });
                                Console.WriteLine("HTTP Request failed # Error MSG => " + response.Content);
                            }
                        }
                        catch (JsonReaderException error)
                        {
                            Running = false;
                            this.Dispatcher.Invoke(() => { Signup_Login_Btn.IsEnabled = true; Signup_Login_Btn.Content = "Log in"; });
                            Console.WriteLine("#ERROR in sending HTTP Request Method [JSON Parser Error]: " + error.Message);
                        }
                    });
                }
                catch (Exception error)
                {
                    Running = false;
                    this.Dispatcher.Invoke(() => { Signup_Login_Btn.IsEnabled = true; Signup_Login_Btn.Content = "Log in"; });
                    Console.WriteLine("#ERROR in sending HTTP Request Method: " + error.Message);
                }
            }
            else
            {
                Running = false;
                Signup_Login_Btn.IsEnabled = true;
                Signup_Login_Btn.Content = "Log in";
            }
        }
        private void SignUp()
        {
            if (Running) return;
            Running = true;
            Signup_Login_Btn.IsEnabled = false;
            Signup_Login_Btn.Content = "Loading";

            if (CheckName() && CheckUserName() && CheckEmail() && CheckPassword())
            {
                var ServerUri = new Uri("http://" + MyTcpSocket.ServerIp + ":" + "8080");

                var client = new RestClient(ServerUri);
                //HTTP Request Route & Method
                var request = new RestRequest("/index/signup/", Method.POST);

                string jsonToSend = new JObject(new JProperty("name", Name.Text),
                                                    new JProperty("username", UserName.Text),
                                                    new JProperty("password", Password.Text),
                                                    new JProperty("email", Email.Text),
                                                    new JProperty("platform", "windows")
                                                    ).ToString();

                request.AddParameter("application/json; charset=utf-8", jsonToSend, ParameterType.RequestBody);
                request.RequestFormat = DataFormat.Json;
                try
                {

                    client.ExecuteAsync(request, response =>
                    {
                        //Json response
                        var JsonResponse = new JObject();
                        try
                        {
                            JsonResponse = JObject.Parse(response.Content);

                            if ((bool)JsonResponse["status"])
                            {
                                Running = false;
                                var token = (string)JsonResponse["data"];

                                this.Dispatcher.Invoke(() => MainUser.mainUser = new MainUser(Name.Text, UserName.Text, Email.Text, token));
                                
                                this.Dispatcher.Invoke(() => Signup_LoginWindow.SwitchPage(ApplicationPage.chat));
                                Console.WriteLine("Server Response Token ==> " + token);

                                //init all MainUser Keys
                                GenerateRSAKeys_LoadAESKeys();

                                //Upload RSA-Public Key to Server
                                UploadRSAPublicKey();
                            }
                            else
                            {
                                Running = false;
                                this.Dispatcher.Invoke(() => { Signup_Login_Btn.IsEnabled = true; Signup_Login_Btn.Content = "Sign Up"; });
                                Console.WriteLine("HTTP Request failed # Error MSG => " + response.Content);
                            }
                        }
                        catch (JsonReaderException error)
                        {
                            Running = false;
                            this.Dispatcher.Invoke(() => { Signup_Login_Btn.IsEnabled = true; Signup_Login_Btn.Content = "Sign Up"; });
                            Console.WriteLine("#ERROR in sending HTTP Request Method [JSON Parser Error]: " + error.Message);
                        }
                    });
                }
                catch (Exception error)
                {
                    Running = false;
                    this.Dispatcher.Invoke(() => { Signup_Login_Btn.IsEnabled = true; Signup_Login_Btn.Content = "Sign Up"; });
                    Console.WriteLine("#ERROR in sending HTTP Request Method: " + error.Message);
                }
            }
            else
            {
                Running = false;
                Signup_Login_Btn.IsEnabled = true;
                Signup_Login_Btn.Content = "Sign Up";
            }
        }

        #region Other Functions

        private bool CheckName()
        {
            if (String.IsNullOrEmpty(this.Name.Text)) { Name.Text = "#ERROR"; return false; }

            if (Name.Text.Contains(" ")) { Name.Text = "#ERROR"; return false; }
            else return true;
        }
        private bool CheckUserName()
        {
            if (String.IsNullOrEmpty(UserName.Text)) { UserName.Text = "#ERROR"; return false; }

            if (UserName.Text.Contains(" ")) { UserName.Text = "#ERROR"; return false; }
            else return true;
        }
        private bool CheckEmail()
        {
            if (String.IsNullOrEmpty(Email.Text)) { Email.Text = "#ERROR"; return false; }

            if (Email.Text.Contains("@") && Email.Text.Contains(".com")) return true;
            else { Email.Text = "#ERROR"; return false; }
        }
        private bool CheckPassword()
        {
            if (String.IsNullOrEmpty(Password.Text)) { Password.Text = "#ERROR"; return false; }

            if (Password.Text.Length < 16 && Password.Text.Length > 6) return true;
            else { Password.Text = "#ERROR"; return false; }
        }
        private void GenerateRSAKeys_LoadAESKeys()
        {
            MainUser.mainUser.InitializeAllKeys();
        }
        private void UploadRSAPublicKey()
        {
            MainUser.mainUser.UploadRSAPublicKey();
        }
        private void UpdateSecretKeysForAllFriends()
        {
            //Get MyFriends PublicKeys:
            var ServerUri = new Uri("http://" + MyTcpSocket.ServerIp + ":" + "8080");

            foreach (var friend in MainUser.mainUser.Friends)
            {
                var client = new RestClient(ServerUri);
                var request = new RestRequest("/user/getPublicKeys/" + friend.userName, Method.GET);

                client.ExecuteAsync(request, response =>
                {
                    //Json response
                    var JsonResponse = new JObject();
                    try
                    {
                        JsonResponse = JObject.Parse(response.Content);

                        if ((bool)JsonResponse["status"])
                        {
                            JArray Keys = (JArray)JsonResponse["data"];
                            foreach(JObject key in Keys)
                            {
                                Console.WriteLine("### TEST ONLY ==> " + key.ToString()); //TODO Delete it
                                var platform = (string)key["platform"];

                                if (platform == "windows")
                                {
                                    var publicKey_XML = (string)key["publicKey"];
                                    var encryptedSecretKey = MainUser.mainUser.GetNewAESKeyEncryptedWith(friend.userName, publicKey_XML, Platform.Windows);
                                    new Event_UpdateSecretKey(friend.userName, Platform.Windows, encryptedSecretKey).SendMessage();
                                }
                                else if (platform == "android")
                                {
                                    var publicKey_JXML = (string)key["publicKey"];
                                    var encryptedSecretKey = MainUser.mainUser.GetNewAESKeyEncryptedWith(friend.userName, publicKey_JXML, Platform.Android);
                                    new Event_UpdateSecretKey(friend.userName, Platform.Android, encryptedSecretKey).SendMessage();
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("HTTP Request failed [GetFriendsPublicKey]# RESPONSE => " + response.Content);
                        }
                    }
                    catch (JsonReaderException)
                    {
                        Console.WriteLine("Error parsing GetFriendsPublicKey JSON Response " + JsonResponse.ToString());
                    }
                    finally
                    {
                        MainUser.mainUser.SaveSecretKeys();
                    }
                });
            }
            Console.WriteLine("AllKeys is being Updated Async !");
        }

        #endregion

        private void Switch_Click_Btn(object sender, System.Windows.RoutedEventArgs e)
        {
            if (Running) return;
            if(LoginState == false)
            {
                LoginState = true;
                Name.IsEnabled = false;
                Email.IsEnabled = false;
                Signup_Login_Btn.Content = "Log In";
                Switch_Btn.Content = "Dont have account yet ... SignUp Now !";
            }
            else //LoginState == true
            {
                LoginState = false;
                Name.IsEnabled = true;
                Email.IsEnabled = true;
                Signup_Login_Btn.Content = "Sign Up";
                Switch_Btn.Content = "I already have an account !";
            }
        }
    }
}
