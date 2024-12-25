using CID_Tester.Model;
using System.Configuration;
using System.Data;
using System.Windows;
using System.IO;
using System.Diagnostics;
using CID_Tester.ViewModel;

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

            LoginViewModel vm = new LoginViewModel();
            Login login = new Login();
            vm.ClosingRequest += (sender, e) => login.Close();
            login.DataContext = vm;
            login.ShowDialog();


        }
    }

}
