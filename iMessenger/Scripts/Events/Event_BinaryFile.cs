using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace iMessenger.Scripts.Events
{
    class Event_BinaryFile : Message
    {
        public string Receiver;
        public string FilePath;
        public string FileExtension;
        public string sentDate;

        /// <summary>
        /// Create Event_Image to send to SERVER
        /// </summary>
        /// <param name="Receiver"></param>
        /// <param name="RealFilePath"></param>
        /// <param name="FileExtension"></param>
        public Event_BinaryFile(string Receiver, string RealFilePath)
        {
            this.type = "BinaryFile";
            this.Receiver = Receiver;
            this.FileExtension = Path.GetExtension(RealFilePath).ToLower();

            var date = DateTime.Now;
            this.FilePath = Path.GetFullPath(Project.Path + "/Database/" + Receiver + "/binaryfiles/" + date.ToFileTime().ToString() + FileExtension);
            this.sentDate = date.ToString();
        }
        public void Event_BinaryFile_Handler()
        {
            //Update MainUser Chats Log:
            MainUser.mainUser.FrindsChat[MessageList.SelectedPerson].Add(this);

            //Store Json Image Message
            var JsonMsg = JObject.Parse(GetJson()); JsonMsg.Add(new JProperty("filePath", FilePath));
            File.AppendAllText(Project.Path + @"/Database/" + Receiver + @"/chat.json", JsonMsg.ToString() + Environment.NewLine);
            Console.WriteLine("#$ Message Stored Successfuly");

            //Update UI
            Application.Current.Dispatcher.Invoke(() => MessageList.addUIItem(new MessageBubble_BinaryFile(FilePath, sentDate, true)));
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
            this.FileExtension = BinaryFileMessage.SelectToken("extension").Value<string>();
            this.FilePath = BinaryFileMessage.SelectToken("filePath").Value<string>();
            this.sentDate = BinaryFileMessage.SelectToken("sentDate").Value<string>();
        }


        /// <summary>
        /// Receive Image from Server
        /// </summary>
        /// <param name="BFMessage"></param>
        /// <param name="file"></param>
        public Event_BinaryFile(JObject BFMessage, byte[] file)
        {
            this.type = BFMessage.SelectToken("type").Value<string>();
            this.ID = "null"; //TODO get ID from server
            this.Receiver = BFMessage.SelectToken("sender").Value<string>();
            this.FileExtension = BFMessage.SelectToken("extension").Value<string>();
            this.sentDate = BFMessage.SelectToken("sentDate").Value<string>();

            this.FilePath= Path.GetFullPath(Project.Path + "/Database/" + Receiver + "/images/" + DateTime.Now.ToFileTime().ToString() + FileExtension);

            //Store Image Byte Array as File.
            File.WriteAllBytes(FilePath, file);
        }


        public override string GetJson()
        {
            JObject json = new JObject(
                new JProperty("type",type),
                new JProperty("extension", FileExtension),
                new JProperty("receiver",Receiver),
                new JProperty("sentDate",sentDate)
                );
            return json.ToString();
        }

        public override byte[] GetBytes()
        {
            return File.ReadAllBytes(this.FilePath);
        }
    }
}
