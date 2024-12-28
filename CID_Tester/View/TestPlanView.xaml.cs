using System;
using System.Collections.Generic;
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

namespace CID_Tester.View
{
    /// <summary>
    /// Interaction logic for TestPlanView.xaml
    /// </summary>
    public partial class TestPlanView : UserControl
    {
        public TestPlanView()
        {
            InitializeComponent();

            //DataContext? context = App.Me.Context;
            //myDataGrid1.ItemsSource = context.TEST_PARAMETER.ToList();
            //myDataGrid2.ItemsSource = context.TEST_PROCEDURE.ToList();
            //myDataGrid3.ItemsSource = context.DUT.ToList();
            //myDataGrid4.ItemsSource = context.TEST_USER.ToList();

        }
    }
}
