using CID_Tester.Command;
using CID_Tester.Model;
using CID_Tester.View;
using System.Windows.Input;

namespace CID_Tester.ViewModel
{
    public class TestPlanViewModel : BaseViewModel, IDocument
    {
        private readonly Store _AppStore;
        public string Title { get; }

        public bool ShowParameterTable { get; set; } = false;
        private IEnumerable<TEST_PARAMETER>? _parameters = null;
        public IEnumerable<TEST_PARAMETER>? TestParameters
        {
            get => _parameters;
            set {
                _parameters = value;
                ShowParameterTable = value != null;
                onPropertyChanged(nameof(ShowParameterTable));
                onPropertyChanged(nameof(TestParameters));
            }
        }

        public ICommand CloseCommand { get; }
        public ICommand CreateTestPlanCommand { get; }
        public ICommand DoubleClickCommand { get; }

        public TestPlanViewModel(Store appStore)
        {
            _AppStore = appStore;
            _AppStore.OnTestParameterUpdated += LoadTestParameters;
            Title = "Test Plan";
            CloseCommand = new RelayCommand(CloseCommandHanlder);
            CreateTestPlanCommand = new RelayCommand(CreateTestPlanHandler);
            TestParameters = _AppStore.TestPlan?.TEST_PARAMETERS;
        }

        private void LoadTestParameters(IEnumerable<TEST_PARAMETER> testParameters)
        {
            TestParameters = testParameters;
        }

        private void CreateTestPlanHandler(object? obj)
        {
            AddTestPlanView testPlanViewModal = new AddTestPlanView();
            testPlanViewModal.DataContext = new AddTestPlanViewModel(_AppStore, () => testPlanViewModal.Close());
            testPlanViewModal.ShowDialog();

        }

        private void CloseCommandHanlder(object? parameter) => _AppStore.RemoveDocument(this);
    }
}
