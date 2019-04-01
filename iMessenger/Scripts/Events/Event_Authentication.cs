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
        public Event_Authentication()
        {
            this.type = "authentication";
            this.accessToken =
                "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VybmFtZSI6InNhbWk5OCIsInVzZXJJRCI6IjNkZDBlZDEwLTU0NTAtMTFlOS1iMWE2LWVkNzZiMzg1M2M2MyIsImlhdCI6MTU1NDEwMzkyMn0.XvD5LqMyFA-v-OFyLhROT_QjIqdeD22v2Dd4Ud42Fss";
            //WORKING HERE TO GET MAIN_USER "ACCESSTOKEN"
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
