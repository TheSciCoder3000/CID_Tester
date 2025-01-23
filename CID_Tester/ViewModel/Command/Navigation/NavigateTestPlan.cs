using CID_Tester.Model;
using CID_Tester.View.Windows;
using CID_Tester.ViewModel.Document;
using CID_Tester.ViewModel.Windows;

namespace CID_Tester.ViewModel.Command.Navigation;

public class NavigateTestPlan(Store appStore) : CommandBase
{
    private readonly Store _AppStore = appStore;
    private readonly TestPlanViewModel _testPlanView = new TestPlanViewModel(appStore);

    public override void Execute(object? parameter)
    {
        if (_AppStore.TestPlan == null)
        {
            OpenTestPlanView testPlanDialog = new OpenTestPlanView();
            OpenTestPlanViewModel vm = new OpenTestPlanViewModel(_AppStore, () => testPlanDialog.Close());
            testPlanDialog.DataContext = vm;
            testPlanDialog.ShowDialog();
        }

        if (_AppStore.TestPlan != null) _AppStore.AddDocument(_testPlanView);
    }
}
