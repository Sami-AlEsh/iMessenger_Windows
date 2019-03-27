using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
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
            this.type = "image";
            this.Receiver = Receiver;
            this.FilePath = FilePath;
            // TODO Extract it from FilePath
            this.Extension = ".JPG";
            this.SentDate = DateTime.Now.ToString();
        }
        public override string GetJson()
        {
            JObject json = new JObject(
                new JProperty("type",type),
                new JProperty("Reciever", Receiver),
                //new JProperty("message", img64),
                new JProperty("extension",Extension),
                new JProperty("sentDate",SentDate)
                );
            return json.ToString();
        }

        public override byte[] GetBytes()
        {
            throw new NotImplementedException();
        }
    }
}
