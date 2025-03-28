﻿using CID_Tester.ViewModel.Command;
using CID_Tester.Model;
using CID_Tester.View.Windows;
using CID_Tester.ViewModel.Interfaces;
using System.Windows.Input;
using CID_Tester.ViewModel.Windows;
using CID_Tester.ViewModel.Anchorables;
using CID_Tester.Store;

namespace CID_Tester.ViewModel.Document;

public class TestPlanViewModel : BaseViewModel, IDocument
{
    private readonly AppStore _AppStore;
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

    private TEST_PARAMETER? _selectedTestParameter = null!;
    public int? SelectedTestParameterId
    {
        get => _selectedTestParameter?.ParamCode;
        set
        {
            _selectedTestParameter = TestParameters.FirstOrDefault(param => param.ParamCode == value);
            onPropertyChanged(nameof(SelectedTestParameterId));
        }
    }

    public ICommand CloseCommand { get; }
    public ICommand DoubleClickCommand { get; }
    public ICommand AddTestParameterCommand { get; }
    public ICommand DeleteTestParameterCommand { get; }

    public TestPlanViewModel(AppStore appStore)
    {
        _AppStore = appStore;
        _AppStore.OnTestParameterUpdated += LoadTestParameters;
        LoadTestParameters(_AppStore.TestPlanStore.SelectedTestPlan!.TEST_PARAMETERS);

        Title = "Test Plan";
        CloseCommand = new RelayCommand(CloseCommandHanlder);

        AddTestParameterCommand = new RelayCommand(AddTestParameterHandler);
        DoubleClickCommand = new RelayCommand(DoubleClickHandler);
        DeleteTestParameterCommand = new RelayCommand(DeleteTestParameterHandler);
    }

    private async void DeleteTestParameterHandler(object? obj)
    {
        if (_selectedTestParameter != null) await _AppStore.TestPlanStore.DeleteTestParameter(_selectedTestParameter);
    }

    private void DoubleClickHandler(object? obj)
    {
        if (_selectedTestParameter != null)
        {
            _AppStore.DocumentStore.ClearAnchorables();
            _AppStore.DocumentStore.AddAnchorables(new TestParameterPropertiesViewModel(_AppStore, _selectedTestParameter));
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

    private void CloseCommandHanlder(object? parameter)
    {
        _AppStore.OnTestParameterUpdated -= LoadTestParameters;
        _AppStore.DocumentStore.RemoveDocument(this);
    }
}
