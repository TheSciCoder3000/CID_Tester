using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;

namespace CID_Tester.ViewModel.Behavior
{
    public class DataGridRowDoubleClickBehavior
    {
        public static readonly DependencyProperty CommandProperty =
        DependencyProperty.RegisterAttached(
            "Command",
            typeof(ICommand),
            typeof(DataGridRowDoubleClickBehavior),
            new PropertyMetadata(null, OnCommandChanged));

        public static ICommand GetCommand(DependencyObject obj) => (ICommand)obj.GetValue(CommandProperty);

        public static void SetCommand(DependencyObject obj, ICommand value) => obj.SetValue(CommandProperty, value);

        private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DataGrid dataGrid)
            {
                dataGrid.MouseDoubleClick -= OnMouseDoubleClick;

                if (e.NewValue != null)
                {
                    dataGrid.MouseDoubleClick += OnMouseDoubleClick;
                }
            }
        }

        private static void OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGrid dataGrid)
            {
                var row = FindParent<DataGridRow>((DependencyObject)e.OriginalSource);
                if (row != null && row.DataContext != null)
                {
                    var command = GetCommand(dataGrid);
                    if (command?.CanExecute(row.DataContext) == true)
                    {
                        command.Execute(row.DataContext);
                    }
                }
            }
        }

        private static T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            var parent = VisualTreeHelper.GetParent(child);
            while (parent != null && !(parent is T))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }
            return parent as T;
        }
    }
}
