﻿using System;
using System.Collections.Generic;
using System.IO;
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
using iMessenger.Scripts;

namespace iMessenger
{
    /// <summary>
    /// Interaction logic for Main Window.xaml
    /// </summary>
    public partial class Signup_LoginWindow : Window
    {
        private static Frame mMainFrame = new Frame();

        public Signup_LoginWindow()
        {
            InitializeComponent();
            mMainFrame = this.MainFrame;
            this.DataContext = new WindowViewModel(this);

            MainUser.mainUser = MainUser.LoadLocalMainUserJS();
            if (MainUser.mainUser != null)
            {
                MainUser.mainUser.InitializeAllKeys();
                SwitchPage(ApplicationPage.chat);
            }
            else SwitchPage(ApplicationPage.login);

            //OnClose Event:
            Closing += MessengerClosed;
        }

        private void MessengerClosed(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //if(MainUser.mainUser != null ) MainUser.SaveLocalMainUserJS();
        }

        public static void SwitchPage(ApplicationPage page)
        {
            switch (page)
            {
                case ApplicationPage.login:
                    {
                        mMainFrame.NavigationService.Navigate(new LoginPage());
                        break;
                    }

                case ApplicationPage.chat:
                    {
                        mMainFrame.NavigationService.Navigate(new ChatPage());
                        break;
                    }
                default:
                    {
                        mMainFrame.NavigationService.Navigate(new LoginPage());
                        break;
                    }
            }
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            //Save Change to Local Storage
            if (MainUser.mainUser != null)
            {
                MainUser.mainUser.FrindsChat.Clear();
                MainUser.SaveLocalMainUserJS();
            }

            //Close Connection
            if(MyTcpSocket.clientsocket != null && MyTcpSocket.clientsocket.Connected) MyTcpSocket.clientsocket.Close();

            base.OnClosing(e);
        }

    }
}
