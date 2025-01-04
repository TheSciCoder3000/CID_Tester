using CID_Tester.ViewModel.Command;
using CID_Tester.Model;
using CID_Tester.View.Windows;
using CID_Tester.View;
using CID_Tester.ViewModel.Interfaces;
using System.Windows.Input;
using CID_Tester.ViewModel.Windows;
using System.ComponentModel;
using System.Windows.Data;
using CID_Tester.ViewModel.Controls;
using System.Collections.ObjectModel;

namespace CID_Tester.ViewModel
{
    public class TestPlanViewModel : BaseViewModel, IDocument
    {
        private readonly Store _AppStore;
        public string Title { get; }

        public bool ShowParameterTable { get; set; } = false;
        private ICollection<TEST_PARAMETER>? _parameters = [];
        public ICollectionView? TestParameters
        {
            get => CollectionViewSource.GetDefaultView(_parameters);
            set {
                if (value == null) _parameters = null;
                else _parameters = ((IEnumerable<TEST_PARAMETER>)value.SourceCollection).ToList();
                ShowParameterTable = value != null;
                onPropertyChanged(nameof(ShowParameterTable));
                onPropertyChanged(nameof(TestParameters));
            }
        }

        public ObservableCollection<TEST_PLAN> TestPlanList => new ObservableCollection<TEST_PLAN>(_AppStore.TestUser.TEST_PLANS);

        private TEST_PLAN _selectedTestPlan;
        public TEST_PLAN SelectedTestPlan
        {
            get => _selectedTestPlan;
            set
            {
                _selectedTestPlan = value;
                onPropertyChanged(nameof(SelectedTestPlan));
            }
        }

        private TEST_PARAMETER _selectedTestParameter;
        public TEST_PARAMETER SelectedTestParameter
        {
            get => _selectedTestParameter;
            set
            {
                _selectedTestParameter = value;
                onPropertyChanged(nameof(SelectedTestParameter));
            }
        }

        public ICommand CloseCommand { get; }
        public ICommand CreateTestPlanCommand { get; }
        public ICommand DoubleClickCommand { get; }
        public ICommand AddTestParameterCommand { get; }
        public ICommand DeleteTestParameterCommand { get; }
        public ICommand OpenTestPlanCommand { get; }

        public TestPlanViewModel(Store appStore)
        {
            _AppStore = appStore;
            _AppStore.OnTestParameterUpdated += LoadTestParameters;
            Title = "Test Plan";
            CloseCommand = new RelayCommand(CloseCommandHanlder);
            CreateTestPlanCommand = new RelayCommand(CreateTestPlanHandler);
            AddTestParameterCommand = new RelayCommand(AddTestParameterHandler);
            TestParameters = CollectionViewSource.GetDefaultView(_AppStore.TestPlan?.TEST_PARAMETERS);
            DoubleClickCommand = new RelayCommand(DoubleClickHandler);
            DeleteTestParameterCommand = new RelayCommand(DeleteTestParameterHandler);
            OpenTestPlanCommand = new RelayCommand(OpenTestPlanHandler);
        }

        private async void DeleteTestParameterHandler(object? obj) => await _AppStore.DeleteTestParameter(SelectedTestParameter);

        private void OpenTestPlanHandler(object? obj)
        {
            if (SelectedTestPlan != null) _AppStore.setTestPlan(SelectedTestPlan);
        }

        private void DoubleClickHandler(object? obj)
        {
            if (obj is TEST_PARAMETER && obj != null)
            {
                TEST_PARAMETER parameter = (TEST_PARAMETER)obj;
                _AppStore.ClearAnchorables();
                _AppStore.AddAnchorables(new TestParameterPropertiesViewModel(_AppStore, parameter));
            }
        }

        private void AddTestParameterHandler(object? obj)
        {
            AddParameterView addParameterView = new AddParameterView();
            AddParameterViewModel vm = new AddParameterViewModel(_AppStore, () => addParameterView.Close());
            addParameterView.DataContext = vm;
            addParameterView.ShowDialog();
        }

        private void LoadTestParameters(IEnumerable<TEST_PARAMETER> testParameters)
        {
            TestParameters = CollectionViewSource.GetDefaultView(testParameters);
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
