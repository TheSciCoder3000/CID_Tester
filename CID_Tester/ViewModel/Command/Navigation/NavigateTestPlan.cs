using CID_Tester.Model;

namespace CID_Tester.ViewModel.Command.Navigation;

public class NavigateTestPlan(Store appStore) : CommandBase
{
    private readonly Store _AppStore = appStore;
    private readonly TestPlanViewModel _testPlanView = new TestPlanViewModel(appStore);

    public override void Execute(object? parameter)
    {
        _AppStore.AddDocument(_testPlanView);
    }
}
