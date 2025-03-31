



using CID_Tester.Model;
using CID_Tester.Store;
using CID_Tester.ViewModel.Command;
using System.CodeDom.Compiler;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CID_Tester.ViewModel
{
    public class DashboardMetricViewModel : BaseViewModel
    {
        private readonly AppStore _AppStore;
        public string DUTName { get => _AppStore.TestPlanStore.SelectedTestPlan!.DUT.DutName; }
        public string DUTDescription { get => _AppStore.TestPlanStore.SelectedTestPlan!.DUT.Description; }

        private int _maxTests;
        public int MaxTests
        {

            get => _maxTests;
            set
            {
                _maxTests = value;
                onPropertyChanged(nameof(MaxTests));
            }
        }

        private int _currentTest;
        public int CurrentTest
        {

            get => _currentTest;
            set
            {
                _currentTest = value;
                onPropertyChanged(nameof(CurrentTest));
            }
        }

        private String _loadingStatus;
        public String LoadingStatus
        {

            get => _loadingStatus;
            set
            {
                _loadingStatus = value;
                onPropertyChanged(nameof(LoadingStatus));
            }
        }


        private int _testsRemaining;

        private ObservableCollection<TEST_OUTPUT> _outputs;
        public ObservableCollection<TEST_OUTPUT> outputs
        {
            get => _outputs;
            set
            {
                _outputs = value;
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
            _AppStore.TestPlanService.OnDUTCompleted += UpdateDashboardTable;
            TotalNumberTests = _AppStore.TestPlanStore.SelectedTestPlan!.TEST_PARAMETERS.Count;
            CyclesCounter = 0;
            LoadingStatus = "Parameter 0 of 0";
            MaxTests = 0;
            CurrentTest = 0;
            outputs = [];
        }

        private void InitializeTestingHandler(TestingMode mode)
        {

            if (mode == TestingMode.Start)
            {
                CyclesCounter = 0;
                MaxTests = _AppStore.TestPlanStore.SelectedTestPlan!.TEST_PARAMETERS.Count * 4 * 3;
                CurrentTest = 0;
                if (outputs != null) outputs.Clear();
            }
            else if (mode == TestingMode.Stop)
            {

                MaxTests = _AppStore.TestPlanStore.SelectedTestPlan!.TEST_PARAMETERS.Count * 4 * 3;
                CurrentTest = 0;
            }

            onPropertyChanged(nameof(TestStatus));
        }

        private void UpdateDashboardTestOutputMetrics(ICollection<TEST_OUTPUT> collection)
        {
            TotalNumberTests -= 1;

            if (TotalNumberTests == 0)
            {
                TotalNumberTests = _AppStore.TestPlanStore.SelectedTestPlan!.TEST_PARAMETERS.Count;
                CyclesCounter += 1;
            }
        }

        private void UpdateDashboardTable(TEST_OUTPUT output)
        {
            CurrentTest++;
            LoadingStatus = $"Parameter {CurrentTest} of {MaxTests}";
            outputs.Add(output);
        }

    }
}
