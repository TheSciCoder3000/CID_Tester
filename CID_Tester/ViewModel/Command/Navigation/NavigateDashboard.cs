using CID_Tester.Model;
using CID_Tester.Store;
using CID_Tester.ViewModel.Document;
using System.Windows.Input;

namespace CID_Tester.ViewModel.Command.Navigation;

public class NavigateDashboard(AppStore appStore, ICommand navigateCommand) : CommandBase
{
    private readonly AppStore _AppStore = appStore;

    public override void Execute(object? parameter)
    {
        _AppStore.DocumentStore.AddDocument<DashboardViewModel>(new DashboardViewModel(appStore, navigateCommand));
    }
}
