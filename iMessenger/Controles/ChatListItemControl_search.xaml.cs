using iMessenger.Scripts;
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
            //TODO : Send HTTP Add friend => On true Response do :
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
