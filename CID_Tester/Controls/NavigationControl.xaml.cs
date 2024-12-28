using CID_Tester.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CID_Tester.Controls
{
    /// <summary>
    /// Interaction logic for NavigationControl.xaml
    /// </summary>
    public partial class NavigationControl : UserControl
    {
        public event Action<string, UserControl> AddDocumentRequested;

        public NavigationControl()
        {
            InitializeComponent();
        }

        private void addDocument_Click(object sender, MouseButtonEventArgs e)
        {

            String type = (String)((Button)sender).Tag;
            UserControl control = null;
            switch(type)
            {
                case "dashboard":
                    control = new DashboardView();
                    //DashboardViewModel vm = new DashboardViewModel(((MainViewModel)this.DataContext).);
                    //control.DataContext = vm;
                    break;
                case "plan":
                    control = new TestPlanControl();
                    break;
                case "results":
                    control = new ResultsControl();
                    break;
                case "settings":
                    control = new SettingsControl();
                    break;

            }

            if (control != null)
            {
                AddDocumentRequested?.Invoke(type, control);
            }
        }
    }
}
