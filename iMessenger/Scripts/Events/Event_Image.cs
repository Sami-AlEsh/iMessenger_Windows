using System;
using System.IO;
using System.Windows;
using System.Drawing;
using Newtonsoft.Json.Linq;
using iMessenger.Scripts.Tools__Static_;

namespace iMessenger.Scripts.Events
{
    class Event_Image : Message
    {
        public string Receiver;
        public string filePath;
        public string extension;
        public string sentDate;


        /// <summary>
        /// Create Event_Image to send to SERVER
        /// </summary>
        /// <param name="Receiver"></param>
        /// <param name="RealFilePath"></param>
        public Event_Image(string Receiver, string RealFilePath)
        {
            this.type = "Image";
            this.Receiver = Receiver;
            this.extension = Path.GetExtension(RealFilePath).ToLower();

            var date = DateTime.Now;
            this.filePath = Path.GetFullPath(Project.Path + "/Database/" + Receiver + "/images/" + date.ToFileTime().ToString() + extension);
            this.sentDate = date.ToString();

            ImageResizer.ResizeImage(filePath, Image.FromFile(RealFilePath), 60);
        }

        /// <summary>
        /// Received image handler
        /// </summary>
        public void Event_Image_Handler()
        {
            //Update MainUser Chats Log:
            MainUser.mainUser.FrindsChat[Receiver].Add(this);

            //Store Json Image Message
            var JsonMsg = JObject.Parse(GetJson()); JsonMsg.Add(new JProperty("filePath", filePath));
            File.AppendAllText(Project.Path + @"/Database/" + Receiver + @"/chat.json", JsonMsg.ToString() + Environment.NewLine);
            Console.WriteLine("#$ Message Stored Successfuly");

            //Update UI
            if (MessageList.SelectedPerson == this.Receiver) //Receiver here as "Sender"
            {
                Application.Current.Dispatcher.Invoke(() => MessageList.addUIItem(new MessageBubble_image(filePath, sentDate, true)));
            }
            else
            {
                Application.Current.Dispatcher.Invoke(() => SideMenu.friendsList.addNotificationTo(this.Receiver, "[Image]"));
            }
            Console.WriteLine("Image Msg Added to UI !");
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
            //Decrypt Image
            var decryptedImage = MainUser.mainUser.DecryptMessage(image, Receiver);

            this.type = ImageMessage.SelectToken("type").Value<string>();
            this.ID = "null"; //TODO get ID from server
            this.Receiver = ImageMessage.SelectToken("sender").Value<string>();
            this.extension = "." + (string)ImageMessage["extension"];
            this.sentDate = ImageMessage.SelectToken("sentDate").Value<string>();

            this.filePath = Path.GetFullPath(Project.Path + "/Database/" + Receiver + "/images/" + DateTime.Now.ToFileTime().ToString() + extension);

            //Store Image Byte Array as File.
            File.WriteAllBytes(filePath, decryptedImage);
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
            var encryptedImage = MainUser.mainUser.EncryptMessage(File.ReadAllBytes(this.filePath), Receiver);
            return encryptedImage;
        }
    }
}
