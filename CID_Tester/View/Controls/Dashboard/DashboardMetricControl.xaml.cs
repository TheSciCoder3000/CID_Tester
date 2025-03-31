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

    private void CreateCell(String displayText, int rowCount, int columnCount)
    {
        TextBlock text = new TextBlock
        {
            Text = displayText,
            Padding = new Thickness(10),
            Foreground = Brushes.White
        };

        Border border = new Border { Child = text, Padding = new Thickness(5) };
        Grid.SetRow(border, rowCount);
        Grid.SetColumn(border, columnCount);
    }
}
