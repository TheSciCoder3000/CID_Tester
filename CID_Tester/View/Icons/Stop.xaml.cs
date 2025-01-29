using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CID_Tester.View.Icons;

public partial class Stop : UserControl
{
    public Stop()
    {
        InitializeComponent();
    }

    public static readonly DependencyProperty CommandProperty =
    DependencyProperty.Register("Command", typeof(ICommand), typeof(Stop), new PropertyMetadata(null));

    public ICommand Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public static readonly DependencyProperty ShowButtonTextProperty =
        DependencyProperty.Register("ShowButtonText", typeof(bool), typeof(Stop), new PropertyMetadata(false));

    public string ButtonText { get => (bool)GetValue(ShowButtonTextProperty) ? "Stop" : ""; }
    public string ShowButtonText { set => SetValue(ShowButtonTextProperty, value); }
}
