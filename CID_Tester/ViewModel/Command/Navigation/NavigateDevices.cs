using CID_Tester.Model;
using CID_Tester.Store;
using CID_Tester.ViewModel.Document;

namespace CID_Tester.ViewModel.Command.Navigation;

public class NavigateDevices(AppStore appStore) : CommandBase
{
    private readonly AppStore _AppStore = appStore;

    public override void Execute(object? parameter)
    {
        _AppStore.AddDocument<DevicesViewModel>(new DevicesViewModel(appStore));
    }
}
