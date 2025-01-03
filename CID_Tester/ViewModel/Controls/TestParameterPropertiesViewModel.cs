using CID_Tester.Model;
using CID_Tester.ViewModel.Command;
using CID_Tester.ViewModel.Interfaces;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CID_Tester.ViewModel.Controls;

public class TestParameterPropertiesViewModel : BaseViewModel, IDocument
{
    private readonly Store _AppStore;
    public TestParameterPropertiesViewModel(Store appStore, TEST_PARAMETER testParameter)
    {
        _AppStore = appStore;
        Name = testParameter.Name;
        SelectedDevice = testParameter.TestPlan.DUT;
        Metric = testParameter.Metric;
        Target = testParameter.Target;
        Title = "Test Parameter Properties";
        CloseCommand = new RelayCommand(CloseAnchorable);
    }

    private void CloseAnchorable(object? obj)
    {
        _AppStore.RemoveAnchorable(this);
    }

    private string _name;
    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            onPropertyChanged(nameof(Name));
        }
    }

    public ObservableCollection<DUT> Devices => new ObservableCollection<DUT>(_AppStore.DUTs);

    private DUT _selectedDevice;
    public DUT SelectedDevice
    {
        get => _selectedDevice;
        set
        {
            _selectedDevice = value;
            onPropertyChanged(nameof(SelectedDevice));
        }
    }

    private string _metric;
    public string Metric
    {
        get => _metric;
        set
        {
            _metric = value;
            onPropertyChanged(nameof(Metric));
        }
    }

    private Decimal _target;
    public Decimal Target
    {
        get => _target;
        set
        {
            _target = value;
            onPropertyChanged(nameof(Target));
        }
    }

    public string Title { get; }

    public ICommand CloseCommand { get; }
}
