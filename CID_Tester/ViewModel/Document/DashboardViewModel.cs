using CID_Tester.ViewModel.Command;
using CID_Tester.View.Controls;
using CID_Tester.Model;
using CID_Tester.ViewModel.Interfaces;
using System.Windows.Controls;
using System.Windows.Input;
using CID_Tester.View.Controls.Dashboard;
using CID_Tester.View.Windows;
using CID_Tester.ViewModel.Windows;
using CID_Tester.ViewModel.Controls.AddTestPlan;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CID_Tester.Store;

namespace CID_Tester.ViewModel.Document;

public class DashboardViewModel : BaseViewModel, IDocument
{
    private readonly AppStore _AppStore;

    public string Title { get; set; }
    public string Fullname { get { return _AppStore.TestUser.ToString(); } }

    private ImageSource _userProfile;
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

    public ImageSource UserProfile
    {
        get
        {
            if (_AppStore.TestUser.ProfileImage == "") return new BitmapImage(new Uri("pack://application:,,,/CID_Tester;component/images/temp-profile.png"));
            return new BitmapImage(new Uri(_AppStore.TestUser.ProfileImage));
        }
    }
    public RelayCommand StartTestCommand => new RelayCommand(execute => PlayTestHandller(), canExecute => canPlay());
    public RelayCommand PauseTestCommand => new RelayCommand(execute => PauseTestHandler(), canExecute => canPause());
    public RelayCommand StopTestCommand => new RelayCommand(execute => StopTestHandler(), canExecute => canStop());

    public DashboardViewModel(AppStore appStore, ICommand navigateToTestPlanCommand)
    {
        _AppStore = appStore;
        _AppStore.TestPlanStore.OnTestPlanUpdated += LoadTestMetrics;
        NavigateToTestPlanCommand = new RelayCommand(OpenTestPlan);
        ImportCsvCommand = new RelayCommand(ImportTestPlan);
        Title = "Dashboard";
        CloseCommand = new RelayCommand(CloseCommandHanlder);

        LoadTestMetrics(_AppStore.TestPlanStore.SelectedTestPlan);

    }

    #region Relay Command Handlers
    private void PlayTestHandller() => _AppStore.Testing = TestingMode.Start;
    private bool canPlay() => _AppStore.canTest && _AppStore.Testing != TestingMode.Start;
    private void StopTestHandler() => _AppStore.Testing = TestingMode.Stop;

    private void PauseTestHandler() => _AppStore.Testing = TestingMode.Pause;
    private bool canPause() => _AppStore.canTest && _AppStore.Testing == TestingMode.Start;
    private bool canStop() => _AppStore.canTest && (_AppStore.Testing == TestingMode.Start || _AppStore.Testing == TestingMode.Pause);

    #endregion

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
        _AppStore.TestPlanStore.OnTestPlanUpdated -= LoadTestMetrics;
        _AppStore.RemoveDocument(this);
    }

}
