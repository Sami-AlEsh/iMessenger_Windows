using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace iMessenger
{
    /// <summary>
    /// Interaction logic for MessageBubble.xaml
    /// </summary>
    public partial class MessageBubble_image : UserControl
    {
        public MessageBubble_image()
        {
            InitializeComponent();
        }
        public MessageBubble_image(string filePath , bool FriendMsgFlag)
        {
            InitializeComponent();
            this.Image.Source = new BitmapImage(new Uri(filePath));
            if (FriendMsgFlag) { this.BubbleBorder.Background = Brushes.Beige; this.HorizontalAlignment = HorizontalAlignment.Left; }
            else { this.BubbleBorder.Background = Brushes.White; this.HorizontalAlignment = HorizontalAlignment.Right; }
        }
    }
}
