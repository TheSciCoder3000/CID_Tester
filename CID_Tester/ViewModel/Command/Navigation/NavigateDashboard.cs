using CID_Tester.Model;
using CID_Tester.ViewModel.Document;
using System.Windows.Input;

namespace CID_Tester.ViewModel.Command.Navigation;

public class NavigateDashboard(Store appStore, ICommand navigateCommand) : CommandBase
{
    private readonly Store _AppStore = appStore;

    public override void Execute(object? parameter)
    {
        _AppStore.AddDocument<DashboardViewModel>(new DashboardViewModel(appStore, navigateCommand));
    }
}
