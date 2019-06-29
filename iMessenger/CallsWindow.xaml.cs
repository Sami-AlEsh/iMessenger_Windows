using iMessenger.Scripts;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace iMessenger
{
    /// <summary>
    /// Interaction logic for CallsWindow.xaml
    /// </summary>
    public partial class CallsWindow : Window
    {
        //public static VoipCall voiceCall;
        public CallsWindow()
        {
            InitializeComponent();
            UpdateFriendList();
        }

        private void UpdateFriendList()
        {
            Task.Factory.StartNew(() =>
            {
                //voiceCall.init();
                foreach (var friend in MainUser.mainUser.Friends)
                {
                    this.Dispatcher.Invoke(() => 
                    {
                        this.FriendsList.Children.Add(new ChatListItemControl_call(friend));
                    });
                }
            });
        }

        private void DropCall(object sender, RoutedEventArgs e)
        {
            //TODO Drop the call
        }
    }
}
