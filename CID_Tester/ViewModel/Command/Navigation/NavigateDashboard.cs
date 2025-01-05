using CID_Tester.Model;
using System.Windows.Input;

namespace CID_Tester.ViewModel.Command.Navigation;

public class NavigateDashboard(Store appStore, ICommand navigateCommand) : CommandBase
{
    private readonly Store _AppStore = appStore;
    private readonly DashboardViewModel _dashboardView = new DashboardViewModel(appStore, navigateCommand);

    public override void Execute(object? parameter)
    {
        _AppStore.AddDocument(_dashboardView);
    }
}
