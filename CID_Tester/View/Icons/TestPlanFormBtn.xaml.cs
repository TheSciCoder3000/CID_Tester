using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CID_Tester.View.Icons;

public partial class TestPlanFormBtn : UserControl
{
    public TestPlanFormBtn()
    {
        InitializeComponent();
    }

    public static readonly DependencyProperty CommandProperty =
    DependencyProperty.Register("Command", typeof(ICommand), typeof(TestPlanFormBtn), new PropertyMetadata(null));

    public ICommand Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }
}
