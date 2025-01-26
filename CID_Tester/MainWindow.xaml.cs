using AvalonDock.Layout;
using AvalonDock.Layout.Serialization;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace CID_Tester
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ToolBar_Loaded(object sender, RoutedEventArgs e)
        {
            ToolBar toolBar = sender as ToolBar;
            var overflowGrid = toolBar.Template.FindName("OverflowGrid", toolBar) as FrameworkElement;
            if (overflowGrid != null)
            {
                overflowGrid.Visibility = Visibility.Collapsed;
            }

            var mainPanelBorder = toolBar.Template.FindName("MainPanelBorder", toolBar) as FrameworkElement;
            if (mainPanelBorder != null)
            {
                mainPanelBorder.Margin = new Thickness(0);
            }
        }

        private void OnSaveLayout(object sender, RoutedEventArgs e)
        {
            string fileName = (sender as MenuItem).Header.ToString();
            var serializer = new XmlLayoutSerializer(dockManager);
            using (var stream = new StreamWriter(string.Format(@".\AvalonDock_{0}.config", fileName)))
                serializer.Serialize(stream);
        }

        private void OnLoadLayout(object sender, RoutedEventArgs e)
        {
            var currentContentsList = dockManager.Layout.Descendents().OfType<LayoutContent>().Where(c => c.ContentId != null).ToArray();

            string fileName = (sender as MenuItem).Header.ToString();
            var serializer = new XmlLayoutSerializer(dockManager);
            //serializer.LayoutSerializationCallback += (s, args) =>
            //    {
            //        var prevContent = currentContentsList.FirstOrDefault(c => c.ContentId == args.Model.ContentId);
            //        if (prevContent != null)
            //            args.Content = prevContent.Content;
            //    };
            using (var stream = new StreamReader(string.Format(@".\AvalonDock_{0}.config", fileName)))
                serializer.Deserialize(stream);

        }



        private void OnDumpToConsole(object sender, RoutedEventArgs e)
        {
            // Uncomment when TRACE is activated on AvalonDock project
            // dockManager.Layout.ConsoleDump(0);
        }

        private void OnUnloadManager(object sender, RoutedEventArgs e)
        {
        }

        private void OnLoadManager(object sender, RoutedEventArgs e)
        {
        }

        private void OnToolWindow1Hiding(object sender, System.ComponentModel.CancelEventArgs e)
        {
        }

        private void OnShowHeader(object sender, RoutedEventArgs e)
        {
        }

        /// <summary>
        /// Method create a new anchorable window to test whether a floating window will auto-adjust its size to the
        /// containing control. See <see cref="DockingManager.AutoWindowSizeWhenOpened"/> dependency property.
        /// and TestUserControl in this demo App for more details.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNewFloatingWindow(object sender, RoutedEventArgs e)
        {
        }

        private void OnShowToolWindow1(object sender, RoutedEventArgs e)
        {

        }

        private void OnShowWinformsWindow(object sender, RoutedEventArgs e)
        {

        }

        private void CustomTitleBar_MinimizeClick(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;

        private void CustomTitleBar_MaximizeClick(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized) WindowState = WindowState.Normal;
            else WindowState = WindowState.Maximized;
        }

        private void CustomTitleBar_CloseClick(object sender, RoutedEventArgs e) => Application.Current.Shutdown();

        private void CustomTitleBar_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                WindowBorder.BorderThickness = new System.Windows.Thickness(2);
            }
            else
            {
                WindowBorder.BorderThickness = new System.Windows.Thickness(0);
            }
        }
    }
}