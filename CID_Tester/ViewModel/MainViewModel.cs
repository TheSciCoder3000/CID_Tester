using CID_Tester.Command;
using CID_Tester.Model;
using CID_Tester.Service.DbCreator;
using CID_Tester.Service.DbProvider;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CID_Tester.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        public ObservableCollection<BaseViewModel> Documents { get; } = new ObservableCollection<BaseViewModel>();
        
        private object _activeDocument = null!;
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
        public ICommand NavigateToDevices { get; set; }
        public ICommand NavigateToTestPlan { get; set; }
        public ICommand NavigateToResults { get; set; }
        public ICommand NavigateToSettings { get; set; }


        public MainViewModel(TEST_USER user, IDbProvider dbProvider, IDbCreator dbCreator)
        {
            NavigateToTestPlan = new NavigateCommand(this, new TestPlanViewModel(user, "Test Plan"));
            NavigateToDevices = new NavigateCommand(this, new DevicesViewModel("Devices", dbProvider, dbCreator));
            NavigateToDashboard = new NavigateCommand(this, new DashboardViewModel(user, "Dashboard", NavigateToTestPlan));
            NavigateToResults = new NavigateCommand(this, new ResultsViewModel(user, "Results Overview"));
            NavigateToSettings = new NavigateCommand(this, new ResultsViewModel(user, "Settings"));
        }
    }
}
