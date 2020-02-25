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
using Newtonsoft.Json;
using iMessenger.Scripts.AES;
using iMessenger.Scripts.RSA;
using System.Security.Cryptography;
using System.Xml;

namespace iMessenger
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Attributes :
        //public MainUser mainUser = new MainUser();


        //Methods:
        public MainWindow()
        {
            InitializeComponent();
            //OnStartUp();

            //Test1(); User Auth
            //Test2(); Save Image
            //ImageSendd();
            //HttpReq();
            //RSA();
        }
        private void OnStartUp()
        {
            //var key = AESOperation.GenerateKey();
            //string key = "2iTd8F35SLK5VBAfwV6Ulg==";
            Console.WriteLine();



            //var stringEncrypted = AESOperation.Encrypt(key, "Done Bro !");
            //Console.WriteLine(stringEncrypted);
            ////var decryptedString = AESOperation.Decrypt(key, stringEncrypted);
            //var decryptedString = AESOperation.Decrypt(key, "xw0NSe22U6r9t7sn8txqSA==");
            //Console.WriteLine(decryptedString);
            /////////////////////////////////////////////

            //var r = File.ReadAllBytes(Project.Path + @"/aa.zip");
            //var EncryptedFile = AESOperation.Encrypt(key, r);
            //File.WriteAllBytes(Project.Path + @"/2Encrypted.zip", EncryptedFile);

            //var theFile = AESOperation.Decrypt(key, EncryptedFile);
            //File.WriteAllBytes(Project.Path + @"/3Decrypted.zip", theFile);
            ////////////////////////////////////////////////
            RSA_keys k = new RSA_keys();
            Console.WriteLine("#1# " + Convert.ToBase64String(k.publicKey.Modulus) + "\n$\n" +  Convert.ToBase64String(k.publicKey.Exponent) );
            Console.WriteLine("#2# " + Convert.ToBase64String(k.privateKey.Modulus) + "\n$\n" + Convert.ToBase64String(k.privateKey.Exponent));
            RSA_keys.StoreKeys(k);

            k = RSA_keys.GetKeys();
            Console.WriteLine("#11# " + Convert.ToBase64String(k.publicKey.Modulus ) + "\n$\n" + Convert.ToBase64String(k.publicKey.Exponent) );
            Console.WriteLine("#22# " + Convert.ToBase64String(k.privateKey.Modulus) + "\n$\n" + Convert.ToBase64String(k.privateKey.Exponent));

            //var moduPUB =Convert.FromBase64String("27lKXjVyI86AVt8eSdBJD2aI0pI+WJmizRfXUTclA6/UokZPvsG9wdd6rHpZ0FEQk13i5oPQvJJZFAuFs4vZhbqfzAJkg+/0v97D9QUGbi7BTRmkOYDroy6pSywUKJg0SiwGIaxk0uKG4x6d/EHJjIqfMh9wym3HSfi7zvGdeyYmLihLjXtB4PzHaWCvlJjBZKZPnJStVp/WSNETHS0XvMb8UTMqe0Gx/ZE8c0gDk8iANlFUrXgq5DFtixVcjpxVcIJWmjnCFkcS7v5uultTiBZ9etDXuli3w+bT8jJ+Mk50jUzWO1o5HFEavDDJpfvK8NLFIqg4ZuVROizetw/o9Q=="); 
            //var moduPriv=Convert.FromBase64String("27lKXjVyI86AVt8eSdBJD2aI0pI+WJmizRfXUTclA6/UokZPvsG9wdd6rHpZ0FEQk13i5oPQvJJZFAuFs4vZhbqfzAJkg+/0v97D9QUGbi7BTRmkOYDroy6pSywUKJg0SiwGIaxk0uKG4x6d/EHJjIqfMh9wym3HSfi7zvGdeyYmLihLjXtB4PzHaWCvlJjBZKZPnJStVp/WSNETHS0XvMb8UTMqe0Gx/ZE8c0gDk8iANlFUrXgq5DFtixVcjpxVcIJWmjnCFkcS7v5uultTiBZ9etDXuli3w+bT8jJ+Mk50jUzWO1o5HFEavDDJpfvK8NLFIqg4ZuVROizetw/o9Q==");
            //var expPUB  =Convert.FromBase64String("AQAB");
            //var expPriv = Convert.FromBase64String("AQAB");   

            //Console.WriteLine("PUBKEY :\n");
            //Console.WriteLine("#1# ==>"+Convert.ToBase64String(moduPUB));
            //Console.WriteLine("#2# ==>"+Convert.ToBase64String(expPUB));
            //Console.WriteLine("\nPriKEY :\n");
            //Console.WriteLine("#1# ==>"+Convert.ToBase64String(moduPriv));
            //Console.WriteLine("#2# ==>"+Convert.ToBase64String(expPriv));
            //var enc = RSAOperation.Encryption(Encoding.UTF8.GetBytes("Hello to RSA"), keys.PUBLIC_Key);
            //var dec = RSAOperation.Decryption(enc, keys.PRIVATE_Key);
            //Console.WriteLine(Encoding.UTF8.GetString(dec));
            //Console.WriteLine(Convert.ToBase64String(enc));

            //RSA_keys keys = new RSA_keys("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");
            //var enc = Convert.FromBase64String("fVpS99mH1SoXv60hIJ69668L6WQFg7Saqv08tj9DVz5Wl8YxWf4Lt5KdLQXOkP7lHXmnCaabNbZCNHU0KcUKbShzxO4mHitKVFyy/sNDXjI7IuCYeFcsUySgK4304xCslUmq68U7venw6edUDJiQlhcJOPeS0Mugto8ggbn+hnNDf2yz+8VqlqIt2epBgWyyLiVSdi45RMD1EKiyqGnobvNQkc/kdWk0dKPFmkh/z0wG2/vags1Gi+x9Uca5F/ysuYmsqhyx7Q4LtFXRCCu74k42SXPas0Cz5nWw8dCb9gkb0xNZkJU1AE4ETzxnlFHVEDDyn03LbbausmZc4MRViA==");
            //var dec = RSAOperation.Decryption(enc, keys.PRIVATE_Key);
            //Console.WriteLine(Encoding.UTF8.GetString(dec));


            //RSA_keys keys = new RSA_keys();
            //byte[] encData = RSAOperation.Encryption(Encoding.UTF8.GetBytes("In the name of allah"), keys.PRIVATE_Key);


            //RSA_keys Nkeys = new RSA_keys(File.ReadAllText("PublicKey.dat"), File.ReadAllText("PrivateKey.dat"));

            //Console.WriteLine(Encoding.UTF8.GetString(RSAOperation.Decryption(Convert.FromBase64String(
            //    "RbRCLGH4k3n8raHkaj10xkQOL9jWok0XPaF8KJ3yqi5P7qvy1ivP6ZBUEmoQmimZmO8VwchEqenX/bNexD3pChdJHGgKOXJ4u5lgDG/ZdLFjeZ8k/+Zam97mXjUweHHgKr8yAEqCfP8OUphqfpUjydEFz8y4WprURtFocGywGhrKJqZ6oHSwTHFteoNdVSHuh0UHkhB/E1yVZsMmksKasoLDw5WhZLxcjclkWzf55fHI2qphc8nMZmjo6SJBqXLt4npT9Y6lbJOZjdq+4m6799W8kXFczhN4ESHPZcnMnkeZaSoMI8AGoe1p0ud7DteTly2ixYUE7Cq5w660jOGzWg=="
            //    ),Nkeys.PRIVATE_Key)));


            //var key = Convert.FromBase64String(
            //    ""
            //    );
            //RSA_keys.DecodeX509PublicKey(key).Decrypt(, false);
            //Console.WriteLine();

            //RSA_keys r = new RSA_keys();
            //Console.WriteLine(RSA_keys.GetJavaKey(r.PUBLIC_Key));



            //mainUser = MainUser.LoadLocalMainUser();
            //if (mainUser != null && mainUser.verified)
            //{
            //    /* if (mainUser.verified) -> Show Messenger Window
            //     * else -> Show (Sign up/Log in) Window
            //     * this Check is useful for case "User tried to modify AccessToken So we Checked it"
            //     */
            //}
            //else
            //{
            //}
        }


        private void Test4()
        {
            JObject videogameRatings = new JObject(
            new JProperty("Halo", 9),
            new JProperty("Starcraft", 9),
            new JProperty("Call of Duty", 7.5));

            Directory.CreateDirectory(Project.Path + @"/Database/sami98");
            File.WriteAllText(Project.Path + @"/Database/sami98/chat1.json", videogameRatings.ToString());

            // write JSON directly to a file
            using (StreamWriter file = File.CreateText(Project.Path + @"/Database/sami98/chat2.json"))
            using (JsonTextWriter writer = new JsonTextWriter(file))
            {
                videogameRatings.WriteTo(writer);
            }
        }

        //private MainUser GetMainUserDetails()
        //{
        //    if (File.Exists(ProjectPath + @"\MainUser\MainUser.binary"))
        //    {
        //        return null;
        //    }
        //    else
        //    {
        //        Console.WriteLine("No Registered User !");
        //        return new MainUser("Null", "Null", "Null","Null");
        //    }
        //}
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

                //new Event_Image("Tareq", base64String).SendMessage();
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
            OnStartUp();
            //MyTcpSocket.ServerIp = this.Serverip_textBox.Text;
            //MyTcpSocket.ServerPort = Convert.ToInt32(this.ServerPort_textBox.Text);
            //new MyTcpSocket().Connect();
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            //new Event_Text("Alaa",this.MessageBox.Text).SendMessage();
            //new Event_BinaryFile("sami98", @"D:\Desktop.rar", "rar").SendMessage();
            //new Event_Image("tareq", @"D:\S.JPG").SendMessage();
        }
    }
}
