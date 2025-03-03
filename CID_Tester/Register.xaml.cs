using System.Windows;
using System.Windows.Input;

namespace CID_Tester;
public partial class Register : Window
{
    public Register()
    {
        InitializeComponent();
    }

    private void Exit_Click(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }

    private void Window_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
            this.DragMove();
    }

    private void MediaElement_MediaEnded(object sender, RoutedEventArgs e)
    {
        bg.Position = TimeSpan.FromMilliseconds(1);
    }
}
