using CID_Tester.ViewModel.Command;
using CID_Tester.View.Controls;
using CID_Tester.Model;
using CID_Tester.ViewModel.Interfaces;
using System.Windows.Controls;
using System.Windows.Input;

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

    public ICommand NavigateToTestPlanCommand { get; set; }
    public ICommand CloseCommand { get; }

    public DashboardViewModel(Store appStore, ICommand navigateToTestPlanCommand)
    {
        _AppStore = appStore;
        NavigateToTestPlanCommand = navigateToTestPlanCommand;
        Title = "Dashboard";
        CloseCommand = new RelayCommand(CloseCommandHanlder);


        if (_AppStore.TestPlan == null)
        {
            TestPlanStatusControl = new TestPlanStatusControl();
        }
        else
        {
            TestPlanStatusControl = new DashboardMetricControl()
            {
                DataContext = new DashboardMetricViewModel(_AppStore.TestPlan)
            };
        }
    }

    private void CloseCommandHanlder(object? parameter) => _AppStore.RemoveDocument(this);

}
