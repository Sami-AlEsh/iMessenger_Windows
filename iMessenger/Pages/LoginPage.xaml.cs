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
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }
        
        private void Signup_Click(object sender, RoutedEventArgs e)
        {
            if ( CheckName()    &&
                 CheckUserName()&&
                 CheckEmail()   &&
                 CheckPassword()  )
            {
                SendHttpReq_SignUp();
            }
            else
            {
                Console.WriteLine("**** Error Happend ****");
            }
        }


        #region SignUp Checkers()

        private bool CheckName()
        {
            if (String.IsNullOrEmpty(this.Name.Text)) { Set2Error(this.Name);  return false; }

            if (this.Name.Text.Contains(" ")) { Set2Error(this.Name); return false; }
            else return true;
        }
        private bool CheckUserName()
        {
            if (String.IsNullOrEmpty(UserName.Text)) { Set2Error(UserName); return false; }

            if (UserName.Text.Contains(" ")) { Set2Error(UserName); return false; }
            else return true;
        }
        private bool CheckEmail()
        {
            if (String.IsNullOrEmpty(Email.Text)) { Set2Error(Email); return false; }

            if (Email.Text.Contains("@") && Email.Text.Contains(".com")) return true;
            else { Set2Error(Email); return false; }
        }
        private bool CheckPassword()
        {
            if (String.IsNullOrEmpty(Password.Text)) { Set2Error(Password); return false; }

            if (Password.Text.Length < 16 && Password.Text.Length > 6) return true;
            else { Set2Error(Password); return false; }
        }
        private void Set2Error(TextBox box)
        {
            box.Text = "#ERROR#";
        }
        #endregion

        void SendHttpReq_SignUp()
        {
            var client = new RestClient("http://192.168.43.56:3002");
            var request = new RestRequest("/signup/", Method.POST);

            string jsonToSend = new JObject(
                new JProperty("name", Name.Text),
                new JProperty("username", UserName.Text), 
                new JProperty("password", Password.Text), 
                new JProperty("email", Email.Text) ).ToString();

            request.AddParameter("application/json; charset=utf-8", jsonToSend, ParameterType. RequestBody);
            request.RequestFormat = RestSharp.DataFormat.Json;
            try
            {
                client.ExecuteAsync(request, response =>
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        Console.WriteLine("Response ==> " + response.Content);
                    }
                    else
                    {
                        Console.WriteLine("REQ failed");
                    }
                });
            }
            catch (Exception error)
            {
                Console.WriteLine("ERROR 404! : " + error.Message);
            }
        }
    }
}
