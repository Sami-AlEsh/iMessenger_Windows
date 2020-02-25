using System;
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
            JObject Jobj = new JObject(new JProperty("type", type), new JProperty("AccessToken", accessToken), new JProperty("platform", "windows"));
            return Jobj.ToString();
        }
    }
}
