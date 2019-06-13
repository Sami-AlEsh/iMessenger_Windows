using iMessenger.Scripts;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace iMessenger
{
    /// <summary>
    /// Interaction logic for FriendsSetting.xaml
    /// </summary>
    public partial class FriendsSetting : Window
    {
        public FriendsSetting()
        {
            InitializeComponent();
        }

        private void FriendSearch(object sender, RoutedEventArgs e)
        {
            var userName_Email = this.Frind_UserName_Email.Text;
            if (string.IsNullOrEmpty(userName_Email)) return;

            FrindsSearchList.Children.Clear();

            #region HTTP Friend Search Request

            var ServerUri = new Uri("http://" + MyTcpSocket.ServerIp + ":" + "8080");

            var client = new RestClient(ServerUri);
            //HTTP Request Route & Method
            var request = new RestRequest("/user/search/" + userName_Email, Method.GET);

            //string jsonToSend = new JObject( new JProperty("name", Name.Text),
            //                                 new JProperty("username", UserName.Text),
            //                                 new JProperty("password", Password.Text),
            //                                 new JProperty("email", Email.Text)
            //                                 ).ToString();

            //request.AddParameter("application/json; charset=utf-8", jsonToSend, ParameterType.RequestBody);
            //request.RequestFormat = RestSharp.DataFormat.Json;

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
                        //this.Dispatcher.Invoke(() => { Signup_Login_Btn.IsEnabled = true; Signup_Login_Btn.Content = "Log in"; });
                        Console.WriteLine("#ERROR in sending HTTP Request Search_Method [JSON Parser Error]: " + error.Message);
                    }

                    if ((bool)JsonResponse["status"])
                    {
                        Update_Search_UI(JsonResponse);
                    }
                    else
                    {
                        Console.WriteLine("HTTP Request failed # Status:false ");
                    }
                });
            }
            catch (Exception error)
            {
                Console.WriteLine("#ERROR in sending HTTP Request Method: " + error.Message);
            }

            #endregion
        }

        private void Update_Search_UI(JObject JsonResponse)
        {
            JArray users = (JArray)JsonResponse["data"];
            for (int i = 0; i < users.Count; i++)
            {
                var name = (string)((JObject)users[i])["name"];
                var username = (string)((JObject)users[i])["username"];
                var email = (string)((JObject)users[i])["email"];

                this.Dispatcher.Invoke(() =>
                {
                    var currUser = new User(name, username, email);
                    FrindsSearchList.Children.Add(new ChatListItemControl_search(currUser));
                });
            }
        }

        private void addFoundedFriend(UserControl UC)
        {
            this.Dispatcher.Invoke(() => this.FrindsSearchList.Children.Add(UC));
        }
    }
}
