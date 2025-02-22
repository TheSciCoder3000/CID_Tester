using CID_Tester.Model;
using CID_Tester.ViewModel.Document;
using CID_Tester.ViewModel.DebugSDK;

namespace CID_Tester.ViewModel.Command.Navigation;

public class NavigateDebug(Store appStore, PS2000 oscilloscope, PS2000SigGen sigGen) : CommandBase
{
    private readonly Store _AppStore = appStore;

    public override void Execute(object? parameter)
    {
        DebugViewModel viewModel = new DebugViewModel(appStore, oscilloscope, sigGen);
        oscilloscope.DebugVM = viewModel;
        _AppStore.AddDocument<DebugViewModel>(viewModel);
    }
}
