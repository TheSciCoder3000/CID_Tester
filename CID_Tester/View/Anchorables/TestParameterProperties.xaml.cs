using System.Windows.Controls;
using System.Windows.Input;

namespace CID_Tester.View.Anchorables;

public partial class TestParameterProperties : UserControl
{
    public TestParameterProperties()
    {
        InitializeComponent();
    }

    private void TextBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            var textBox = sender as TextBox;
            if (textBox != null)
            {
                var bindingExpression = textBox.GetBindingExpression(TextBox.TextProperty);
                bindingExpression?.UpdateSource();
                Keyboard.ClearFocus();
            }
        }
    }
}
