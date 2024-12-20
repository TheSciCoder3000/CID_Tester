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


            DataContext? context = App.Me.Context;

            CreateCustomTable(context.TEST_PARAMETER.ToList());
            //myDataGrid2.ItemsSource = context.TEST_PROCEDURE.ToList();
            //myDataGrid3.ItemsSource = context.DUT.ToList();
            //myDataGrid4.ItemsSource = context.TEST_USER.ToList();
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

                CreateCell(test.DESCRIPTION.ToString(), row_count, 0);
                CreateCell(test.METRIC.ToString(), row_count, 1);
                CreateCell(test.TARGET.ToString(), row_count, 2);
                CreateCell(test.VALUE.ToString(), row_count, 3);

                TextBlock text = new TextBlock
                {
                    Text = (test.PASS == 1) ? "PASS" : "FAIL",
                    FontWeight = FontWeights.Bold,
                    Padding = new Thickness(10),
                    Foreground = Brushes.White,
                    HorizontalAlignment = HorizontalAlignment.Center,
                };

                Border border = new Border
                {
                    Child = text,
                    Padding = new Thickness(5),
                    Background = (test.PASS == 1) ? Brushes.Green : Brushes.Red
                };
                Grid.SetRow(border, row_count);
                Grid.SetColumn(border, 4);
                MainGrid.Children.Add(border);


                RowDefinition rowParams = new RowDefinition();
                MainGrid.RowDefinitions.Add(rowParams);

                Debug.WriteLine(row_count);
                row_count++;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var currentExecutablePath = Process.GetCurrentProcess().MainModule.FileName;
            Process.Start(currentExecutablePath);
            App.Current.Shutdown();
        }
    }


}

