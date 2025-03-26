
using CID_Tester.Store;
using CID_Tester.ViewModel.Document;

namespace CID_Tester.ViewModel.Command.Navigation;

public class NavigateHistory(AppStore appStore) : CommandBase
{
    private readonly AppStore _AppStore = appStore;

    public override void Execute(object? parameter)
    {
        _AppStore.DocumentStore.AddDocument<HistoryViewModel>(new HistoryViewModel(appStore));
    }
}
