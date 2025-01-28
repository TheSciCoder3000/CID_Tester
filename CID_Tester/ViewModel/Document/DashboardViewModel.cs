using CID_Tester.ViewModel.Command;
using CID_Tester.View.Controls;
using CID_Tester.Model;
using CID_Tester.ViewModel.Interfaces;
using System.Windows.Controls;
using System.Windows.Input;
using CID_Tester.View.Controls.Dashboard;
using Microsoft.Identity.Client;
using CID_Tester.View.Windows;
using CID_Tester.ViewModel.Windows;
using CID_Tester.ViewModel.Controls.AddTestPlan;

namespace CID_Tester.ViewModel.Document;

public class DashboardViewModel : BaseViewModel, IDocument
{
    private readonly Store _AppStore;

    public string Title { get; set; }
    public string Fullname { get { return _AppStore.TestUser.ToString(); } }

    private UserControl _testPlanStatusControl = null!;
    public UserControl TestPlanStatusControl
    {
        get { return _testPlanStatusControl; }
        set
        {
            _testPlanStatusControl = value;
            onPropertyChanged(nameof(TestPlanStatusControl));
        }
    }

    public ICommand ImportCsvCommand { get; }
    public ICommand NavigateToTestPlanCommand { get; }
    public ICommand CloseCommand { get; }

    public DashboardViewModel(Store appStore, ICommand navigateToTestPlanCommand)
    {
        _AppStore = appStore;
        _AppStore.OnTestPlanUpdated += LoadTestMetrics;
        NavigateToTestPlanCommand = new RelayCommand(OpenTestPlan);
        ImportCsvCommand = new RelayCommand(ImportTestPlan);
        Title = "Dashboard";
        CloseCommand = new RelayCommand(CloseCommandHanlder);

        LoadTestMetrics(_AppStore.TestPlan);

    }

    #region Open Test Plan Commands
    private void ImportTestPlan(object? obj)
    {
        OpenTestPlanView testPlanDialog = new OpenTestPlanView();
        Action CloseDialog = () => testPlanDialog.Close();
        OpenTestPlanViewModel vm = new OpenTestPlanViewModel(_AppStore, CloseDialog, new AddTestPlanImporterViewModel(_AppStore, CloseDialog));
        testPlanDialog.DataContext = vm;
        testPlanDialog.ShowDialog();
    }

    private void OpenTestPlan(object? obj)
    {
        OpenTestPlanView testPlanDialog = new OpenTestPlanView();
        Action CloseDialog = () => testPlanDialog.Close();
        OpenTestPlanViewModel vm = new OpenTestPlanViewModel(_AppStore, CloseDialog, new AddTestPlanTableSelectorViewModel(_AppStore, CloseDialog));
        testPlanDialog.DataContext = vm;
        testPlanDialog.ShowDialog();
    }
    #endregion

    private void LoadTestMetrics(TEST_PLAN? testPlan)
    {
        if (testPlan == null)
        {
            TestPlanStatusControl = new TestPlanStatus();
        }
        else
        {
            TestPlanStatusControl = new DashboardMetricControl()
            {
                DataContext = new DashboardMetricViewModel(_AppStore)
            };
        }
    }

    private void CloseCommandHanlder(object? parameter)
    {
        _AppStore.OnTestPlanUpdated -= LoadTestMetrics;
        _AppStore.RemoveDocument(this);
    }

}
