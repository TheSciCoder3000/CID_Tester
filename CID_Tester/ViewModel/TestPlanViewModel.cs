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


        private ICollection<TEST_PARAMETER> _testParameters;
        public ICollection<TEST_PARAMETER> TestParameters
        {
            get => _testParameters;
            set
            {
                _testParameters = value;
                onPropertyChanged(nameof(TestParameters));
            }
        }

        private TEST_PARAMETER _selectedTestParameter = null!;
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
        public ICommand DoubleClickCommand { get; }
        public ICommand AddTestParameterCommand { get; }
        public ICommand DeleteTestParameterCommand { get; }

        public TestPlanViewModel(Store appStore)
        {
            _AppStore = appStore;
            _AppStore.OnTestParameterUpdated += LoadTestParameters;

            Title = "Test Plan";
            CloseCommand = new RelayCommand(CloseCommandHanlder);

            AddTestParameterCommand = new RelayCommand(AddTestParameterHandler);
            DoubleClickCommand = new RelayCommand(DoubleClickHandler);
            DeleteTestParameterCommand = new RelayCommand(DeleteTestParameterHandler);
        }

        private async void DeleteTestParameterHandler(object? obj) => await _AppStore.DeleteTestParameter(SelectedTestParameter);

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

        private void LoadTestParameters(IEnumerable<TEST_PARAMETER> testParameters) => TestParameters = testParameters.ToList();

        private void CloseCommandHanlder(object? parameter) => _AppStore.RemoveDocument(this);
    }
}
