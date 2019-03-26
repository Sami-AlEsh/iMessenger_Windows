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
        private string img64;
        private string sentDate;

        public Event_Image(string Receiver, string img64)
        {
            this.type = "image";
            this.Receiver = Receiver;
            this.img64 = img64;
            sentDate = DateTime.Now.ToString();
        }
        public override string GetJson()
        {
            JObject json = new JObject(
                new JProperty("type",type),
                new JProperty("Reciever", Receiver),
                new JProperty("message", img64),
                new JProperty("extension",".JPG"),
                new JProperty("sentDate",sentDate)
                );
            return json.ToString();
        }

        public override byte[] GetBinaryFile()
        {
            throw new NotImplementedException();
        }
    }
}
