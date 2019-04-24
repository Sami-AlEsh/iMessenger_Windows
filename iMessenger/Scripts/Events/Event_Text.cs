using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iMessenger.Scripts.Events
{
    class Event_Text : Message
    {
        private string Receiver;
        public string text;
        private string SentDate;

        //When Sending Text Message :
        public Event_Text(string Receiver, string text)
        {
            this.type = "Text";
            this.ID = Message.GetID().ToString();
            this.Receiver = Receiver;
            this.text = text;
            SentDate = DateTime.Now.ToString();
            
        }
        //---------------------------

        //When Recieving Text Message :
        public Event_Text(JObject TextMessage)
        {
            this.Receiver = TextMessage.SelectToken("receiver").Value<string>();
            this.type = TextMessage.SelectToken("type").Value<string>();
            this.ID = "null";
            this.text = TextMessage.SelectToken("text").Value<string>();
            SentDate = TextMessage.SelectToken("sentDate").Value<string>();
        }
        public void Event_Text_Handler()
        {
            //Update Friend Chat Log
            if (File.Exists("/Database/" + Receiver + "/" + "Chat/Chat.json"))
            {
                File.AppendAllText("/Database/" + Receiver + "/" + "Chat/Chat.json", GetJson() + Environment.NewLine);
                Console.WriteLine("#$ Message Stored Successfuly");
            }
            //Create Friend Chat Log
            else
            {
                string ProjectBinPath = Environment.CurrentDirectory;
                string ProjectPath = Directory.GetParent(ProjectBinPath).Parent.FullName;

                try
                {
                    Directory.CreateDirectory("/Database/" + Receiver + "/Chat/");
                    File.WriteAllText(ProjectPath + @"/Database/" + Receiver + "/chat.json", GetJson());
                }
                catch(Exception e)
                {
                    Console.WriteLine("#$ Error Handling Text Message -=> " + e.Message);
                }
            }
        }
        //-----------------------------

        #region Get JSON\Bytes()
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
        #endregion
    }
}
