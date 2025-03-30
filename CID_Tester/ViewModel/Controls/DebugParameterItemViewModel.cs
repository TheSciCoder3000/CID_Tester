using CID_Tester.Model;
using CID_Tester.Service.Serial;
using CID_Tester.ViewModel.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CID_Tester.ViewModel.Controls;

public class DebugParameterItemViewModel : BaseViewModel
{
    private SwitchMatrix _switchMatrixService;
    private bool _isParameterActive = false;
    private TEST_PARAMETER _param;
    public bool IsParameterActive
    {
        get => _isParameterActive;
        set
        {
            _isParameterActive = value;
            onPropertyChanged(nameof(IsParameterActive));
        }
    }

    public string ParameterName { get => _param.Name;  }

    public ICommand SetParameterCommand { get; }

    public DebugParameterItemViewModel(TEST_PARAMETER param, SwitchMatrix switchMatrix)
    {
        _param = param;
        _switchMatrixService = switchMatrix;
        SetParameterCommand = new RelayCommand(SetParameterCommand_Handler);
    }


    private async void SetParameterCommand_Handler(object? obj)
    {
        _switchMatrixService.Open();
        _switchMatrixService.Reset();
        await Task.Delay(1000);
        if (_param != null)
        {
            await _switchMatrixService.Start(_param.ParseToParameterDictionary());
        }
        _switchMatrixService.Close();
    }
}
