using CID_Tester.Model;
using CID_Tester.ViewModel.Command;
using System.Windows.Input;

namespace CID_Tester.ViewModel.Controls.AddTestPlan;

public class AddTestPlanTableSelectorViewModel : BaseViewModel
{
    private readonly Store _AppStore;
    private readonly Action _closeDialog;

    public ICollection<TEST_PLAN> TestPlans
    {
        get => _AppStore.TEST_PLANS.ToList();
    }

    private TEST_PLAN? _selectedTestPlan;
    public TEST_PLAN? SelectedTestPlan
    {
        get => _selectedTestPlan;
        set
        {
            _selectedTestPlan = value;
            onPropertyChanged(nameof(SelectedTestPlan));
        }
    }

    public AddTestPlanTableSelectorViewModel(Store appStore, Action closeDialog)
    {
        _AppStore = appStore;
        _closeDialog = closeDialog;
        OpenCommand = new RelayCommand(OpenTestPlanHandler);
    }

    private void OpenTestPlanHandler(object? obj)
    {
        if (SelectedTestPlan != null)
        {
            _AppStore.setTestPlan(SelectedTestPlan);
            _closeDialog();
        }
    }

    public ICommand OpenCommand { get; }

}
