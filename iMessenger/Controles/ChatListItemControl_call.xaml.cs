using iMessenger.Scripts;
using System.Windows;
using System.Windows.Controls;

namespace iMessenger
{
    /// <summary>
    /// Interaction logic for ChatListItemControl_call.xaml
    /// </summary>
    public partial class ChatListItemControl_call : UserControl
    {
        public ChatListItemControl_call()
        {
            InitializeComponent();
        }

        public ChatListItemControl_call(User user)
        {
            InitializeComponent();

            //Alias
            this.FriendNameAlias.Text = user.name.ToUpper()[0].ToString();
            
            //Name
            this.FriendName.Text = user.name;
        }

        private void CallFriend(object sender, RoutedEventArgs e)
        {
            //TODO CAll Friend via Events
        }
    }
}
