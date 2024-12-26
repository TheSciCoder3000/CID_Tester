using CID_Tester.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace CID_Tester.Controls
{
    /// <summary>
    /// Interaction logic for DashboardControl.xaml
    /// </summary>
    public partial class DashboardControl : UserControl
    {
        


        public DashboardControl()
        {
            InitializeComponent();


            //myDataGrid2.ItemsSource = context.TEST_PROCEDURE.ToList();
            //myDataGrid3.ItemsSource = context.DUT.ToList();
            //myDataGrid4.ItemsSource = context.TEST_USER.ToList();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var currentExecutablePath = Process.GetCurrentProcess().MainModule.FileName;
            Process.Start(currentExecutablePath);
            App.Current.Shutdown();
        }
    }


}

