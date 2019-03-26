///***************************************************************************************/
///*                                      TCP Socket                                     */
///***************************************************************************************/

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Net.Sockets;
//using System.Net;
//using System.IO;
//using System.Runtime.Serialization.Formatters.Binary;

//namespace iMessenger.Scripts
//{
//    static class MySockt
//    {
//        public static bool connected { get; set; }
//        public static string ServerIp { set; get; }
//        public static string ServerPort { set; get; }
//        public static Socket socket { set; get; }


//        /// <summary>
//        /// TCP Socket Connect to server
//        /// </summary>
//        public static void Connect()
//        {
//            IPAddress ipAddr = IPAddress.Parse(ServerIp);
//            IPEndPoint localEndPoint = new IPEndPoint(ipAddr, Convert.ToInt32(ServerPort));

//            socket = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

//            try
//            {
//                socket.Connect(localEndPoint);
//                Console.WriteLine("Socket connected to -> {0} ", socket.RemoteEndPoint.ToString());
//                connected = true;
//            }
//            // Manage of Socket's Exceptions 
//            catch (ArgumentNullException ane)
//            {
//                Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
//            }

//            catch (SocketException se)
//            {

//                Console.WriteLine("SocketException : {0}", se.ToString());
//            }

//            catch (Exception e)
//            {
//                Console.WriteLine("Unexpected exception : {0}", e.ToString());
//            }

//        }
        
//        //<EOM> Method
//        //public static void SendJson(string JsonData)
//        //{
//        //    JsonData += "<EOM>"; //Adding <EOM> to prevent Messages overlap.
//        //    byte[] messageSent = Encoding.UTF8.GetBytes(JsonData);
//        //    int byteSent = socket.Send(messageSent);
//        //    Console.WriteLine("Message Sent !");
//        //}
//        private static long GetBinaryFileSize(object obj)
//        {
//            long size = 0;
//            using (Stream s = new MemoryStream())
//            {
//                BinaryFormatter formatter = new BinaryFormatter();
//                formatter.Serialize(s, obj);
//                return size = s.Length;
//            }
//        }
//        private static byte[] AddBytesArray(byte[] a1 , byte[] a2)
//        {
//            byte[] rv = new byte[a1.Length + a2.Length];
//            System.Buffer.BlockCopy(a1, 0, rv, 0, a1.Length);
//            System.Buffer.BlockCopy(a2, 0, rv, a1.Length, a2.Length);
//            return rv;
//        }


//        /// <summary>
//        /// Send Binary File after adding 8 Bytes (FileSize) 
//        /// </summary>
//        /// <param name="file"></param>
//        public static void SendBinary(byte[] file)
//        {
//            byte[] MessageSize = BitConverter.GetBytes(GetBinaryFileSize(file));
//            byte[] messageSent = AddBytesArray(MessageSize , file);
//            int byteSent = socket.Send(messageSent);
//            Console.WriteLine("Message Sent !");
//        }
//        public static void SendJson(string JsonData)
//        {
//            byte[] MessageSize = BitConverter.GetBytes(GetBinaryFileSize(JsonData));
//            byte[] messageSent = AddBytesArray(MessageSize, Encoding.UTF8.GetBytes(JsonData));
//            int byteSent = socket.Send(messageSent);
//            Console.WriteLine("Message Sent !");
//        }
//        public static void SendJsonEOM(string JsonData)
//        {
//            JsonData += "<EOM>"; //Adding <EOM> to prevent Messages overlap.
//            byte[] messageSent = Encoding.UTF8.GetBytes(JsonData);
//            int byteSent = socket.Send(messageSent);
//            Console.WriteLine("Message Sent !");
//        }
//    }
//}