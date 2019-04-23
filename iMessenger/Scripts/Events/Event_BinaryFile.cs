using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iMessenger.Scripts.Events
{
    class Event_BinaryFile : Message
    {
        private string Receiver;
        private string FilePath;
        private string FileExtension;
        private string sentDate;

        public Event_BinaryFile(string Receiver, string FilePath , string FileExtension)
        {
            this.type = "BinaryFile";
            this.Receiver = Receiver;
            this.FilePath = FilePath;
            this.FileExtension = FileExtension;
            this.sentDate = DateTime.Now.ToString();

        }

        public Event_BinaryFile(JObject TextMessage, byte[] file)
        {
            var ReceiverName = TextMessage.SelectToken("receiver").Value<string>();
            var MSG_ID = TextMessage.SelectToken("ID").Value<string>();
            var Extension = TextMessage.SelectToken("extension").Value<string>();

            //Update Friend File Log
            //Check Chat.json File
            if (!File.Exists("/Database/" + ReceiverName + "/" + "Chat/Chat.json"))
            {
                string ProjectBinPath = Environment.CurrentDirectory;
                string ProjectPath = Directory.GetParent(ProjectBinPath).Parent.FullName;

                try { Directory.CreateDirectory("/Database/" + ReceiverName + "/Chat/"); }
                catch (Exception e) { Console.WriteLine("#$ Error Handling Text Message -=> " + e.Message); }
            }
            //Check /Files/ Directory
            if (!Directory.Exists("/Database/" + ReceiverName + "/Files"))
                Directory.CreateDirectory("/Database/" + ReceiverName + "/Files/");

            File.AppendAllText("/Database/" + ReceiverName + "/" + "Chat/Chat.json", TextMessage.ToString() + Environment.NewLine);
            File.WriteAllBytes("/Database/" + ReceiverName + "/Files/" + MSG_ID + "." + Extension, file);
            Console.WriteLine("#$ Message Stored Successfuly");
        }


        public override string GetJson()
        {
            JObject json = new JObject(
                new JProperty("type",type),
                new JProperty("extension", FileExtension),
                new JProperty("receiver",Receiver),
                new JProperty("sentDate",sentDate)
                );
            return json.ToString();
        }

        public override byte[] GetBytes()
        {
            if (File.ReadAllBytes(this.FilePath).Length == 0) Console.WriteLine("Selected Binary File Doesnt Exist !!");
            return File.ReadAllBytes(this.FilePath);
        }
    }
}
