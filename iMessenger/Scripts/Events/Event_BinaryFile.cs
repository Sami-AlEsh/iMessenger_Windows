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
            if (File.ReadAllBytes(this.FilePath).Length == 0) Console.WriteLine("SSSSSSSS");
            return File.ReadAllBytes(this.FilePath);
        }
    }
}
