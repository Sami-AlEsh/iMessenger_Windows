using iMessenger.Scripts;
using iMessenger.Scripts.Events;
using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace iMessenger
{
    /// <summary>
    /// Interaction logic for ChatPage.xaml
    /// </summary>
    public partial class ChatPage : Page
    {
        public static TextBlock SelectedPerson;
        public ChatPage()
        {
            InitializeComponent();
            new MyTcpSocket().Connect();
            SelectedPerson = this.selectedPerson;
        }
        
        private void SendTextMsg(object sender, RoutedEventArgs e)
        {
            //Check Message
            if (string.IsNullOrEmpty(MessageList.SelectedPerson) || string.IsNullOrEmpty(this.InputBox.Text)) return;

            //Create Event_Text:
            var message = new Event_Text(MessageList.SelectedPerson, this.InputBox.Text);
            this.InputBox.Text = "";
            
            //Update UI & MainUser Chats Log:
            MainUser.mainUser.FrindsChat[MessageList.SelectedPerson].Add(message);

            //Send via TCP:
            message.SendMessage();
            //Store Sent JSON Message:
            message.Event_Text_Handler();
        }

        private void AttachFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                InitialDirectory = @"C:\",
                Title = "Select a file to upload to your Friend !",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "jpg",
                Filter ="All files (*.*)|*.*",
                FilterIndex = 2,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };
            openFileDialog1.ShowDialog();

            //Check Friend & File
            if( !string.IsNullOrEmpty(openFileDialog1.FileName) && !string.IsNullOrEmpty(MessageList.SelectedPerson))
            {
                var fileExt = openFileDialog1.FileName.Substring(openFileDialog1.FileName.Length - 3).ToLower();
                var filePath = openFileDialog1.FileName;
                switch (fileExt)
                {
                    //Images
                    case "jpg":
                    case "png":
                    case "gif":
                        {
                            //Create Event_Text:
                            var message = new Event_Image(MessageList.SelectedPerson, filePath);
                            this.InputBox.Text = "";

                            //Update UI & MainUser Chats Log:
                            MainUser.mainUser.FrindsChat[MessageList.SelectedPerson].Add(message);

                            //Send via TCP:
                            message.SendMessage();
                            //Store Sent JSON Message:
                            message.Event_Image_Handler();
                            break;
                        }

                    //Audios
                    case "mp3":
                    case "m4a":
                        {
                            //TODO attach audio files
                            break;
                        }

                    //Videos
                    case "mp4":
                        {
                            //TODO attach video file
                            break;
                        }

                    //Other
                    default:
                        {
                            Console.WriteLine("Unsupported File Extension to send ! (#"+fileExt+ ")");
                            break;
                        }
                }
            }
        }
    }
}
