using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iMessenger.Scripts.Events
{
    class Event_Image : Message
    {
        private string Receiver;
        private string FilePath;
        private string Extension;
        private string SentDate;

        public Event_Image(string Receiver, string FilePath)
        {
            this.type = "Image";
            this.Receiver = Receiver;
            this.FilePath = FilePath;
            // TODO Extract it from FilePath
            this.Extension = ".JPG";
            this.SentDate = DateTime.Now.ToString();
        }

        /// <summary>
        /// Receive Image from Server
        /// </summary>
        /// <param name="Json">Image detail</param>
        /// <param name="image">Image a Byte array</param>
        public Event_Image(JObject TextMessage, byte[] image)
        {
            var ReceiverName = TextMessage.SelectToken("receiver").Value<string>();
            var MSG_ID = TextMessage.SelectToken("ID").Value<string>();

            //Update Friend File Log
            //Check Chat.json File
            if (!File.Exists("/Database/" + ReceiverName + "/" + "Chat/Chat.json"))
            {
                //string ProjectBinPath = Environment.CurrentDirectory;
                //string ProjectPath = Directory.GetParent(ProjectBinPath).Parent.FullName;

                try { Directory.CreateDirectory("/Database/" + ReceiverName + "/Chat/"); }
                catch (Exception e) { Console.WriteLine("#$ Error Create DIR Message -=> " + e.Message); }
            }
            //Check /Files/ Directory
            if (!Directory.Exists("/Database/" + ReceiverName + "/Files"))
                Directory.CreateDirectory("/Database/" + ReceiverName + "/Files/");

            File.AppendAllText("/Database/" + ReceiverName + "/" + "Chat/Chat.json", TextMessage.ToString() + Environment.NewLine);
            File.WriteAllBytes("/Database/" + ReceiverName + "/Files/" + MSG_ID + "." + Extension, image);
            Console.WriteLine("#$ Message Stored Successfuly");
        }




        public override string GetJson()
        {
            JObject json = new JObject(
                new JProperty("type",type),
                new JProperty("reciever", Receiver),
                new JProperty("extension",Extension),
                new JProperty("sentDate",SentDate)
                );
            return json.ToString();
        }

        public override byte[] GetBytes()
        {
            if (File.ReadAllBytes(this.FilePath).Length == 0) Console.WriteLine("Selected Image Doesnt Exist !!");
            return File.ReadAllBytes(this.FilePath);
        }
    }
}
