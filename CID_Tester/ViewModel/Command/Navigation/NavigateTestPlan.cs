using CID_Tester.Model;
using CID_Tester.Store;
using CID_Tester.View.Windows;
using CID_Tester.ViewModel.Document;
using CID_Tester.ViewModel.Windows;

namespace CID_Tester.ViewModel.Command.Navigation;

public class NavigateTestPlan(AppStore appStore) : CommandBase
{
    private readonly AppStore _AppStore = appStore;

    public override void Execute(object? parameter)
    {
        if (_AppStore.TestPlanStore.SelectedTestPlan == null)
        {
            OpenTestPlanView testPlanDialog = new OpenTestPlanView();
            OpenTestPlanViewModel vm = new OpenTestPlanViewModel(_AppStore, () => testPlanDialog.Close());
            testPlanDialog.DataContext = vm;
            testPlanDialog.ShowDialog();
        }

        if (_AppStore.TestPlanStore.SelectedTestPlan != null) _AppStore.AddDocument<TestPlanViewModel>(new TestPlanViewModel(appStore));
    }
}
