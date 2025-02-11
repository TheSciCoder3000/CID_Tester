using CID_Tester.Model;
using CID_Tester.ViewModel.Document;

namespace CID_Tester.ViewModel.Command.Navigation;

public class NavigateDebug(Store appStore) : CommandBase
{
    private readonly Store _AppStore = appStore;

    public override void Execute(object? parameter)
    {
        _AppStore.AddDocument<DebugViewModel>(new DebugViewModel(appStore));
    }
}
