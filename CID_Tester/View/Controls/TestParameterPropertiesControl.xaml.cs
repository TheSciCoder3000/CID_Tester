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

namespace CID_Tester.View.Controls
{
    /// <summary>
    /// Interaction logic for TestParameterPropertiesControl.xaml
    /// </summary>
    public partial class TestParameterPropertiesControl : UserControl
    {
        public TestParameterPropertiesControl()
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
}
