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
            
            #region HTTP Friend Search Request

            var ServerUri = new Uri("http://" + MyTcpSocket.ServerIp + ":" + "8080");

            var client = new RestClient(ServerUri);
            //HTTP Request Route & Method
            //var request = new RestRequest("/index/___/", Method.___);

            //string jsonToSend = new JObject( new JProperty("name", Name.Text),
            //                                 new JProperty("username", UserName.Text),
            //                                 new JProperty("password", Password.Text),
            //                                 new JProperty("email", Email.Text)
            //                                 ).ToString();

            //request.AddParameter("application/json; charset=utf-8", jsonToSend, ParameterType.RequestBody);
            //request.RequestFormat = RestSharp.DataFormat.Json;

            #endregion
        }

        private void addFoundedFriend(UserControl UC)
        {
            this.Dispatcher.Invoke(() => this.FrindsSearchList.Children.Add(UC));
        }
    }
}
