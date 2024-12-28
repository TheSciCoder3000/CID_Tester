using CID_Tester.Command;
using CID_Tester.Model;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CID_Tester.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        public ObservableCollection<BaseViewModel> Documents { get; } = new ObservableCollection<BaseViewModel>();
        
        private object _activeDocument;
        public object ActiveDocument
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
            NavigateToDashboard = new NavigateCommand(this, new DashboardViewModel(user, "Dashboard"));
            NavigateToTestPlan = new NavigateCommand(this, new TestPlanViewModel(user, "Test Plan"));
            NavigateToResults = new NavigateCommand(this, new ResultsViewModel(user, "Results Overview"));
            NavigateToSettings = new NavigateCommand(this, new DashboardViewModel(user, "Settings"));
        }

        public void NavigateTo(BaseViewModel viewModel)
        {
            if (!Documents.Contains(viewModel))
            {
                Documents.Add(viewModel);
            }
            ActiveDocument = Documents[Documents.IndexOf(viewModel)];
        }
    }
}
