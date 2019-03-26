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
        }
        
    }
}
