using CID_Tester.Command;
using CID_Tester.Controls;
using CID_Tester.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

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

        private UserControl _testPlanStatusControl;
        public UserControl TestPlanStatusControl
        {
            get { return _testPlanStatusControl; }
            set
            {
                _testPlanStatusControl = value;
                onPropertyChanged(nameof(TestPlanStatusControl));
            }
        }

        public ICommand NavigateToTestPlanCommand { get; set; }

        public DashboardViewModel(TEST_USER user, string title, ICommand navigateToTestPlanCommand)
        {
            NavigateToTestPlanCommand = navigateToTestPlanCommand;
            Title = title;
            _fullname = user.ToString();
            TestPlanStatusControl = new TestPlanStatusControl();
        }
    }
}
