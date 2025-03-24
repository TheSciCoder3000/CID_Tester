using CID_Tester.Model;
using CID_Tester.Store;
using CID_Tester.View.Windows;
using CID_Tester.ViewModel.Command;
using CID_Tester.ViewModel.Controls.AddTestPlan;
using CID_Tester.ViewModel.Windows;
using System.Windows.Input;

namespace CID_Tester.ViewModel;

public class ToolbarViewModel : BaseViewModel
{
    private readonly AppStore _AppStore;

    public ToolbarViewModel(AppStore appStore)
    {
        _AppStore = appStore;
        _AppStore.TestPlanStore.OnTestPlanUpdated += updateTestPlanList;
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

    public IEnumerable<TEST_PLAN> TestPlans { get => _AppStore.TestPlanStore.TestPlans; }
    public TEST_PLAN? SelectedTestPlan
    {
        get => _AppStore.TestPlanStore.SelectedTestPlan;
        set
        {
            if (value != null) _AppStore.TestPlanStore.SelectTestPlan(value);
            onPropertyChanged(nameof(SelectedTestPlan));
        }
    }

    public RelayCommand AddTestPlanCommand => new RelayCommand(AddTestPlanHandler, canExecute => canAddTestPlan());
    public RelayCommand ImportTestPlanCommand => new RelayCommand(ImportTestPlanHandler, canExecute => canAddTestPlan());
    public RelayCommand PlayTestCommand => new RelayCommand(execute => PlayTestHandller(), canExecute => canPlay());
    public RelayCommand PauseTestCommand => new RelayCommand(execute => PauseTestHandler(), canExecute => canPause());
    public RelayCommand StopTestCommand => new RelayCommand(execute => StopTestHandler(), canExecute => canStop());
    public RelayCommand ReconnectDevicesCommand => new RelayCommand(execute => _AppStore.ReinitializeTestDevices());

    public bool NotLocked
    {
        get => _AppStore.Testing == TestingMode.Stop;
    }
}
