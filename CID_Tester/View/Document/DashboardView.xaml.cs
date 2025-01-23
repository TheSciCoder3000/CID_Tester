using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace CID_Tester.View.Document;

public partial class DashboardView : UserControl
{
    public DashboardView()
    {
        InitializeComponent();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        var currentExecutablePath = Process.GetCurrentProcess().MainModule.FileName;
        Process.Start(currentExecutablePath);
        App.Current.Shutdown();
    }
}
