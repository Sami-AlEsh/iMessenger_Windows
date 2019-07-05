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

            var ServerUri = new Uri("https://" + MyTcpSocket.ServerIp );

            var client = new RestClient(ServerUri);
            //HTTP Request Route & Method
            var request = new RestRequest("/user/search/" + userName_Email, Method.GET);
            
            try
            {
                client.ExecuteAsync(request, response =>
                {
                    try
                    {
                        JObject JsonResponse = JObject.Parse(response.Content);

                        if ((bool)JsonResponse["status"])
                        {
                            Update_Search_UI(JsonResponse);
                        }
                        else
                        {
                            Console.WriteLine("HTTP Search Friend Request failed # Status:false ");
                        }
                    }
                    catch (JsonReaderException error)
                    {
                        Console.WriteLine("#ERROR in sending HTTP Request Search_Method [JSON Parser Error]: " + error.Message);
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
    }
}
