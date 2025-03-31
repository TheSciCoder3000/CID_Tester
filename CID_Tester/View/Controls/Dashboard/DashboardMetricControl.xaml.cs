using CID_Tester.Model;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CID_Tester.View.Controls.Dashboard;

public partial class DashboardMetricControl : UserControl
{
    public DashboardMetricControl()
    {
        InitializeComponent();

        //DataContext? context = App.Me.Context;

        //CreateCustomTable(context.TEST_PARAMETER.Local.ToList());
    }


    private void OngoingTests_AddingNewItem(object sender, AddingNewItemEventArgs e)
    {
        var dataGrid = sender as DataGrid;
        if (dataGrid.Items.Count > 0)
        {
            var lastItem = dataGrid.Items[dataGrid.Items.Count - 1];
            dataGrid.ScrollIntoView(lastItem);
        }
    }
}
