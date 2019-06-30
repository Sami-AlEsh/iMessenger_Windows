using iMessenger.Scripts;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

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
                                                 new JProperty("password", Password.Text)
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
                        }
                        catch (JsonReaderException error)
                        {
                            Running = false;
                            this.Dispatcher.Invoke(() => { Signup_Login_Btn.IsEnabled = true; Signup_Login_Btn.Content = "Log in"; });
                            Console.WriteLine("#ERROR in sending HTTP Request Method [JSON Parser Error]: " + error.Message);
                        }
                    
                        if ((bool)JsonResponse["status"])
                        {
                            Running = false;
                            var token = (string)JsonResponse["data"];
                            //TODO Take : Name,Email,Friends from Response (modify MainUser Constructoe to add Friends)
                            this.Dispatcher.Invoke(() => {
                                MainUser.mainUser = new MainUser("sami1", UserName.Text, "LoginNoEmailSAMI1", token);
                                MainUser.UpdateFriendsList();
                            });
                            this.Dispatcher.Invoke(() => Signup_LoginWindow.SwitchPage(ApplicationPage.chat));
                            Console.WriteLine("Server Response Token ==> " + token);
                        }
                        else
                        {
                            Running = false;
                            this.Dispatcher.Invoke(() => { Signup_Login_Btn.IsEnabled = true; Signup_Login_Btn.Content = "Log in"; });
                            Console.WriteLine("HTTP Request failed # Error MSG => " + response.Content);
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
                                                    new JProperty("email", Email.Text)
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
                        }
                        catch (JsonReaderException error)
                        {
                            Running = false;
                            this.Dispatcher.Invoke(() => { Signup_Login_Btn.IsEnabled = true; Signup_Login_Btn.Content = "Sign Up"; });
                            Console.WriteLine("#ERROR in sending HTTP Request Method [JSON Parser Error]: " + error.Message);
                        }

                        if ((bool)JsonResponse["status"])
                        {
                            Running = false;
                            var token = (string)JsonResponse["data"];
                            this.Dispatcher.Invoke(() => {
                                MainUser.mainUser = new MainUser(Name.Text, UserName.Text, Email.Text, token);
                                MainUser.SaveLocalMainUserJS();
                            });
                            this.Dispatcher.Invoke(() => Signup_LoginWindow.SwitchPage(ApplicationPage.chat));
                            Console.WriteLine("Server Response Token ==> " + token);
                        }
                        else
                        {
                            Running = false;
                            this.Dispatcher.Invoke(() => { Signup_Login_Btn.IsEnabled = true; Signup_Login_Btn.Content = "Sign Up"; });
                            Console.WriteLine("HTTP Request failed # Error MSG => " + response.Content);
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
