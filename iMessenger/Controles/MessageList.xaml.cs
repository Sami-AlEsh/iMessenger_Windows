using iMessenger.Scripts;
using iMessenger.Scripts.Events;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace iMessenger
{
    /// <summary>
    /// Interaction logic for MessageList.xaml
    /// </summary>
    public partial class MessageList : UserControl
    {
        public static StackPanel messagesList = new StackPanel();
        public static string SelectedPerson { get; set; }

        public MessageList()
        {
            InitializeComponent();
            messagesList = this.List;
        }
        public static void ShowMessagesFrom(string name)
        {
            InitList();
            SelectedPerson = name;
            ChatPage.SelectedPerson.Text = SelectedPerson;
            foreach (var Msg in MainUser.mainUser.FrindsChat[name])
            {
                switch (Msg.type)
                {
                    case "Text":
                        {
                            var txtMsg = Msg as Event_Text;
                            bool Txtflag = (txtMsg.Receiver == MainUser.mainUser.userName ? true : false);
                            Console.WriteLine("#### flag : " + txtMsg.Receiver + " - " + MainUser.mainUser.userName);
                            messagesList.Children.Add(new MessageBubble_text(txtMsg.text, txtMsg.sentDate, Txtflag));
                            break;
                        }
                    case "Image":
                        {
                            var ImgMsg = Msg as Event_Image;
                            bool Imgflag = (ImgMsg.Receiver == MainUser.mainUser.userName ? true : false);
                            Console.WriteLine("#### flag : " + ImgMsg.Receiver + " - " + MainUser.mainUser.userName);
                            messagesList.Children.Add(new MessageBubble_image(ImgMsg.filePath, ImgMsg.sentDate, Imgflag));
                            break;
                        }
                    default:
                        {
                            Console.WriteLine("## Not Handled MessageBubble Type HERE");
                            break;
                        }
                }
            }
        }

        private static void InitList()
        {
            while (messagesList.Children.Count > 0)
            {
                messagesList.Children.RemoveAt(messagesList.Children.Count - 1);
            }
        }

        public static void addUIItem(UserControl MB)
        {
            //this.Dispatcher.Invoke(() => );
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                MessageList.messagesList.Children.Add(MB);
            }));
        }
    }
}
