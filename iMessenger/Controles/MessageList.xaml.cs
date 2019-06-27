using iMessenger.Scripts;
using iMessenger.Scripts.Events;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

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
            Task.Factory.StartNew(() =>
            {
                Application.Current.Dispatcher.Invoke(() => InitList());
                SelectedPerson = name;
                Application.Current.Dispatcher.Invoke(() => ChatPage.SelectedPerson.Text = SelectedPerson);
                foreach (var Msg in MainUser.mainUser.FrindsChat[name])
                {
                    switch (Msg.type)
                    {
                        case "Text":
                            {
                                var txtMsg = Msg as Event_Text;
                                bool Txtflag = (txtMsg.Receiver == MainUser.mainUser.userName ? true : false);
                                Console.WriteLine("#### flag : " + txtMsg.Receiver + " - " + MainUser.mainUser.userName);
                                Application.Current.Dispatcher.Invoke(() => 
                                    messagesList.Children.Add(new MessageBubble_text(txtMsg.text, txtMsg.sentDate, Txtflag)));
                                break;
                            }
                        case "Image":
                            {
                                var ImgMsg = Msg as Event_Image;
                                bool Imgflag = (ImgMsg.Receiver == MainUser.mainUser.userName ? true : false);
                                Console.WriteLine("#### flag : " + ImgMsg.Receiver + " - " + MainUser.mainUser.userName);
                                Application.Current.Dispatcher.Invoke(() => 
                                    messagesList.Children.Add(new MessageBubble_image(ImgMsg.filePath, ImgMsg.sentDate, Imgflag)));
                                break;
                            }
                        case "BinaryFile":
                            {
                                var BFMsg = Msg as Event_BinaryFile;
                                bool BFflag = (BFMsg.Receiver == MainUser.mainUser.userName ? true : false);
                                Console.WriteLine("#### flag : " + BFMsg.Receiver + " - " + MainUser.mainUser.userName);
                                Application.Current.Dispatcher.Invoke(() => 
                                    messagesList.Children.Add(new MessageBubble_BinaryFile(BFMsg.filePath, BFMsg.sentDate, BFflag)));
                                break;
                            }
                        default:
                            {
                                Console.WriteLine("## Not Handled MessageBubble Type HERE");
                                break;
                            }
                    }
                }

            
                //Get last seen:
                var ServerUri = new Uri("http://" + MyTcpSocket.ServerIp + ":" + "8080");

                var client = new RestClient(ServerUri);
                //HTTP Request Route & Method
                var request = new RestRequest("/user/lastSeen/" + name, Method.GET);
            
                try
                {
                    client.ExecuteAsync(request, response =>
                    {
                        //Json response
                        var JsonResponse = new JObject();
                        try
                        {
                            JsonResponse = JObject.Parse(response.Content);

                            if ((bool)JsonResponse["status"])
                            {
                                var lastseen = (string)JsonResponse["data"];
                                Application.Current.Dispatcher.Invoke(() => ChatPage.LastSeen.Text = lastseen);
                            }
                            else
                            {
                                Console.WriteLine("HTTP Request failed # RESPONSE => " + response.Content);
                            }
                        }
                        catch (JsonReaderException)
                        {
                            Console.WriteLine("Error parsing LastSeen JSON Response");
                        }
                    });
                }
                catch (Exception error)
                {
                    Console.WriteLine("#ERROR in sending HTTP Request [LastSeen]: " + error.Message);
                }
            });
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
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                MessageList.messagesList.Children.Add(MB);
            }));
        }
    }
}
