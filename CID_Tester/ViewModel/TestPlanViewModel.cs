﻿using CID_Tester.ViewModel.Command;
using CID_Tester.Model;
using CID_Tester.View.Windows;
using CID_Tester.View;
using CID_Tester.ViewModel.Interfaces;
using System.Windows.Input;
using CID_Tester.ViewModel.Windows;

namespace CID_Tester.ViewModel
{
    public class TestPlanViewModel : BaseViewModel, IDocument
    {
        private readonly Store _AppStore;
        public string Title { get; }

        public bool ShowParameterTable { get; set; } = false;
        private ICollection<TEST_PARAMETER>? _parameters = [];
        public ICollection<TEST_PARAMETER>? TestParameters
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
        public ICommand AddTestParameterCommand { get; }

        public TestPlanViewModel(Store appStore)
        {
            _AppStore = appStore;
            _AppStore.OnTestParameterUpdated += LoadTestParameters;
            Title = "Test Plan";
            CloseCommand = new RelayCommand(CloseCommandHanlder);
            CreateTestPlanCommand = new RelayCommand(CreateTestPlanHandler);
            AddTestParameterCommand = new RelayCommand(AddTestParameterHandler);
            TestParameters = _AppStore.TestPlan?.TEST_PARAMETERS;
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
            TestParameters = testParameters.ToList();
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
