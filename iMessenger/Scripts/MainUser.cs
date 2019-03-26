using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iMessenger.Scripts
{
    public class MainUser : User
    {
        private DateTime startDate;
        private string accessToken;

        public bool verified { set; get; } = false;


        public MainUser(string name,string userName , string email) : base(name, userName, email)
        {
            startDate = DateTime.Now;
            verified = GetUserAuthentication();
        }

        public bool GetUserAuthentication()
        {
            string ProjectBinPath = Environment.CurrentDirectory;
            string ProjectPath = Directory.GetParent(ProjectBinPath).Parent.FullName;
            if (File.Exists(ProjectPath + @"\AccessToken\AT.bin"))
            {
                accessToken = File.ReadAllText(ProjectPath + @"\AccessToken\AT.bin");
                Console.WriteLine("Your AccessToken -> "+accessToken);
                return true;
            }
            else
            {
                Console.WriteLine("No Current User to Auto-Login !");
                return false;
            }
        }
    }
}
