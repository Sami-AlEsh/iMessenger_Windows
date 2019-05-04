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
        private string filePath = null;

        public MessageBubble_image()
        {
            InitializeComponent();
        }
        public MessageBubble_image(string filePath , bool FriendMsgFlag)
        {
            InitializeComponent();
            this.filePath = filePath;
            this.Image.Source = new BitmapImage(new Uri(filePath));
            if (FriendMsgFlag) { this.BubbleBorder.Background = Brushes.Beige; this.HorizontalAlignment = HorizontalAlignment.Left; }
            else { this.BubbleBorder.Background = Brushes.White; this.HorizontalAlignment = HorizontalAlignment.Right; }
        }
        

        private void OpenImage(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (!String.IsNullOrEmpty(filePath)) System.Diagnostics.Process.Start(filePath);
        }
    }
}
