using System;
using Newtonsoft.Json.Linq;
using iMessenger.Scripts.RSA;

namespace iMessenger.Scripts.Events
{
    class Event_UpdateSecretKey : Message
    {
        private string Receiver;
        private Platform platform;
        private string encryptedSecretKey;

        /// <summary>
        /// Create Event_UpdateSecretKey to send to SERVER
        /// </summary>
        /// <param name="Receiver"></param>
        /// <param name="text"></param>
        /// <param name="platform"></param>
        /// <param name="encryptedSecretKey"></param>
        public Event_UpdateSecretKey(string Receiver, Platform platform, string encryptedSecretKey)
        {
            this.type = "updateSecretKey";
            this.Receiver = Receiver;
            this.platform = platform;
            this.encryptedSecretKey = encryptedSecretKey;
        }

        /// <summary>
        /// Parsing received Event_UpdateSecretKey from Server (Update & Store New Secret Key)
        /// </summary>
        /// <param name="TextMessage"></param>
        public Event_UpdateSecretKey(JObject UpdatedSecretKey)
        {
            var friendUsername = (string)UpdatedSecretKey["from"];
            var encryptedAESKey = (string)UpdatedSecretKey["encryptedSecretKey"];
            MainUser.mainUser.UpdateFriendKey(friendUsername, encryptedAESKey);
        }


        #region Get JSON\Bytes()

        public override string GetJson()
        {
            var platform_str = (this.platform == Platform.Windows ? "windows" : "android");

            JObject Jobj = new JObject(
                new JProperty("type", type),
                new JProperty("to", Receiver),
                new JProperty("toPlatform", platform_str),
                new JProperty("encryptedSecretKey", encryptedSecretKey)
                );

            return Jobj.ToString();
        }

        public override byte[] GetBytes()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
