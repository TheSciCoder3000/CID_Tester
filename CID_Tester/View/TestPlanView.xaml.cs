using System.Windows.Controls;

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
