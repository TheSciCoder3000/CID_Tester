﻿using System;
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
using CID_Tester.ViewModel;

namespace CID_Tester
{

    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
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
    }
}
