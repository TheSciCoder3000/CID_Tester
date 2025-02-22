using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using CID_Tester.ViewModel.Document;

namespace CID_Tester.View.Document;

public partial class DebugView : UserControl
{
    public DebugView()
    {
        InitializeComponent();
        
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        //Output.AppendText( + Environment.NewLine);
    }

    private void Output_TextChanged(object sender, TextChangedEventArgs e)
    {
        Scroll.ScrollToEnd();
    }

    private void TextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
    {
        var textBox = sender as TextBox;
        e.Handled = Regex.IsMatch(e.Text, "[^0-9]+");
    }
}