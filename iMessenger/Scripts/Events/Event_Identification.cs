using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace iMessenger.Scripts.Events
{
    class Event_Identification : Message
    {
        private string accessToken;
        public Event_Identification()
        {
            this.type = "Identification";
            accessToken = "SAMI_6548$#%#%";//WORKING HERE TO GET MAIN_USER "ACCESSTOKEN"
        }

        public override byte[] GetBinaryFile()
        {
            throw new NotImplementedException();
        }

        public override string GetJson()
        {
            JObject Jobj = new JObject(new JProperty("type", "Identification"), new JProperty("accessToken", accessToken));
            return Jobj.ToString();
        }
    }
}
