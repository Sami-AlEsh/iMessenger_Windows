﻿using iMessenger.Scripts;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Windows;
using System.Windows.Controls;

namespace iMessenger
{
    /// <summary>
    /// Interaction logic for ChatListItemControl_search.xaml
    /// </summary>
    public partial class ChatListItemControl_search : UserControl
    {
        User thisUser;
        public ChatListItemControl_search()
        {
            InitializeComponent();
        }

        public ChatListItemControl_search(User user)
        {
            InitializeComponent();
            this.thisUser = user;

            //Alias
            this.FriendNameAlias.Text = user.name.ToUpper()[0].ToString();
            //Name
            this.FriendName.Text = user.name;
            //Button
            if (MainUser.mainUser.Friends.Contains(user))
                this.Add_Btn.IsEnabled = false;
            else
                this.Delete_Btn.IsEnabled = false;
        }

        private void AddFriend(object sender, RoutedEventArgs e)
        {
            var ServerUri = new Uri("http://" + MyTcpSocket.ServerIp + ":" + "8080");

            var client = new RestClient(ServerUri);
            //HTTP Request Route & Method
            var request = new RestRequest("/user/addFriend", Method.POST);

            string jsonToSend = new JObject(new JProperty("current", MainUser.mainUser.userName),
                                             new JProperty("friend", thisUser.userName)
                                             ).ToString();

            request.AddParameter("application/json; charset=utf-8", jsonToSend, ParameterType.RequestBody);
            request.RequestFormat = RestSharp.DataFormat.Json;

            client.ExecuteAsync(request, response =>
            {
                Console.WriteLine("JS sent : " + jsonToSend);
                Console.WriteLine("RES : " + response.Content);
            });

            //Update MainUser Object & UI:
            MainUser.AddFriend(thisUser);
        }

        private void DeleteFriend(object sender, RoutedEventArgs e)
        {
            //TODO : Send HTTP Delete friend => On true Response do :
            MainUser.Delete_Block_Friend(thisUser);
        }

        private void BlockFriend(object sender, RoutedEventArgs e)
        {
            //TODO : Send HTTP Block friend => On true Response do :
            MainUser.mainUser.Friends.Remove(thisUser);
            //TODO Remove from UI
            //TODO Delete Messages and every thing
            //Now You Cant add this user
        }

    }
}
