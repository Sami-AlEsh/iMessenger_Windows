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
            this.type = "Authentication";
            this.accessToken = "SAMI_6548$#%#%";//WORKING HERE TO GET MAIN_USER "ACCESSTOKEN"
        }

        public override byte[] GetBytes()
        {
            throw new NotImplementedException();
        }

        public override string GetJson()
        {
            JObject Jobj = new JObject(new JProperty("type", type), new JProperty("accessToken", accessToken));
            return Jobj.ToString();
        }
    }
}
