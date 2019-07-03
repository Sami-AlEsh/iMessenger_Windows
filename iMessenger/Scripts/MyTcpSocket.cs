/***************************************************************************************/
/*                                      TCP Client Socket                                     */
/***************************************************************************************/

using System;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using iMessenger.Scripts.Events;
using System.Windows;

namespace iMessenger.Scripts
{
    class MyTcpSocket
    {
        #region public property

        //Server IP
        public static string ServerIp { set; get; } = "192.168.43.56";

        //Server Port
        public static int ServerPort { set; get; } = 3001;

        //My Socket
        public static TcpClient clientsocket  { set; get; }

        #endregion

        #region private property

        NetworkStream stream = null;
        private static BinaryReader reader = null;
        private static BinaryWriter writer = null;

        private Task Connection = null;
        private Task ServerListener = null;

        #endregion

        /// <summary>
        /// TCP Socket Connect to server
        /// </summary>
        public void Connect()
        {
            Connection = Task.Factory.StartNew(() =>
            {
                Console.WriteLine("Trying to Connect ... ");

                while ( (clientsocket == null) || (clientsocket != null && !clientsocket.Connected))
                {
                    try
                    {
                        //Connect :
                        clientsocket = new TcpClient(ServerIp, ServerPort);
                        clientsocket.SendTimeout = 2;

                        //Authentication :
                        new Event_Authentication(MainUser.mainUser.AccessToken).SendMessage();

                        ServerListener = Task.Factory.StartNew(() =>
                        {
                            Console.WriteLine("Server Listener is Running ...");
                            stream = clientsocket.GetStream();

                            if (!clientsocket.Connected) Task.Delay(2000);
                            while (clientsocket.Connected)
                            {
                                if (stream != null)
                                {
                                    reader = new BinaryReader(stream);
                                    try
                                    {
                                        var RecMessage = reader.ReadBytes(BitConverter.ToInt32(reader.ReadBytes(4), 0));
                                        JObject JsonMessage = JObject.Parse(Encoding.UTF8.GetString(RecMessage));
                                        Console.WriteLine("###  Message Recieced  ### => "+JsonMessage);
                                        switch ((string)JsonMessage["type"])
                                        {
                                            case "Text":
                                                {
                                                    var TextMessage = JsonMessage;
                                                    new Event_Text(TextMessage).Event_Text_Handler(true);
                                                    break;
                                                }
                                            case "Image":
                                                {
                                                    var ImageMessage = JsonMessage;
                                                    var RecImage = reader.ReadBytes(BitConverter.ToInt32(reader.ReadBytes(4), 0));
                                                    new Event_Image(ImageMessage, RecImage).Event_Image_Handler();
                                                    break;
                                                }
                                            case "BinaryFile":
                                                {
                                                    var BFMessage = JsonMessage;
                                                    var RecBinaryFile = reader.ReadBytes(BitConverter.ToInt32(reader.ReadBytes(4), 0));
                                                    new Event_BinaryFile(BFMessage, RecBinaryFile).Event_BinaryFile_Handler();
                                                    break;
                                                }
                                            case "VoiceCall":
                                                {
                                                    var VoipMessage = JsonMessage;
                                                    new Event_VoiceCall(VoipMessage);
                                                    break;
                                                }
                                            case "updateSecretKey":
                                                {
                                                    var SecretKeyNotification = JsonMessage;
                                                    new Event_UpdateSecretKey(SecretKeyNotification);
                                                    break;
                                                }
                                            case "new friend":
                                                {
                                                    Console.WriteLine("## New Friend !");
                                                    var n_JSUser = JsonMessage;
                                                    User user = new User("", (string)n_JSUser["from"], "");
                                                    Application.Current.Dispatcher.Invoke(() => MainUser.AddFriend(user));
                                                    break;
                                                }
                                            default:
                                                {
                                                    Console.WriteLine("Unknown Message Type : " + JsonMessage);
                                                    break;
                                                }
                                        }
                                    }
                                    catch (JsonReaderException exp)
                                    {
                                        Console.WriteLine("JSON isn't valid => " + exp.Message);
                                    }
                                    catch (IOException)
                                    {
                                        Console.WriteLine("# Application Closed => Stream Closed #");
                                    }
                                }
                                Task.Delay(500);
                            }
                            Connect();
                        });

                        //Send offline Messages:
                        var messages = MainUser.mainUser.ChatsQueue.ToArray();
                        foreach (var msg in messages)
                        {
                            msg.SendMessage();
                        }
                    }
                    catch (SocketException e)
                    {
                        Console.WriteLine("Socket Exception Happen here ==> " + e.Message);
                        //Connect();
                    }
                    Task.Delay(3000);
                }

                Console.WriteLine("Connected Successfuly !");
            });
        }
            
        
        private byte[] AddBytesArray(byte[] a1, byte[] a2)
        {
            byte[] rv = new byte[a1.Length + a2.Length];
            System.Buffer.BlockCopy(a1, 0, rv, 0, a1.Length);
            System.Buffer.BlockCopy(a2, 0, rv, a1.Length, a2.Length);
            return rv;
        }

        /// <summary>
        /// Send Binary File after adding 8 Bytes (FileSize) 
        /// </summary>
        /// <param name="file"></param>
        public void SendBinary(byte[] file)
        {
            byte[] MessageSize = BitConverter.GetBytes(file.Length);
            byte[] messageSent = AddBytesArray(MessageSize, file);
            try
            {
                stream = clientsocket.GetStream();
                stream.Flush();
                writer = new BinaryWriter(stream);
                writer.Write(messageSent);
                Console.WriteLine("Message Sent !");
            }
            catch (WebException e)
            {
                Console.WriteLine("Failed to send message ==> " + e.Message);
            }
        }
        public void SendJson(string JsonData)
        {
            byte[] MessageSize = BitConverter.GetBytes(Convert.ToInt32(Encoding.UTF8.GetBytes(JsonData).Length));
            Console.WriteLine(BitConverter.ToInt32(MessageSize, 0));
            byte[] messageSent = AddBytesArray(MessageSize, Encoding.UTF8.GetBytes(JsonData));
            try
            {
                stream = clientsocket.GetStream();
                stream.Flush();
                writer = new BinaryWriter(stream);
                writer.Write(messageSent);
                
                Console.WriteLine("Message Sent !");
            }
            catch (Exception)
            {
                throw new Exception("Message not sent !");
            }
        }

    }
}