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
using System.Windows.Shapes;
using CID_Tester.Model;

namespace CID_Tester
{

    public partial class Login : Window
    {

        DataContext context;
        public Login()
        {
            InitializeComponent();
            context = new DataContext();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void MediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            bg.Position = TimeSpan.FromMilliseconds(1);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            foreach (var user in context.TEST_USER.ToList())
            {
                MessageBox.Show(this, username.Text + " = " + user.USER_NAME + " | " + password.Text + " = " + user.PASSWORD);
                if (username.Text == user.USER_NAME && password.Text == user.PASSWORD)
                {
                    MainWindow main = new MainWindow();
                    main.Show();
                    this.Close();
                }

            }
        }
    }
}
