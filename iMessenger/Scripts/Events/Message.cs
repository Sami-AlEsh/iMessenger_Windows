using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace iMessenger.Scripts.Events
{
    public abstract class Message
    {
        #region Local Message ID Counter
        private static int IDCounter = 0;
        public static int GetID() { return IDCounter++; }
        #endregion

        public string type;
        public string ID { set; get; } = "null";

        /// <summary>
        /// Return Message as JSON
        /// </summary>
        /// <returns></returns>
        public abstract string GetJson();

        public abstract byte[] GetBytes();
        

        public void SendMessage()
        {
            Console.WriteLine("Sending Message ...");
            try
            {
                //Binary Data:
                if (this.type == "BinaryFile" || this.type == "Image" || this.type == "Audio")
                {
                    //First Message
                    Console.WriteLine("String Message -> " + this.GetJson());
                    new MyTcpSocket().SendJson(this.GetJson());

                    //Second Message
                    Console.WriteLine("# Binary Message #");
                    new MyTcpSocket().SendBinary(this.GetBytes());
                }
                //Json Data:
                else
                {
                    Console.WriteLine("String Message -> "+GetJson());
                    new MyTcpSocket().SendJson(this.GetJson());
                }

            }
            catch (Exception)
            {
                Console.WriteLine("**** Message not sent ! => it stored in Queue");
                try
                {
                    MainUser.ChatsQueue.Enqueue(this);
                }
                catch(NullReferenceException)
                {
                    MainUser.ChatsQueue = new Queue<Message>();
                    MainUser.ChatsQueue.Enqueue(this);
                }
            }
        }
    }
}
