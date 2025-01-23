using CID_Tester.Model;
using CID_Tester.ViewModel.Document;

namespace CID_Tester.ViewModel.Command.Navigation;

public class NavigateDevices(Store appStore) : CommandBase
{
    private readonly Store _AppStore = appStore;
    private readonly DevicesViewModel _devicesView = new DevicesViewModel(appStore);

    public override void Execute(object? parameter)
    {
        _AppStore.AddDocument(_devicesView);
    }
}
