using CID_Tester.Model;
using CID_Tester.ViewModel.Document;
using CID_Tester.ViewModel.DebugSDK;
using CID_Tester.Store;

namespace CID_Tester.ViewModel.Command.Navigation;

public class NavigateDebug(AppStore appStore, PS2000 oscilloscope, PS2000SigGen sigGen) : CommandBase
{
    private readonly AppStore _AppStore = appStore;

    public override void Execute(object? parameter)
    {
        DebugViewModel viewModel = new DebugViewModel(appStore, oscilloscope, sigGen);
        oscilloscope.DebugVM = viewModel;
        _AppStore.AddDocument<DebugViewModel>(viewModel);
    }
}
