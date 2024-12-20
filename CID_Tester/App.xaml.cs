using CID_Tester.Model;
using System.Configuration;
using System.Data;
using System.Windows;

namespace CID_Tester
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static App Me => ((App)Application.Current);

        public DataContext? Context { get; set; }

        private void ApplicationStart(object sender, StartupEventArgs e)
        {
            Context = new DataContext();

            Login login = new Login();
            login.ShowDialog();

            
        }
    }

}
