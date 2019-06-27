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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace iMessenger
{
    /// <summary>
    /// Interaction logic for ChatListItemControl.xaml
    /// </summary>
    public partial class ChatListItemControl : UserControl
    {

        public string friendNameAlias { get; set; }
        public string friendName { get; set; }
        public string lastMessage { get; set; }
        public Visibility notification { get; set; }

        public ChatListItemControl()
        {
            InitializeComponent();
        }
        public ChatListItemControl(string name,string alias,string lastMsg)
        {
            InitializeComponent();

            friendNameAlias = this.FriendNameAlias.Text = alias;
            friendName = this.FriendName.Text = name;
            lastMessage = this.LastMessage.Text = lastMsg;
            notification = this.Notification.Visibility = Visibility.Hidden;
        }

        private void ShowChat(object sender, RoutedEventArgs e)
        {
            this.Notification.Visibility = Visibility.Hidden;
            MessageList.ShowMessagesFrom(friendName);
            Console.WriteLine("Show " + friendName + " Chat");
        }
    }
}
