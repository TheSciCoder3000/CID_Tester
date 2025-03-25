using CID_Tester.Model;
using CID_Tester.ViewModel.Command;
using System.CodeDom.Compiler;

namespace CID_Tester.ViewModel
{
    public class DashboardMetricViewModel : BaseViewModel
    {
        private readonly Store _AppStore;
        public string DUTName { get => _AppStore.TestPlan!.DUT.DutName; }
        public string DUTDescription { get => _AppStore.TestPlan!.DUT.Description; }
        public string TotalNumberTests { get => $"{_AppStore.TestPlan!.TEST_PARAMETERS.Count} tests"; }
        //public string TestsPassed { get => $"{_AppStore.TestPlan!.TEST_PARAMETERS.Where(par => par.Pass == true).Count()} PASSED"; }
        //public string TestsFailed { get => $"{_AppStore.TestPlan!.TEST_PARAMETERS.Where(par => par.Pass == true).Count()} FAILED"; }
        public string TestStatus
        {
            get
            {
                switch (_AppStore.Testing)
                {
                    case TestingMode.Start: return "Testing In Progress";
                    case TestingMode.Pause: return "Testing on Hold";
                    default: return "Testing Stopped";
                }
            }
        }

        

        public DashboardMetricViewModel(Store appStore)
        {
            _AppStore = appStore;
            _AppStore.OnTesting += (TestingMode _) => onPropertyChanged(nameof(TestStatus));
        }
    }
}
