using CID_Tester.Model;
using CID_Tester.Store;
using CID_Tester.ViewModel.Command;
using System.CodeDom.Compiler;
using System.Windows.Input;

namespace CID_Tester.ViewModel
{
    public class DashboardMetricViewModel : BaseViewModel
    {
        private readonly AppStore _AppStore;
        public string DUTName { get => _AppStore.TestPlanStore.SelectedTestPlan!.DUT.DutName; }
        public string DUTDescription { get => _AppStore.TestPlanStore.SelectedTestPlan!.DUT.Description; }

        private int _testsRemaining;

        private ICollection<TEST_OUTPUT> _outputs;
        public ICollection<TEST_OUTPUT> outputs
        {
            get => _outputs;
            set
            {
                _outputs = value;
                onPropertyChanged(nameof(outputs));
            }
        }
        public int TotalNumberTests
        {
            get => _testsRemaining;
            set
            {
                _testsRemaining = value;
                onPropertyChanged(nameof(TotalNumberTests));
            }
        }

        private int _cyclesCounter;
        public int CyclesCounter
        {
            get => _cyclesCounter;
            set
            {
                _cyclesCounter = value;
                onPropertyChanged(nameof(CyclesCounter));
            }
        }

        public string TestsPassed { get; }
        public string TestsFailed { get; }
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



        public DashboardMetricViewModel(AppStore appStore)
        {
            _AppStore = appStore;
            _AppStore.OnTesting += InitializeTestingHandler;
            _AppStore.TestPlanService.OnTestCompleted += UpdateDashboardTestOutputMetrics;
            TotalNumberTests = _AppStore.TestPlanStore.SelectedTestPlan!.TEST_PARAMETERS.Count;
            CyclesCounter = 0;
        }

        private void InitializeTestingHandler(TestingMode mode)
        {
            if (mode == TestingMode.Start)
            {
                CyclesCounter = 0;
            }
            onPropertyChanged(nameof(TestStatus));
        }

        private void UpdateDashboardTestOutputMetrics(ICollection<TEST_OUTPUT> collection)
        {
            outputs = collection;
            TotalNumberTests -= 1;
            if (TotalNumberTests == 0)
            {
                TotalNumberTests = _AppStore.TestPlanStore.SelectedTestPlan!.TEST_PARAMETERS.Count;
                CyclesCounter += 1;
            }
            CommandManager.InvalidateRequerySuggested();
        }
    }
}
