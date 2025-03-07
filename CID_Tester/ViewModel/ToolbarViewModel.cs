﻿using CID_Tester.Model;
using CID_Tester.View.Windows;
using CID_Tester.ViewModel.Command;
using CID_Tester.ViewModel.Controls.AddTestPlan;
using CID_Tester.ViewModel.Windows;

namespace CID_Tester.ViewModel;

public class ToolbarViewModel : BaseViewModel
{
    private readonly Store _AppStore;

    public ToolbarViewModel(Store appStore)
    {
        _AppStore = appStore;
        _AppStore.OnTestPlanUpdated += updateTestPlanList;
        _AppStore.OnTesting += (TestingMode testing) => onPropertyChanged(nameof(NotLocked));
    }



    #region Command Handlers
    private void PlayTestHandller() => _AppStore.Testing = TestingMode.Start;

    private void PauseTestHandler() => _AppStore.Testing = TestingMode.Pause;

    private void StopTestHandler() => _AppStore.Testing = TestingMode.Stop;

    private bool canAddTestPlan() => _AppStore.Testing == TestingMode.Stop;
    private bool canPlay() => _AppStore.canTest && _AppStore.Testing != TestingMode.Start;
    private bool canPause() => _AppStore.canTest && _AppStore.Testing == TestingMode.Start;
    private bool canStop() => _AppStore.canTest && (_AppStore.Testing == TestingMode.Start || _AppStore.Testing == TestingMode.Pause);
    private void AddTestPlanHandler(object? obj)
    {
        OpenTestPlanView testPlanDialog = new OpenTestPlanView();
        Action CloseDialog = () => testPlanDialog.Close();
        OpenTestPlanViewModel vm = new OpenTestPlanViewModel(_AppStore, CloseDialog, new AddTestPlanViewModel(_AppStore, CloseDialog));
        testPlanDialog.DataContext = vm;
        testPlanDialog.ShowDialog();
    }
    private void ImportTestPlanHandler(object? obj)
    {
        OpenTestPlanView testPlanDialog = new OpenTestPlanView();
        Action CloseDialog = () => testPlanDialog.Close();
        OpenTestPlanViewModel vm = new OpenTestPlanViewModel(_AppStore, CloseDialog, new AddTestPlanImporterViewModel(_AppStore, CloseDialog));
        testPlanDialog.DataContext = vm;
        testPlanDialog.ShowDialog();
    }


    #endregion

    private void updateTestPlanList(TEST_PLAN? testPlan)
    {
        onPropertyChanged(nameof(TestPlans));
        onPropertyChanged(nameof(SelectedTestPlan));
    }

    public IEnumerable<TEST_PLAN> TestPlans { get => _AppStore.TestUser.TEST_PLANS; }
    public TEST_PLAN? SelectedTestPlan
    {
        get => _AppStore.TestPlan;
        set
        {
            if (value != null) _AppStore.setTestPlan(value);
            onPropertyChanged(nameof(SelectedTestPlan));
        }
    }

    public RelayCommand AddTestPlanCommand => new RelayCommand(AddTestPlanHandler, canExecute => canAddTestPlan());
    public RelayCommand ImportTestPlanCommand => new RelayCommand(ImportTestPlanHandler, canExecute => canAddTestPlan());
    public RelayCommand PlayTestCommand => new RelayCommand(execute => PlayTestHandller(), canExecute => canPlay());
    public RelayCommand PauseTestCommand => new RelayCommand(execute => PauseTestHandler(), canExecute => canPause());
    public RelayCommand StopTestCommand => new RelayCommand(execute => StopTestHandler(), canExecute => canStop());

    public bool NotLocked
    {
        get => _AppStore.Testing == TestingMode.Stop;
    }
}
