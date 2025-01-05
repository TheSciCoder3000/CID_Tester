using CID_Tester.Model;

namespace CID_Tester.ViewModel.Command.Navigation;

public class NavigateResults(Store appStore) : CommandBase
{
    private readonly Store _AppStore = appStore;
    private readonly ResultsViewModel _resultsView = new ResultsViewModel(appStore);
    public override void Execute(object? parameter)
    {
        _AppStore.AddDocument(_resultsView);
    }
}
