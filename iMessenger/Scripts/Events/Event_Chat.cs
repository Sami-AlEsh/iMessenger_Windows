using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iMessenger.Scripts.Events
{
    class Event_Chat : Message
    {
        private string Receiver;
        private string text;
        private string sentDate;

        public Event_Chat(string Receiver, string text)
        {
            this.type = "Text Message";
            this.Receiver = Receiver;
            this.text = text;
            sentDate = DateTime.Now.ToString();
            
        }
        public override string GetJson()
        {
            JObject Jobj = new JObject(
                new JProperty("Type", type),
                new JProperty("Receiver",Receiver) ,
                new JProperty("Text", text) ,
                new JProperty("SentDate", sentDate)
                );

            return Jobj.ToString();
        }

        public override byte[] GetBinaryFile()
        {
            throw new NotImplementedException();
        }
    }
}
