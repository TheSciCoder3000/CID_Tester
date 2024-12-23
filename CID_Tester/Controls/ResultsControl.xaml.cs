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
using System.IO;
using System.Diagnostics;

namespace CID_Tester.Controls
{
    /// <summary>
    /// Interaction logic for ResultsControl.xaml
    /// </summary>
    public partial class ResultsControl : UserControl
    {
        public ResultsControl()
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
