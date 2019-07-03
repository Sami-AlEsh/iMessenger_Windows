using System;
using System.IO;
using System.Windows;
using Newtonsoft.Json.Linq;

namespace iMessenger.Scripts.Events
{
    class Event_BinaryFile : Message
    {
        public string Receiver;
        public string filePath;
        public string extension;
        public string sentDate;

        /// <summary>
        /// Create Event_BinaryFile to send to SERVER
        /// </summary>
        /// <param name="Receiver"></param>
        /// <param name="RealFilePath"></param>
        /// <param name="FileExtension"></param>
        public Event_BinaryFile(string Receiver, string RealFilePath)
        {
            this.type = "BinaryFile";
            this.Receiver = Receiver;
            this.extension = Path.GetExtension(RealFilePath).ToLower();

            var date = DateTime.Now;
            this.filePath = RealFilePath;
            this.sentDate = date.ToString();
        }

        /// <summary>
        /// Received binaryfile handler
        /// </summary>
        public void Event_BinaryFile_Handler()
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
                Application.Current.Dispatcher.Invoke(() => MessageList.addUIItem(new MessageBubble_BinaryFile(filePath, sentDate, true)));
            }
            else
            {
                Application.Current.Dispatcher.Invoke(() => SideMenu.friendsList.addNotificationTo(this.Receiver, "[BinaryFile]"));
            }
            Console.WriteLine("BinaryFile Msg Added to UI !");
        }


        /// <summary>
        /// Parsing Event_BinaryFile from Chat.json file to show
        /// </summary>
        /// <param name="BinaryFileMessage"></param>
        public Event_BinaryFile(JObject BinaryFileMessage)
        {
            this.type = BinaryFileMessage.SelectToken("type").Value<string>();
            this.ID = "null"; //TODO get ID from server
            this.Receiver = BinaryFileMessage.SelectToken("receiver").Value<string>();
            this.extension = BinaryFileMessage.SelectToken("extension").Value<string>();
            this.filePath = BinaryFileMessage.SelectToken("filePath").Value<string>();
            this.sentDate = BinaryFileMessage.SelectToken("sentDate").Value<string>();
        }


        /// <summary>
        /// Receive BinaryFile from Server
        /// </summary>
        /// <param name="BFMessage"></param>
        /// <param name="file"></param>
        public Event_BinaryFile(JObject BFMessage, byte[] file)
        {
            this.type = BFMessage.SelectToken("type").Value<string>();
            this.ID = "null"; //TODO get ID from server
            this.Receiver = BFMessage.SelectToken("sender").Value<string>();
            this.extension = "." + (string)BFMessage["extension"];
            this.sentDate = BFMessage.SelectToken("sentDate").Value<string>();

            //Decrypt file
            var decryptedFile = MainUser.mainUser.DecryptMessage(file, Receiver);

            this.filePath= Path.GetFullPath(Project.Path + "/Database/" + Receiver + "/images/" + DateTime.Now.ToFileTime().ToString() + extension);

            //Store Image Byte Array as File.
            File.WriteAllBytes(filePath, decryptedFile);
        }


        public override string GetJson()
        {
            JObject json = new JObject(
                new JProperty("type",type),
                new JProperty("extension", extension),
                new JProperty("receiver",Receiver),
                new JProperty("sentDate",sentDate)
                );
            return json.ToString();
        }

        public override byte[] GetBytes()
        {
            var encryptedFile = MainUser.mainUser.EncryptMessage(File.ReadAllBytes(this.filePath), Receiver);
            return encryptedFile;
        }
    }
}
