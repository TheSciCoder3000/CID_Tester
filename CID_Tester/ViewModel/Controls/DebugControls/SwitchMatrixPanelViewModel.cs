using CID_Tester.Model;
using CID_Tester.Service.Serial;
using CID_Tester.Store;
using CID_Tester.ViewModel.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CID_Tester.ViewModel.Controls.DebugControls;

public class SwitchMatrixPanelViewModel
{


    private PowerSupply _powerSupplyService;
    private SwitchMatrix _switchMatrixService;
    private Measure _measureService;

    private AppStore _appStore;


    public event PropertyChangedEventHandler PropertyChanged;

    private string _invValue;
    public string InvValue
    {
        get { return _invValue; }
        set
        {
            _invValue = value;
            // Call OnPropertyChanged whenever the property is updated
            OnPropertyChanged();
        }
    }

    private string _ninvValue;
    public string NinvValue
    {
        get { return _ninvValue; }
        set
        {
            _ninvValue = value;
            // Call OnPropertyChanged whenever the property is updated
            OnPropertyChanged();
        }
    }
    public ICommand ToggleDut1Command { get; }
    public ICommand ToggleDut2Command { get; }
    public ICommand ToggleDut3Command { get; }
    public ICommand ToggleDut4Command { get; }
    public ICommand ToggleOffCommand { get; }

    public ICommand ToggleInvCommand { get; }
    public ICommand ToggleNinvCommand { get; }

    public ICommand MeasureCommand { get; }

    public IEnumerable<DebugParameterItemViewModel> ParameterItems { get; set; }

    public SwitchMatrixPanelViewModel(AppStore appstore)
    {
        _appStore = appstore;
        ParameterItems = _appStore.TestPlanStore.SelectedTestPlan.TEST_PARAMETERS.Select(param => new DebugParameterItemViewModel(param, _switchMatrixService));
        ToggleDut1Command = new RelayCommand(ToggleDut1Command_Handler);
        ToggleDut2Command = new RelayCommand(ToggleDut2Command_Handler);
        ToggleDut3Command = new RelayCommand(ToggleDut3Command_Handler);
        ToggleDut4Command = new RelayCommand(ToggleDut4Command_Handler);
        ToggleOffCommand = new RelayCommand(ToggleOffCommand_Handler);
        ToggleInvCommand = new RelayCommand(ToggleInvCommand_Handler);
        ToggleNinvCommand = new RelayCommand(ToggleNinvCommand_Handler);
        Initialize();
    }

    private int Initialize()
    {
        _powerSupplyService = new PowerSupply();
        _measureService = new Measure();
        _switchMatrixService = new SwitchMatrix();

        return _measureService.Unconnected() + _powerSupplyService.Unconnected() + _switchMatrixService.Unconnected();
    }

    private void ToggleDut1Command_Handler(object? obj)
    {
        _switchMatrixService.ChangeDut(1);
    }

    private void ToggleDut2Command_Handler(object? obj)
    {

        _switchMatrixService.ChangeDut(2);
    }

    private void ToggleDut3Command_Handler(object? obj)
    {

        _switchMatrixService.ChangeDut(3);
    }

    private void ToggleDut4Command_Handler(object? obj)
    {

        _switchMatrixService.ChangeDut(4);
    }

    private void ToggleOffCommand_Handler(object? obj)
    {
        _switchMatrixService.DutOff();
    }

    private async void ToggleInvCommand_Handler(object? obj)
    {
        _powerSupplyService.ClosePMU();
        await Task.Delay(500);
        String command = $"PMU1 = ON, PMU2 = OFF, Input = {InvValue}";
        await _powerSupplyService.StartPMU("DC", command);
    }

    private async void ToggleNinvCommand_Handler(object? obj)
    {
        _powerSupplyService.ClosePMU();
        await Task.Delay(500);
        String command = $"PMU1 = OFF, PMU2 = ON, Input = {InvValue}";
        await _powerSupplyService.StartPMU("DC", command);
    }

    protected void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }


}
