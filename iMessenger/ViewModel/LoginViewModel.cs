using iMessenger.Scripts;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Net;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace iMessenger
{

    /// <summary>
    /// The View Model of the Login window
    /// </summary>
    public class LoginViewModel : BaseViewModel
    {
        #region Public Properties

        public string Name { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public bool LoginIsRunning { get; set; } = false;
        #endregion

        #region Commands
        public RelayCommand LoginCommand { get; set; }
        #endregion

        #region Constractor
        
        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(() => Login() );
        }

        #endregion

        private void Login()
        {
            if (LoginIsRunning) return;
            var khe = this.Name;

            LoginIsRunning = true;
            

            LoginIsRunning = false;
        }

        
    }
}
