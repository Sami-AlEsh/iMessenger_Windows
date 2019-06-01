using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace iMessenger.Scripts.Events
{
    class Event_Text : Message
    {
        public string Receiver;
        public string text;
        public string sentDate;

        /// <summary>
        /// Create Event_Text to send to SERVER
        /// </summary>
        /// <param name="Receiver"></param>
        /// <param name="text"></param>
        public Event_Text(string Receiver, string text)
        {
            this.type = "Text";
            //this.ID = "null"; //TODO get ID from server
            this.Receiver = Receiver;
            this.text = text;
            sentDate = DateTime.Now.ToString();
        }
        public void Event_Text_Handler()
        {
            //Update MainUser Chats Log:
            MainUser.mainUser.FrindsChat[MessageList.SelectedPerson].Add(this);

            //Store Json Image Message
            var JsonMsg = JObject.Parse(GetJson());
            File.AppendAllText(Project.Path + @"/Database/" + Receiver + @"/chat.json", JsonMsg.ToString() + Environment.NewLine);
            Console.WriteLine("#$ Message Stored Successfuly");

            //Update UI
            Application.Current.Dispatcher.Invoke(() => MessageList.addUIItem(new MessageBubble_text(text, sentDate, true)));
            Console.WriteLine("Text Msg Added to UI !");
        }


        /// <summary>
        /// Parsing Event_Text from Chat.json file to show / Receive Text from Server
        /// </summary>
        /// <param name="TextMessage"></param>
        public Event_Text(JObject TextMessage)
        {
            this.Receiver = (string)TextMessage["receiver"];
            if (string.IsNullOrEmpty(Receiver))
                this.Receiver = (string)TextMessage["sender"];

            this.type = TextMessage.SelectToken("type").Value<string>();
            this.ID = "null"; //TODO get ID from server
            this.text = TextMessage.SelectToken("message").Value<string>();
            sentDate = TextMessage.SelectToken("sentDate").Value<string>();
        }

        #region Get JSON\Bytes()
        public override string GetJson()
        {
            JObject Jobj = new JObject(
                new JProperty("type", type),
                new JProperty("receiver",Receiver) ,
                new JProperty("message", text) ,
                new JProperty("sentDate", sentDate)
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
