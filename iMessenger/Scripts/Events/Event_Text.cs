using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iMessenger.Scripts.Events
{
    class Event_Text : Message
    {
        private string Receiver;
        private string text;
        private string SentDate;

        public Event_Text(string Receiver, string text)
        {
            this.type = "Text";
            this.Receiver = Receiver;
            this.text = text;
            SentDate = DateTime.Now.ToString();
            
        }
        public override string GetJson()
        {
            JObject Jobj = new JObject(
                new JProperty("type", type),
                new JProperty("receiver",Receiver) ,
                new JProperty("message", text) ,
                new JProperty("SentDate", SentDate)
                );

            return Jobj.ToString();
        }

        public override byte[] GetBytes()
        {
            throw new NotImplementedException();
        }
    }
}
