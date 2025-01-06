using CID_Tester.Model;
using Microsoft.Identity.Client;

namespace CID_Tester.ViewModel.Command.Navigation;

public class NavigateResults : CommandBase
{
    private readonly Store _AppStore;
    private readonly ResultsViewModel _resultsView;

    public NavigateResults(Store appStore)
    {
        _AppStore = appStore;
        _resultsView = new ResultsViewModel(appStore);
        _AppStore.OnTestPlanUpdated += (_) => OnCanExecuteChanged();
    }

    public override bool CanExecute(object? parameter)
    {
        return _AppStore.TestPlan != null && base.CanExecute(parameter);
    }

    public override void Execute(object? parameter)
    {
        _AppStore.AddDocument(_resultsView);
    }
}
