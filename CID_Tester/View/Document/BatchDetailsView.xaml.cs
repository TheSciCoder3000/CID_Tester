using System.Windows.Controls;
using System.Windows.Input;

namespace CID_Tester.View.Document
{
    /// <summary>
    /// Interaction logic for BatchDetailsView.xaml
    /// </summary>
    public partial class BatchDetailsView : UserControl
    {
        public BatchDetailsView()
        {
            InitializeComponent();
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }
    }
}
