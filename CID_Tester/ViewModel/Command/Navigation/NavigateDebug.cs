using CID_Tester.Model;
using CID_Tester.ViewModel.Document;
using CID_Tester.ViewModel.DebugSDK;
using CID_Tester.Store;

namespace CID_Tester.ViewModel.Command.Navigation;

public class NavigateDebug : CommandBase
{
    private readonly AppStore _AppStore;
    private readonly PS2000 _Oscilloscope;
    private readonly PS2000SigGen _SigGen;

    public NavigateDebug(AppStore appStore, PS2000 oscilloscope, PS2000SigGen sigGen)
    {
        _AppStore = appStore;
        _Oscilloscope = oscilloscope;
        _SigGen = sigGen;
        _AppStore.TestPlanStore.OnTestPlanUpdated += (_) => OnCanExecuteChanged();
    }

    public override void Execute(object? parameter)
    {
        DebugViewModel viewModel = new DebugViewModel(_AppStore, _Oscilloscope, _SigGen);
        _Oscilloscope.DebugVM = viewModel;
        _AppStore.DocumentStore.AddDocument<DebugViewModel>(viewModel);
    }

    public override bool CanExecute(object? parameter)
    {
        return _AppStore.TestPlanStore.SelectedTestPlan != null && base.CanExecute(parameter);
    }
}
