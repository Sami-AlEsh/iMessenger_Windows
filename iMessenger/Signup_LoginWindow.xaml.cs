using System;
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
    /// Interaction logic for Signup_LoginWindow.xaml
    /// </summary>
    public partial class Signup_LoginWindow : Window
    {

        public Signup_LoginWindow()
        {
            InitializeComponent();

            this.DataContext = new WindowViewModel(this);

            MainUser.SaveLocalMainUser(new MainUser("sami", "sami98", "s@gmail.com"));
            MainUser mainUser = MainUser.LoadLocalMainUser();
            #region /* My Code */

            if (mainUser == null)
            {
                Console.WriteLine("No Local Main User !");
            }
            else
            {
                Console.WriteLine("Main User Founded !");
                //TODO: leave this window and open iMessenger Window
            }

            #endregion

        }

    }
}
