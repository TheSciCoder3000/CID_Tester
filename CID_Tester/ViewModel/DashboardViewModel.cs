using CID_Tester.Command;
using CID_Tester.Controls;
using CID_Tester.Model;
using Microsoft.Identity.Client;
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
    public class DashboardViewModel : BaseViewModel, IDocument
    {
        private readonly Store _AppStore;

        public string Title { get; set; }
        public string Fullname { get { return _AppStore.TestUser.ToString(); } }

        private UserControl _testPlanStatusControl = null!;
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
        public ICommand CloseCommand { get; }

        public DashboardViewModel(Store appStore, ICommand navigateToTestPlanCommand)
        {
            _AppStore = appStore;
            NavigateToTestPlanCommand = navigateToTestPlanCommand;
            Title = "Dashboard";
            CloseCommand = new RelayCommand(CloseCommandHanlder);


            if (_AppStore.TestPlan == null)
            {
                TestPlanStatusControl = new TestPlanStatusControl();
            }
            else
            {
                TestPlanStatusControl = new DashboardMetricControl()
                {
                    DataContext = new DashboardMetricViewModel(_AppStore.TestPlan)
                };
            }
        }

        private void CloseCommandHanlder(object? parameter) => _AppStore.RemoveDocument(this);

    }
}
