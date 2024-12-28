using CID_Tester.Command;
using CID_Tester.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CID_Tester.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private readonly UserViewModel _user;
        public UserViewModel User => _user;

        private BaseViewModel _activeDocument;
        public ObservableCollection<BaseViewModel> Documents { get; } = new ObservableCollection<BaseViewModel>();
        
        public BaseViewModel ActiveDocument
        {
            get
            {
                return _activeDocument;
            }
            set
            {
                _activeDocument = value;
                onPropertyChanged(nameof(ActiveDocument));
            }
        }

        public ICommand NavigateToDashboard { get; set; }
        public ICommand NavigateToTestPlan { get; set; }
        public ICommand NavigateToResults { get; set; }
        public ICommand NavigateToSettings { get; set; }

        public MainViewModel(TEST_USER user)
        {
            _user = new UserViewModel(user);
            NavigateToDashboard = new NavigateCommand(this, new DashboardViewModel(user, "Dashboard"));
            NavigateToTestPlan = new NavigateCommand(this, new TestPlanViewModel(user, "Test Plan"));
            NavigateToResults = new NavigateCommand(this, new DashboardViewModel(user, "Results Overview"));
            NavigateToSettings = new NavigateCommand(this, new DashboardViewModel(user, "Settings"));
        }

        public void NavigateTo(BaseViewModel viewModel)
        {
            if (Documents.Contains(viewModel))
            {
                ActiveDocument = viewModel;
            }
            else
            {
                Documents.Add(viewModel);
                ActiveDocument = viewModel;
            }
        }
    }
}
