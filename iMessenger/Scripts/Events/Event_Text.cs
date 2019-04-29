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

        //When Sending Text Message :
        public Event_Text(string Receiver, string text)
        {
            this.type = "Text";
            this.ID = Message.GetID().ToString();
            this.Receiver = Receiver;
            this.text = text;
            sentDate = DateTime.Now.ToString();
            
        }
        //---------------------------

        //When Recieving Text Message :
        public Event_Text(JObject TextMessage)
        {
            this.Receiver = TextMessage.SelectToken("receiver").Value<string>();
            this.type = TextMessage.SelectToken("type").Value<string>();
            this.ID = "null"; //TODO get ID from server
            this.text = TextMessage.SelectToken("message").Value<string>();
            sentDate = TextMessage.SelectToken("sentDate").Value<string>();
        }
        public void Event_Text_Handler()
        {
            //Update Friend Chat Log
            if (File.Exists(Project.Path +@"/Database/" + Receiver + @"/chat.json"))
            {
                File.AppendAllText(Project.Path + @"/Database/" + Receiver + @"/chat.json", GetJson() + Environment.NewLine);
                Console.WriteLine("#$ Message Stored Successfuly");
                Event_Text_UpdateUI();
            }
            //Create Friend Chat Log
            else
            {
                try
                {
                    Directory.CreateDirectory("/Database/" + Receiver + "/");
                    File.WriteAllText(Project.Path + @"/Database/" + Receiver + "/chat.json", GetJson());
                    Console.WriteLine("#$ Message Stored Successfuly");

                    Event_Text_UpdateUI();
                }
                catch(Exception e)
                {
                    Console.WriteLine("#$ Error Handling Text Message -=> " + e.Message);
                }
            }
        }
        //-----------------------------
        private void Event_Text_UpdateUI()
        {
            Application.Current.Dispatcher.Invoke(()=> MessageList.addUIItem(new MessageBubble_text(text, true)));
            Console.WriteLine("JsonMsg Added to UI !");
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
