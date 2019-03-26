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
    abstract class Message
    {
        protected string type;
        //TODO Add AccessToken

        /// <summary>
        /// Return Message as JSON
        /// </summary>
        /// <returns></returns>
        public abstract string GetJson();

        public abstract byte[] GetBinaryFile();
        

        public void SendMessage()
        {
            Console.WriteLine("Sending Message ...");
            try
            {
                //Binary Data:
                if (this.type == "BinaryFile")
                {
                    //First Message
                    Console.WriteLine("String Message -> " + this.GetJson());
                    MyWebSocket.SendJson(this.GetJson());

                    //Second Message
                    Console.WriteLine("# Binary Message #");
                    MyWebSocket.SendBinary(this.GetBinaryFile());
                }
                //Json Data:
                else
                {
                    Console.WriteLine("String Message -> "+GetJson());
                    MyWebSocket.SendJson(this.GetJson());
                }

                Console.WriteLine("Message Sent !");
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR Happen Here : "+e.Message);
            }
        }
    }
}
