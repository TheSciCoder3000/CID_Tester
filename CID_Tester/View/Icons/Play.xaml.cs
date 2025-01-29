using System.IdentityModel.Tokens.Jwt;
using System.Security.RightsManagement;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CID_Tester.View.Icons;

public partial class Play : UserControl
{
    public Play()
    {
        InitializeComponent();
    }

    public static readonly DependencyProperty CommandProperty = 
        DependencyProperty.Register("Command", typeof(ICommand), typeof(Play), new PropertyMetadata(null));

    public ICommand Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public static readonly DependencyProperty ShowButtonTextProperty =
        DependencyProperty.Register("ShowButtonText", typeof(bool), typeof(Play), new PropertyMetadata(false));

    public string ButtonText { get => (bool)GetValue(ShowButtonTextProperty) ? "Start" : ""; }
    public string ShowButtonText { set => SetValue(ShowButtonTextProperty, value); }
}
