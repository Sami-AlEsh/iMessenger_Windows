﻿using iMessenger.Scripts;
using iMessenger.Scripts.Events;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace iMessenger
{
    /// <summary>
    /// Interaction logic for SideMenu.xaml
    /// </summary>
    public partial class SideMenu : UserControl
    {
        public static SideMenu friendsList;
        public SideMenu()
        {
            InitializeComponent();
            friendsList = this;
        }
        
        public void UpdateFriendsUI()
        {
            //Load Last Friends Chats UI:
            Task.Factory.StartNew(() =>
            {
                foreach (User usr in MainUser.mainUser.Friends)
                {
                    string MainFilePath = Project.Path + @"\DataBase\" + usr.name + @"\chat.json";
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

                            switch ((string)obj["type"])
                            {
                                case "Text":
                                    messages.Add(new Event_Text(obj));
                                    break;
                                case "Image":
                                    messages.Add(new Event_Image(obj));
                                    break;
                                case "BinaryFile":
                                    messages.Add(new Event_BinaryFile(obj));
                                    break;
                                default:
                                    Console.WriteLine("Undefined Stored Message Type !");
                                    break;
                            }
                        }
                        MainUser.mainUser.FrindsChat.Add(usr.name, messages);

                        Console.WriteLine(usr.name + "Chats Loaded !");
                        reader.Close();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Binary Formatter Deserialize Error at User " + usr.name + " => " + e.Message);
                    }
                }
                Console.WriteLine("## All Chats Loaded ##");

                foreach (User usr in MainUser.mainUser.Friends)
                {
                    var count = MainUser.mainUser.FrindsChat[usr.name].Count;
                    if (count <= 0) continue;
                    var Msg = MainUser.mainUser.FrindsChat[usr.name][count - 1];
                    if (Msg.type == "Text")
                    {
                        Event_Text lastMsg = (Event_Text)Msg;
                        this.Dispatcher.Invoke(() => this.FriendsList.Children.Add(new ChatListItemControl(usr.name, usr.name[0].ToString(), lastMsg.text)));
                    }
                    else
                        this.Dispatcher.Invoke(() => this.FriendsList.Children.Add(new ChatListItemControl(usr.name, usr.name[0].ToString(), "[ " + Msg.type + " ]")));
                }
            });
        }

        public void AddFriend_UI(ChatListItemControl item)
        {
            this.Dispatcher.Invoke(() => this.FriendsList.Children.Add(item));
        }

        public void DeleteFriend_UI(string userName)
        {
            foreach (ChatListItemControl item in this.FriendsList.Children)
            {
                if (item.friendName == userName)
                {
                    this.Dispatcher.Invoke(() => this.FriendsList.Children.Remove(item));
                    return;
                }
                Console.WriteLine("#ERROR : trying to delete a friend not exist !!");
            }
        }


        private void ShowFriendsWindow(object sender, RoutedEventArgs e)
        {
            new FriendsSetting().ShowDialog();
        }
    }
}
