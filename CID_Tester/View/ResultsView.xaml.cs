using System.Diagnostics;
using System.Windows.Controls;

namespace CID_Tester.View
{
    /// <summary>
    /// Interaction logic for ResultsView.xaml
    /// </summary>
    public partial class ResultsView : UserControl
    {
        public ResultsView()
        {
            InitializeComponent();

            //String path = AppDomain.CurrentDomain.BaseDirectory;
            //String sample = "file:///" + path + "PDF\\Sample.pdf";


            //Debug.WriteLine(new Uri(sample));
            //pdf.Source = new Uri(sample);
            //pdf.NavigateToString(sample);

        }

        private async void pdf_Initialized(object sender, EventArgs e)
        {

            String path = AppDomain.CurrentDomain.BaseDirectory;
            String sample = "file:///" + path + "PDF\\Sample.pdf";


            await pdf.EnsureCoreWebView2Async(null);
            //pdf.NavigateToString("https://google.com");
            pdf.Source = new Uri("https://www.google.com");

            Debug.WriteLine(new Uri(sample));
        }
    }
}
