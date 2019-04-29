﻿using iMessenger.Scripts;
using iMessenger.Scripts.Events;
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
        public ChatPage()
        {
            InitializeComponent();
            new MyTcpSocket().Connect();
        }

        private void SendTextMsg(object sender, RoutedEventArgs e)
        {
            //Create Event_Text:
            var message = new Event_Text(MessageList.SelectedPerson, this.InputBox.Text);
            this.InputBox.Text = "";
            
            //Update UI & MainUser Chats Log:
            MainUser.mainUser.FrindsChat[MessageList.SelectedPerson].Add(message);

            //Send via TCP:
            message.SendMessage();
            //Store Sent JSON Message:
            message.Event_Text_Handler();
            //StoreMessage(message);
        }

        private void StoreMessage(Event_Text message)
        {

            using (StreamWriter file = File.CreateText(@"/Database/" + message.Receiver + "/chat.json"))
            using (JsonTextWriter writer = new JsonTextWriter(file))
            {
                JObject.Parse(message.GetJson()).WriteTo(writer);
            };
        }
    }
}