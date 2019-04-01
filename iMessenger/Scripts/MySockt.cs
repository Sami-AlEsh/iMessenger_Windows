/***************************************************************************************/
/*                                      TCP Socket                                     */
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
    static class MySocket
    {
        public static bool connected { get; set; }
        public static string ServerIp { set; get; }
        public static string ServerPort { set; get; }
        public static Socket socket { set; get; }


        /// <summary>
        /// TCP Socket Connect to server
        /// </summary>
        public static void Connect() //TODO Calling this function repetadliy
        {
            IPAddress ipAddr = IPAddress.Parse(ServerIp);
            IPEndPoint localEndPoint = new IPEndPoint(ipAddr, Convert.ToInt32(ServerPort));

            if(socket == null)
                socket = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                socket.Connect(localEndPoint);
                if (socket.Connected)
                {
                    Console.WriteLine("Connected");
                    connected = true;
                    new Event_Authentication().SendMessage();
                    Console.WriteLine("Socket connected to -> {0} ", socket.RemoteEndPoint.ToString());

                    //StartListening ..
                    var ServerListener = new Thread(() =>
                    {
                        Console.WriteLine("Running Thread ...");
                        while (socket.Connected)
                        {
                            //if (!MySocket.connected) break;

                            Console.WriteLine("Trying 2 Get Message");

                            // --- First Message ---
                            //Message Size:
                            byte[] MessageSize = new byte[4];
                            int byteRecv1 = socket.Receive(MessageSize, 4, SocketFlags.None);
                            if (byteRecv1 <= 0)
                            {
                                Console.WriteLine("No Recieved Message");
                                Thread.Sleep(500);
                                continue;
                            }
                            //Message Content:
                            byte[] MessageReceived = new byte[BitConverter.ToInt32(MessageSize, 0)];
                            int byteRecv2 = socket.Receive(MessageReceived, BitConverter.ToInt32(MessageSize, 0), SocketFlags.None);

                            Console.WriteLine("New Message Recieved ! Handling....");
                            //Handling Message
                            try
                            {
                                //string JsonString = BitConverter.ToString(MessageReceived);
                                string JsonString = Encoding.UTF8.GetString(MessageReceived);
                                Console.WriteLine(JsonString);
                                JObject JMessagRecieved = JObject.Parse(JsonString);
                                string Type = JMessagRecieved.SelectToken("type").Value<string>();
                                Console.WriteLine("Message Type -> " + Type);
                                if (Type != "Text")
                                {
                                    // Type => BinaryFile [ Image, Audio ,Binary ..]

                                }
                                else
                                {
                                    //Type => TEXT
                                }
                            }
                            catch (Exception e)
                            {

                            }

                            // --- Second Message ---



                            //Task messageParser = Task.Factory.StartNew(() => MessageParser(MessageReceived));
                        }

                        //While loop has been breaked up :
                        Console.WriteLine("Server Listener Stopped (Connection Lost) !");
                    });
                    ServerListener.Start();
                }
                else
                {
                    Console.WriteLine("Socket failed to connect !");
                    //TODO every X seconds Call this Method"Connect" to check for connection !
                }
            }
            // Manage of Socket's Exceptions 
            catch (ArgumentNullException ane)
            {
                Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
            }

            catch (SocketException se)
            {

                Console.WriteLine("SocketException : {0}", se.ToString());
            }

            catch (Exception e)
            {
                Console.WriteLine("Unexpected exception : {0}", e.ToString());
            }

        }

        //<EOM> Method
        //public static void SendJson(string JsonData)
        //{
        //    JsonData += "<EOM>"; //Adding <EOM> to prevent Messages overlap.
        //    byte[] messageSent = Encoding.UTF8.GetBytes(JsonData);
        //    int byteSent = socket.Send(messageSent);
        //    Console.WriteLine("Message Sent !");
        //}
        private static int GetBinaryFileSize(object obj)
        {
            int size = 0;
            using (Stream s = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(s, obj);
                Console.WriteLine("file length" + Convert.ToInt32(s.Length));
                return size = Convert.ToInt32(s.Length);
            }
        }
        private static byte[] AddBytesArray(byte[] a1, byte[] a2)
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
        public static void SendBinary(byte[] file)
        {
            byte[] MessageSize = BitConverter.GetBytes(GetBinaryFileSize(file));
            byte[] messageSent = AddBytesArray(MessageSize, file);
            int byteSent = socket.Send(messageSent);
            Console.WriteLine("Message Sent !");
        }
        public static void SendJson(string JsonData)
        {
            //byte[] MessageSize = BitConverter.GetBytes(GetBinaryFileSize(JsonData)); //ERROR
            byte[] MessageSize = BitConverter.GetBytes(Convert.ToInt32(Encoding.UTF8.GetBytes(JsonData).Length));
            Console.WriteLine(BitConverter.ToInt32(MessageSize, 0));
            byte[] messageSent = AddBytesArray(MessageSize, Encoding.UTF8.GetBytes(JsonData));
            int byteSent = socket.Send(messageSent);
            Console.WriteLine("Message Sent !");
        }
        public static void SendJsonEOM(string JsonData)
        {
            JsonData += "<EOM>"; //Adding <EOM> to prevent Messages overlap.
            byte[] messageSent = Encoding.UTF8.GetBytes(JsonData);
            int byteSent = socket.Send(messageSent);
            Console.WriteLine("Message Sent !");
        }


        public static void RecieveJson(string JsonData)
        {
            //byte[] MessageSize = BitConverter.GetBytes(GetBinaryFileSize(JsonData));
            //byte[] messageSent = AddBytesArray(MessageSize, Encoding.UTF8.GetBytes(JsonData));
            //int byteSent = socket.Send(messageSent);
            //Console.WriteLine("Message Sent !");
        }
    }
}