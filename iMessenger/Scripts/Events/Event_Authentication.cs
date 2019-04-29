using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace iMessenger.Scripts.Events
{
    class Event_Authentication : Message
    {
        private string accessToken;
        public Event_Authentication(string token)
        {
            this.type = "authentication";
            this.accessToken = token;
        }

        public override byte[] GetBytes()
        {
            throw new NotImplementedException();
        }

        public override string GetJson()
        {
            JObject Jobj = new JObject(new JProperty("type", type), new JProperty("AccessToken", accessToken));
            return Jobj.ToString();
        }
    }
}
