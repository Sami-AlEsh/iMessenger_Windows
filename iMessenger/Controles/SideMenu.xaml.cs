using iMessenger.Scripts;
using iMessenger.Scripts.Events;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
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
    /// Interaction logic for SideMenu.xaml
    /// </summary>
    public partial class SideMenu : UserControl
    {
        public static StackPanel friendsList = new StackPanel();
        public SideMenu()
        {
            InitializeComponent();
            friendsList = this.FriendsList;

            //Load Last Friends Chats UI:
            Task.Factory.StartNew(() => {
                string ProjectBinPath = Environment.CurrentDirectory;
                string ProjectPath = Directory.GetParent(ProjectBinPath).Parent.FullName;

                foreach (User usr in MainUser.mainUser.Friends)
                {
                    string MainFilePath = ProjectPath + @"\DataBase\"+usr.name+@"\chat.json";
                    string json = File.ReadAllText(MainFilePath);

                    try
                    {
                        List<Message> messages = new List<Message>();

                        JsonTextReader reader = new JsonTextReader(new StringReader(json));
                        reader.SupportMultipleContent = true;

                        while (reader.Read())
                        {
                            JsonSerializer serializer = new JsonSerializer();
                            JObject obj = serializer.Deserialize<JObject>(reader);

                            switch (obj["type"].Value<string>())
                            {
                                case "Text":
                                    messages.Add(new Event_Text(obj));
                                    break;
                                default:
                                    break;
                            }
                        }
                        MainUser.mainUser.FrindsChat.Add(usr.name, messages);

                        Console.WriteLine(usr.name + "Chats Loaded !");
                        reader.Close();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Binary Formatter Deserialize Error at User "+usr.name+" => " + e.Message);
                    }
                }
                Console.WriteLine("## All Chats Loaded ##");

                foreach (User usr in MainUser.mainUser.Friends)
                {
                    var count = MainUser.mainUser.FrindsChat[usr.name].Count;
                    if (count <= 0) continue;
                    Event_Text lastMsg = (Event_Text)MainUser.mainUser.FrindsChat[usr.name][count-1];
                    if(lastMsg.type == "Text")
                        this.Dispatcher.Invoke(()=>this.FriendsList.Children.Add(new ChatListItemControl(usr.name, usr.name[0].ToString(), lastMsg.text)));
                    else
                        this.Dispatcher.Invoke(() => this.FriendsList.Children.Add(new ChatListItemControl(usr.name, "AA", "BinatyFile NOT Handled yet here")));
                }
            });
        }
        
        public void AddFriend_UI(ChatListItemControl item)
        {
            this.Dispatcher.Invoke(() => this.FriendsList.Children.Add(item));
        }
    }
}
