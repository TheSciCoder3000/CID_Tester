using CID_Tester.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CID_Tester.ViewModel
{
    public class DashboardViewModel : BaseViewModel
    {
        private readonly TEST_USER _user;
        private string _fullname;

        public string Title { get; set; }

        public string Fullname 
        {
            get { return _fullname; }
            set 
            { 
                _fullname = value;
                onPropertyChanged(nameof(Fullname));
            }
        }

        private UserControl _dashboardMetric;
        public UserControl DashboardMetric
        {
            get { return _dashboardMetric; }
            set
            {
                _dashboardMetric = value;
                onPropertyChanged(nameof(DashboardMetric));
            }
        }

        public DashboardViewModel(TEST_USER user, string title)
        {
            Title = title;
            _fullname = user.ToString();
        }
    }
}
