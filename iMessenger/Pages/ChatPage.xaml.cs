using iMessenger.Scripts;
using iMessenger.Scripts.Events;
using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;

namespace iMessenger
{
    /// <summary>
    /// Interaction logic for ChatPage.xaml
    /// </summary>
    public partial class ChatPage : Page
    {
        public static TextBlock SelectedPerson;
        public static TextBlock LastSeen;
        public static TextBox InputBoxRef;
        public ChatPage()
        {
            InitializeComponent();
            new MyTcpSocket().Connect();
            SelectedPerson = this.selectedPerson;
            LastSeen = this.lastSeen;
            InputBoxRef = this.InputBox;
            SideMenu.UpdateFriendsUI();
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
            message.Event_Text_Handler(false);
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
                            //Create Event_Image:
                            var message = new Event_Image(MessageList.SelectedPerson, filePath);
                            this.InputBox.Text = "";

                            //Send via TCP:
                            message.SendMessage();

                            //Store JSON Message & Update UI & Update MainUser Frinds Chats Logs:
                            message.Event_Image_Handler();
                            break;
                        }

                    //Audios
                    case "mp3":
                    case "wav":
                    case "m4a":
                        {
                            //Create Event_BinaryFile:
                            var message = new Event_BinaryFile(MessageList.SelectedPerson, filePath);
                            this.InputBox.Text = "";

                            //Send via TCP:
                            message.SendMessage();

                            //Store JSON Message & Update UI & Update MainUser Frinds Chats Logs:
                            message.Event_BinaryFile_Handler();
                            break;
                        }

                    //Videos
                    case "avi":
                    case "mp4":
                        {
                            //Create Event_BinaryFile:
                            var message = new Event_BinaryFile(MessageList.SelectedPerson, filePath);
                            this.InputBox.Text = "";

                            //Send via TCP:
                            message.SendMessage();

                            //Store JSON Message & Update UI & Update MainUser Frinds Chats Logs:
                            message.Event_BinaryFile_Handler();
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

        private void ShowEmojisKeyboard(object sender, RoutedEventArgs e)
        {
            var progFiles = @"C:\Program Files\Common Files\Microsoft Shared\ink";
            var keyboardPath = System.IO.Path.Combine(progFiles, "TabTip.exe");
            System.Diagnostics.Process.Start(keyboardPath);
        }

        private void FixConnection(object sender, RoutedEventArgs e)
        {
            new MyTcpSocket().Connect();
        }
    }
}
