using CID_Tester.ViewModel;

namespace CID_Tester.ViewModel.Command;

class NavigateCommand : CommandBase
{
    private readonly Action<BaseViewModel> _navigateFunc;
    private readonly BaseViewModel _destViewModel;

    public NavigateCommand(Action<BaseViewModel> navigateFunc, BaseViewModel destinationViewModel)
    {
        _navigateFunc = navigateFunc;
        _destViewModel = destinationViewModel;
    }

    public override void Execute(object? parameter)
    {
        _navigateFunc(_destViewModel);
    }
}
