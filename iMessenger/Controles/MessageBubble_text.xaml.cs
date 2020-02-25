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
    /// Interaction logic for MessageBubble.xaml
    /// </summary>
    public partial class MessageBubble_text : UserControl
    {
        public MessageBubble_text()
        {
            InitializeComponent();
        }
        public MessageBubble_text(string text , string sentDate, bool FriendMsgFlag)
        {
            InitializeComponent();
            this.BubbleText.Text = text;
            this.BubbleDate.Text = sentDate;
            if (FriendMsgFlag)
            {
                this.BubbleBorder.Background = Brushes.Beige;
                this.BubbleDate.HorizontalAlignment = HorizontalAlignment.Right;
                this.HorizontalAlignment = HorizontalAlignment.Left;
            }
            else
            {
                this.BubbleBorder.Background = Brushes.White;
                this.BubbleDate.HorizontalAlignment = HorizontalAlignment.Left;
                this.HorizontalAlignment = HorizontalAlignment.Right;
            }
        }
    }
}
