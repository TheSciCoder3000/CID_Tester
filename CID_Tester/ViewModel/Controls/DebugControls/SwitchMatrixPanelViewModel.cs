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
using System.Windows;
using System.Windows.Input;

namespace CID_Tester.ViewModel.Controls.DebugControls;

public class SwitchMatrixPanelViewModel
{


    private PowerSupply _powerSupplyService;
    private SwitchMatrix _switchMatrixService;
    private Measure _measureService;
    private FunctionSwitchService _functionSwitchService;

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

    public ICommand ToggleFGRelay1Command { get; }
    public ICommand ToggleFGRelay2Command { get; }
    public ICommand ResetFGRelayCommand { get; }

    public ICommand ResetSwitchCommand { get; }

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
        ToggleFGRelay1Command = new RelayCommand(ToggleFGRelay1Command_Handler);
        ToggleFGRelay2Command = new RelayCommand(ToggleFGRelay2Command_Handler);
        ResetFGRelayCommand = new RelayCommand(ResetFGRelayCommand_Handler);
        ResetSwitchCommand = new RelayCommand(ResetSwitchCommand_Handler);
        int unconnected = Initialize();
        if (unconnected > 0) MessageBox.Show($"{unconnected} Devices unconnected");
    }

    private void ResetFGRelayCommand_Handler(object? obj)
    {
        _functionSwitchService.Open();
        _functionSwitchService.CloseAll();
        _functionSwitchService.Close();
    }

    private void ResetSwitchCommand_Handler(object? obj)
    {
        _switchMatrixService.Open();
        _switchMatrixService.Reset();
        _switchMatrixService.Close();
    }

    private int Initialize()
    {
        _powerSupplyService = new PowerSupply();
        _measureService = new Measure();
        _switchMatrixService = new SwitchMatrix();
        _functionSwitchService = new FunctionSwitchService();

        return _measureService.Unconnected() + _functionSwitchService.Unconnected() + _powerSupplyService.Unconnected() + _switchMatrixService.Unconnected();
    }

    private void ToggleDut1Command_Handler(object? obj)
    {
        _switchMatrixService.Open();
        _switchMatrixService.ChangeDut(1);
        _switchMatrixService.Close();
    }

    private void ToggleDut2Command_Handler(object? obj)
    {

        _switchMatrixService.Open();
        _switchMatrixService.ChangeDut(2);
        _switchMatrixService.Close();
    }

    private void ToggleDut3Command_Handler(object? obj)
    {

        _switchMatrixService.Open();
        _switchMatrixService.ChangeDut(3);
        _switchMatrixService.Close();
    }

    private void ToggleDut4Command_Handler(object? obj)
    {

        _switchMatrixService.Open();
        _switchMatrixService.ChangeDut(4);
        _switchMatrixService.Close();
    }

    private void ToggleFGRelay1Command_Handler(object? obj)
    {


        _functionSwitchService.Open();
        _functionSwitchService.OpenInvFG();
        _functionSwitchService.Close();
    }

    private void ToggleFGRelay2Command_Handler(object? obj)
    {

        _functionSwitchService.Open();
        _functionSwitchService.OpenNinvFG();
        _functionSwitchService.Close();
    }

    private void ToggleOffCommand_Handler(object? obj)
    {

        _switchMatrixService.Open();
        _switchMatrixService.DutOff();
        _switchMatrixService.Close();
    }

    private async void ToggleInvCommand_Handler(object? obj)
    {
        _powerSupplyService.Open();
        _powerSupplyService.ClosePMU();
        await Task.Delay(500);
        String command = $"PMU1=ON, PMU2=OFF, Input={InvValue}";
        await _powerSupplyService.StartPMU("DC", command);
        _powerSupplyService.Close();
    }

    private async void ToggleNinvCommand_Handler(object? obj)
    {

        _powerSupplyService.Open();
        _powerSupplyService.ClosePMU();
        await Task.Delay(500);
        String command = $"PMU1=OFF, PMU2=ON, Input={InvValue}";
        await _powerSupplyService.StartPMU("DC", command);
        _powerSupplyService.Close();
    }

    protected void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }


}
