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

namespace CID_Tester.Controls
{
    /// <summary>
    /// Interaction logic for DashboardControl.xaml
    /// </summary>
    public partial class DashboardControl : UserControl
    {
        public DashboardControl()
        {
            InitializeComponent();
            ViewDataTable.ItemsSource = LoadCollectionData();
        }


        private List<SampleData> LoadCollectionData()
        {
            List<SampleData> SampleDatas = new List<SampleData>();
            SampleDatas.Add(new SampleData()
            {
                PARAM_CODE = 1,
                TEST_CODE = 101,
                DESC= "Graphics Programming with GDI+",
                VALUE = 150.6F,
                UNIT = "mV",
                PASS = true
            });

            SampleDatas.Add(new SampleData()
            {
                PARAM_CODE = 2,
                TEST_CODE = 101,
                DESC = "Test Plan Parameter 2",
                VALUE = 150.6F,
                UNIT = "mV",
                PASS = true
            });

            SampleDatas.Add(new SampleData()
            {
                PARAM_CODE = 3,
                TEST_CODE = 101,
                DESC = "Test Plan Parameter 3",
                VALUE = 150.6F,
                UNIT = "mV",
                PASS = true
            });

            return SampleDatas;
        }

        

    }


}
