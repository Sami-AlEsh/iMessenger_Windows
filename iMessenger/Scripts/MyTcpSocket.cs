/***************************************************************************************/
/*                                      TCP Client Socket                                     */
/***************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using iMessenger.Scripts.Events;

namespace iMessenger.Scripts
{
    class MyTcpSocket
    {
        #region public property

        //Server IP
        public static string ServerIp { set; get; } = "192.168.43.55";

        //Server Port
        public static int ServerPort { set; get; } = 3001;

        //My Socket
        public static TcpClient clientsocket  { set; get; }

        #endregion

        #region private property

        NetworkStream stream = null;
        //private TcpListener listener = null;
        private static BinaryReader reader = null;
        private static BinaryWriter writer = null;

        private Task Connection = null;
        private Task ServerListener = null;

        private string ProjectPath = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
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
                                    Console.WriteLine("###  Message Recieced  ###");
                                    reader = new BinaryReader(stream);
                                    try
                                    {
                                        var RecMessage = reader.ReadBytes(BitConverter.ToInt32(reader.ReadBytes(4), 0));
                                        JObject JsonMessage = JObject.Parse(Encoding.UTF8.GetString(RecMessage));
                                        switch (JsonMessage.SelectToken("type").Value<string>())
                                        {
                                            case "Text":
                                                var TextMessage = JsonMessage;
                                                new Event_Text(TextMessage).Event_Text_Handler();
                                                break;
                                            case "Image":
                                                var ImageMessage = JsonMessage;
                                                var RecImage = reader.ReadBytes(BitConverter.ToInt32(reader.ReadBytes(4), 0));
                                                //new Event_Image();
                                                break;
                                            //case "BinaryFile":
                                            //    var RecBinMessage = reader.ReadBytes(BitConverter.ToInt32(reader.ReadBytes(4), 0));
                                            //    break;

                                            default:
                                                break;
                                        }
                                    }
                                    catch (JsonReaderException exp)
                                    {
                                        Console.WriteLine("JSON isn't valid => " + exp.Message);
                                    }
                                    catch (IOException exp)
                                    {
                                        Console.WriteLine("# Application Closed => Stream Closed #");
                                    }
                                }
                                Task.Delay(500);
                            }
                        });

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
            

        //            //StartListening ..
        //            var ServerListener = new Thread(() =>
        //            {
        //                Console.WriteLine("Running Thread ...");
        //                while (clientsocket .Connected)
        //                {
        //                    //if (!MySocket.connected) break;

        //                    Console.WriteLine("Trying 2 Get Message");

        //                    // --- First Message ---
        //                    //Message Size:
        //                    byte[] MessageSize = new byte[4];
        //                    int byteRecv1 = clientsocket .Receive(MessageSize, 4, SocketFlags.None);
        //                    if (byteRecv1 <= 0)
        //                    {
        //                        Console.WriteLine("No Recieved Message");
        //                        Thread.Sleep(500);
        //                        continue;
        //                    }
        //                    //Message Content:
        //                    byte[] MessageReceived = new byte[BitConverter.ToInt32(MessageSize, 0)];
        //                    int byteRecv2 = clientsocket .Receive(MessageReceived, BitConverter.ToInt32(MessageSize, 0), SocketFlags.None);

        //                    Console.WriteLine("New Message Recieved ! Handling....");
        //                    //Handling Message
        //                    try
        //                    {
        //                        //string JsonString = BitConverter.ToString(MessageReceived);
        //                        string JsonString = Encoding.UTF8.GetString(MessageReceived);
        //                        Console.WriteLine(JsonString);
        //                        JObject JMessagRecieved = JObject.Parse(JsonString);
        //                        string Type = JMessagRecieved.SelectToken("type").Value<string>();
        //                        Console.WriteLine("Message Type -> " + Type);
        //                        if (Type != "Text")
        //                        {
        //                            // Type => BinaryFile [ Image, Audio ,Binary ..]

        //                        }
        //                        else
        //                        {
        //                            //Type => TEXT
        //                        }
        //                    }
        //                    catch (Exception e)
        //                    {

        //                    }

        //                    // --- Second Message ---



        //                    //Task messageParser = Task.Factory.StartNew(() => MessageParser(MessageReceived));
        //                }

        //                //While loop has been breaked up :
        //                Console.WriteLine("Server Listener Stopped (Connection Lost) !");
        //            });
        //            ServerListener.Start();
        //        }
        //        else
        //        {
        //            Console.WriteLine("Socket failed to connect !");
        //            //TODO every X seconds Call this Method"Connect" to check for connection !
        //        }
        //    }
        //    // Manage of Socket's Exceptions 
        //    catch (ArgumentNullException ane)
        //    {
        //        Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
        //    }

        //    catch (SocketException se)
        //    {

        //        Console.WriteLine("SocketException : {0}", se.ToString());
        //    }

        //    catch (Exception e)
        //    {
        //        Console.WriteLine("Unexpected exception : {0}", e.ToString());
        //    }

        //}

        //private int GetBinaryFileSize(object obj)
        //{
            //int size = 0;
            //using (Stream s = new MemoryStream())
            //{
            //    BinaryFormatter formatter = new BinaryFormatter();
            //    formatter.Serialize(s, obj);
            //    Console.WriteLine("file length" + Convert.ToInt32(s.Length));
            //    return size = Convert.ToInt32(s.Length);
            //}
        //}
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
            catch (WebException e)
            {
                Console.WriteLine("Failed to send message ==> " + e.Message);
            }
        }

    }
}