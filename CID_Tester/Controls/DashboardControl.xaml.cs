using CID_Tester.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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
using static System.Net.Mime.MediaTypeNames;

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
            DataContext context = new DataContext();

            Debug.WriteLine("TEST");
            CreateCustomTable(context.TEST_PARAMETER.ToList());
            //myDataGrid2.ItemsSource = context.TEST_PROCEDURE.ToList();
            //myDataGrid3.ItemsSource = context.DUT.ToList();
            //myDataGrid4.ItemsSource = context.TEST_USER.ToList();
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

        private void CreateCell(String displayText, int rowCount, int columnCount)
        {
            TextBlock text = new TextBlock
            {
                Text = displayText,
                Padding = new Thickness(10),
                Foreground = Brushes.White
            };

            Border border = new Border { Child = text, Padding = new Thickness(5)};
            Grid.SetRow(border, rowCount);
            Grid.SetColumn(border, columnCount);
            MainGrid.Children.Add(border);
        }

        private void CreateCustomTable(List<TEST_PARAMETER> tableData)
        {
            Debug.WriteLine(tableData.Count);


            int row_count = 1;

            foreach (var test in tableData)
            {
                // Create a TextBlock to display the data


                CreateCell(test.DESCRIPTION.ToString(), row_count, 0);
                CreateCell(test.METRIC.ToString(), row_count, 1);
                CreateCell(test.TARGET.ToString(), row_count, 2);
                CreateCell(test.VALUE.ToString(), row_count, 3);
                CreateCell(test.PASS.ToString(), row_count, 4);


                // Add the cell to the grid at the appropriate row and column

                RowDefinition rowParams = new RowDefinition();
                MainGrid.RowDefinitions.Add(rowParams);

                Debug.WriteLine(row_count);
                row_count++;
            }
        }



    }


}

