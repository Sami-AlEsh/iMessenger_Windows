using System.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Navigation;
//using System.Windows.Shapes;
using iMessenger.Scripts;
using iMessenger.Scripts.Events;
using System.IO;
using RestSharp;
using Newtonsoft.Json.Linq;
using System.Net;

namespace iMessenger
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Attributes :
        public MainUser mainUser = null;


        //Methods:
        public MainWindow()
        {
            InitializeComponent();
            OnStartUp();

            //Test1(); User Auth
            //Test2(); Save Image
            //ImageSendd();
            //HttpReq();
            //RSA();
        }
        private void OnStartUp()
        {
            mainUser = GetMainUserDetails();
            if (mainUser.verified)
            {
                //TODO: POST Request to check Program
                /* if (mainUser.verified) -> Show Messenger Window
                 * else -> Show (Sign up/Log in) Window
                 * this Check is useful for case "User tried to modify AccessToken So we Checked it"
                 */
            }
            else
            {
                //TODO: Show (Sign up/Log in) Window
            }
        }
        private MainUser GetMainUserDetails()
        {
            string ProjectBinPath = Environment.CurrentDirectory;
            string ProjectPath = Directory.GetParent(ProjectBinPath).Parent.FullName;
            if (File.Exists(ProjectPath + @"\MainUser\MainUser.binary"))
            {
                //TODO: Deserialize "MainUser" Object from the File
                return null;
            }
            else
            {
                Console.WriteLine("No Registered User !");
                return new MainUser("Null", "Null", "Null","Null");
            }
        }
        void RSA()
        {
            // Specify a "currently active folder"
            string activeDir = @"c:\testdir2";

            //Create a new subfolder under the current active folder
            string newPath = System.IO.Path.Combine(activeDir, "mySubDir");

            // Create the subfolder
            System.IO.Directory.CreateDirectory(newPath);

            // Create a new file name. This example generates a random string.
            string newFileName = System.IO.Path.GetRandomFileName();

            // Combine the new file name with the path
            newPath = System.IO.Path.Combine(newPath, newFileName);

            // Create the file and write to it.
            // DANGER: System.IO.File.Create will overwrite the file
            // if it already exists. This can occur even with random file names.
            if (!System.IO.File.Exists(newPath))
            {
                using (System.IO.FileStream fs = System.IO.File.Create(newPath))
                {
                    for (byte i = 0; i < 100; i++)
                    {
                        fs.WriteByte(i);
                    }
                }
            }

            // Read data back from the file to prove that the previous code worked.
            try
            {
                byte[] readBuffer = System.IO.File.ReadAllBytes(newPath);
                foreach (byte b in readBuffer)
                {
                    Console.WriteLine(b);
                }
            }
            catch (System.IO.IOException e)
            {
                Console.WriteLine(e.Message);
            }

            // Keep the console window open in debug mode.
            System.Console.WriteLine("Press any key to exit.");
            System.Console.ReadKey();
        }
        //void GetMessageNow()
        //{
        //    //Message Size:
        //    byte[] messageReceivedSize = new byte[4];
        //    int byteRecv = MySockt.socket.Receive(messageReceivedSize);
        //    int size = BitConverter.ToInt32(messageReceivedSize,0);
        //    Console.WriteLine("Size is ===> " + size);
        //    //MessageContent:
        //    byte[] messageReceived = new byte[size];
        //    int byteRecv2 = MySockt.socket.Receive(messageReceived);
        //    string RecMessage = Encoding.UTF8.GetString(messageReceived);
        //    Console.WriteLine("Message Recived -> " + RecMessage);
        //}
        void HttpReq()
        {
            var client = new RestClient("http://192.168.43.56:3002");
            var request = new RestRequest("/signup/", Method.POST);

            string jsonToSend = new JObject(new JProperty("username","dsgsg"), new JProperty("password", "fgg"), new JProperty("email", "asggdgdfaf@gmail.com")).ToString();

            request.AddParameter("application/json; charset=utf-8", jsonToSend, ParameterType.RequestBody);
            request.RequestFormat = RestSharp.DataFormat.Json;
            try
            {
                client.ExecuteAsync(request, response =>
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        Console.WriteLine("Response ==> "+response.Content);
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
        void ImageSendd()
        {
            Image img = Image.FromFile(@"D:\2.JPG");
            using (MemoryStream m = new MemoryStream())
            {
                img.Save(m, img.RawFormat);
                byte[] imageBytes = m.ToArray();

                // Convert byte[] to Base64 String
                string base64String = Convert.ToBase64String(imageBytes);

                new Event_Image("Tareq", base64String).SendMessage();
            }
            img.Dispose();
        }
        //void Test1()
        //{
        //    var mainUser = new MainUser("sami","sami98","s@gmail.com");
        //    if(mainUser.GetUserAuthentication() == true)
        //    {
        //        Console.WriteLine("Auth Correct");
        //    }
        //    else
        //    {
        //        Console.WriteLine("Auth Wrong");
        //    }
        //}
        void Test2()
        {
            FileInfo fileInfo = new FileInfo(@"D:\S.JPG");
            byte[] data = new byte[fileInfo.Length];

            using (FileStream fs = fileInfo.OpenRead())
            {
                fs.Read(data, 0, data.Length);
            }
            //Save the Byte Array as File.
            File.WriteAllBytes(@"D:\SS.jpg", data);
        }

        private void ConnectToServer_Click(object sender, RoutedEventArgs e)
        {
            MySocket.ServerIp = this.Serverip_textBox.Text;
            MySocket.ServerPort = this.ServerPort_textBox.Text;
            MySocket.Connect();

            //MyWebSocket.Connect();
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            new Event_Text("Alaa",this.MessageBox.Text).SendMessage();
            //new Event_BinaryFile("Nader", @"D:\Desktop.rar", "rar").SendMessage();
        }
    }
}
