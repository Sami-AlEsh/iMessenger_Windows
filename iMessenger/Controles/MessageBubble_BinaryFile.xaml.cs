using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;

namespace iMessenger
{
    /// <summary>
    /// Interaction logic for MessageBubble.xaml
    /// </summary>
    public partial class MessageBubble_BinaryFile : UserControl
    {
        private string filePath = null;

        public MessageBubble_BinaryFile()
        {
            InitializeComponent();
        }
        public MessageBubble_BinaryFile(string filePath , string sentDate, bool FriendMsgFlag)
        {
            InitializeComponent();
            this.filePath = filePath;
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

        private void OpenBinaryFile(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(filePath)) System.Diagnostics.Process.Start(filePath);
        }
    }
}
