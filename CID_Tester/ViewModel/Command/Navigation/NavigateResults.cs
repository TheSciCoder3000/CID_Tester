using CID_Tester.Model;
using CID_Tester.ViewModel.Document;
using Microsoft.Identity.Client;

namespace CID_Tester.ViewModel.Command.Navigation;

public class NavigateResults : CommandBase
{
    private readonly Store _AppStore;

    public NavigateResults(Store appStore)
    {
        _AppStore = appStore;
        _AppStore.OnTestPlanUpdated += (_) => OnCanExecuteChanged();
    }

    public override bool CanExecute(object? parameter)
    {
        return _AppStore.TestPlan != null && base.CanExecute(parameter);
    }

    public override void Execute(object? parameter)
    {
        _AppStore.AddDocument<ResultsViewModel>(new ResultsViewModel(_AppStore));
    }
}
