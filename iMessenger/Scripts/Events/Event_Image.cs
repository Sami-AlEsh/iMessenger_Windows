using System.Windows;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iMessenger.Scripts.Events
{
    class Event_Image : Message
    {
        public string Receiver;
        public string filePath;
        public string extension;
        public string sentDate;

        private byte[] image;

        /// <summary>
        /// Create Event_Image to send to SERVER
        /// </summary>
        /// <param name="Receiver"></param>
        /// <param name="FilePath"></param>
        public Event_Image(string Receiver, string FilePath)
        {
            this.type = "Image";
            this.Receiver = Receiver;
            this.filePath = FilePath;
            this.extension = FilePath.Substring(FilePath.Length-4,4);
            this.sentDate = DateTime.Now.ToString();

            this.image = File.ReadAllBytes(filePath);
        }

        /// <summary>
        /// Parsing Event_Image from Chat.json file to show
        /// </summary>
        /// <param name="ImageMessage"></param>
        public Event_Image(JObject ImageMessage)
        {
            this.type = ImageMessage.SelectToken("type").Value<string>();
            this.ID = "null"; //TODO get ID from server
            this.Receiver = ImageMessage.SelectToken("receiver").Value<string>();
            this.extension = ImageMessage.SelectToken("extension").Value<string>();
            this.filePath = ImageMessage.SelectToken("filePath").Value<string>();
            this.sentDate = ImageMessage.SelectToken("sentDate").Value<string>();
        }


        /// <summary>
        /// Receive Image from Server
        /// </summary>
        /// <param name="Json">Image detail</param>
        /// <param name="image">Image a Byte array</param>
        public Event_Image(JObject ImageMessage, byte[] image)
        {
            this.type = ImageMessage.SelectToken("type").Value<string>();
            this.ID = "null"; //TODO get ID from server
            this.Receiver = ImageMessage.SelectToken("sender").Value<string>();
            this.extension = ImageMessage.SelectToken("extension").Value<string>();
            this.filePath = Project.Path + @"/Database/" + Receiver + @"/images/" + DateTime.Now.ToString() + extension;
            this.sentDate = ImageMessage.SelectToken("sentDate").Value<string>();

            this.image = image;
        }
        public void Event_Image_Handler()
        {
            //Store Json Image Message
            var JsonMsg = JObject.Parse(GetJson());  JsonMsg.Add(new JProperty("filePath", filePath));
            File.AppendAllText(Project.Path + @"/Database/" + Receiver + @"/chat.json",JsonMsg.ToString() + Environment.NewLine);
            Console.WriteLine("#$ Message Stored Successfuly");

            //Store Image Byte Array as File.
            File.WriteAllBytes(filePath, this.image);
            Event_Image_UpdateUI();
        }

        private void Event_Image_UpdateUI()
        {
            Application.Current.Dispatcher.Invoke(() => MessageList.addUIItem(new MessageBubble_image(filePath, sentDate, true)));
            Console.WriteLine("Image Msg Added to UI !");
        }


        public override string GetJson()
        {
            JObject json = new JObject(
                new JProperty("type",type),
                new JProperty("receiver", Receiver),
                new JProperty("extension",extension),
                new JProperty("sentDate",sentDate)
                );
            return json.ToString();
        }

        public override byte[] GetBytes()
        {
            return File.ReadAllBytes(this.filePath);
        }
    }
}
