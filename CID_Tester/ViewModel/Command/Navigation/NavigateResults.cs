using CID_Tester.Model;
using CID_Tester.Store;
using CID_Tester.ViewModel.Document;
using Microsoft.Identity.Client;

namespace CID_Tester.ViewModel.Command.Navigation;

public class NavigateResults : CommandBase
{
    private readonly AppStore _AppStore;

    public NavigateResults(AppStore appStore)
    {
        _AppStore = appStore;
        _AppStore.TestPlanStore.OnTestPlanUpdated += (_) => OnCanExecuteChanged();
    }

    public override bool CanExecute(object? parameter)
    {
        return _AppStore.TestPlanStore.SelectedTestPlan != null && base.CanExecute(parameter);
    }

    public override void Execute(object? parameter)
    {
        _AppStore.AddDocument<ResultsViewModel>(new ResultsViewModel(_AppStore));
    }
}
