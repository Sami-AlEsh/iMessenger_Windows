using System.Windows;
using Audio.VoipCall;
using iMessenger.Scripts.Events;

namespace iMessenger
{
    /// <summary>
    /// Interaction logic for Call.xaml
    /// </summary>
    public partial class Call : Window
    {
        private static Window thisWindow;
        public static VoipCall voiceCall;
        private CallStatus callStatus;
        private string username;
        private static bool isInitialized = false;

        public Call(string username, CallStatus callStatus, string ip, int port = 1550)
        {
            InitializeComponent();
            thisWindow = this;

            this.callStatus = callStatus;
            this.username = username;
            if (this.callStatus == CallStatus.Caller) this.PickBtn.IsEnabled = false;
            else if (this.callStatus == CallStatus.Receiver) voiceCall = new VoipCall(ip, port);
        }

        private void PickCall(object sender, RoutedEventArgs e)
        {
            new Event_VoiceCall(username, Command.Accept).SendMessage();
            isInitialized = true;
            voiceCall.InitVoiceCall();
            this.PickBtn.IsEnabled = false;
        }

        private void DropCall(object sender, RoutedEventArgs e)
        {
            if(isInitialized)voiceCall.DropCall();
            isInitialized = false;
            new Event_VoiceCall(username, Command.Drop).SendMessage();
            DropCallRecieved();
        }

        public static void AcceptCallRecieved(string ip, int port)
        {
            voiceCall = new VoipCall(ip, port);
            voiceCall.InitVoiceCall();
            isInitialized = true;
        }

        public static void DropCallRecieved()
        {
            if(isInitialized)voiceCall.DropCall();
            isInitialized = false;
            voiceCall = null;

            thisWindow.Close();
        }
    }

    public enum CallStatus
    {
        Caller = 0,
        Receiver = 1
    }
}
