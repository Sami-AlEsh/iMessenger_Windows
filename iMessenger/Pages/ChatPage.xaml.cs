using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace iMessenger.Pages
{
    /// <summary>
    /// Interaction logic for ChatPage.xaml
    /// </summary>
    public partial class ChatPage : Page
    {
        public ChatPage()
        {
            InitializeComponent();
        }
        
        void SendHttpReq_SignUp()
        {
            //var client = new RestClient("http://192.168.43.56:3002");
            //var request = new RestRequest("/signup/", Method.POST);

            //string jsonToSend = new JObject(
            //    new JProperty("name", Name.Text),
            //    new JProperty("username", UserName.Text), 
            //    new JProperty("password", Password.Password), 
            //    new JProperty("email", Email.Text) ).ToString();

            //request.AddParameter("application/json; charset=utf-8", jsonToSend, ParameterType. RequestBody);
            //request.RequestFormat = RestSharp.DataFormat.Json;
            //try
            //{
            //    client.ExecuteAsync(request, response =>
            //    {
            //        if (response.StatusCode == HttpStatusCode.OK)
            //        {
            //            Console.WriteLine("Response ==> " + response.Content);
            //        }
            //        else
            //        {
            //            Console.WriteLine("REQ failed");
            //        }
            //    });
            //}
            //catch (Exception error)
            //{
            //    Console.WriteLine("ERROR 404! : " + error.Message);
            //}
        }
    }
}
