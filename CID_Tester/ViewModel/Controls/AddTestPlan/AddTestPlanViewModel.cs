﻿using CID_Tester.ViewModel.Command;
using CID_Tester.Model;
using System.Windows.Input;
using CID_Tester.Store;

namespace CID_Tester.ViewModel.Controls.AddTestPlan;

public class AddTestPlanViewModel : BaseViewModel
{
    private readonly AppStore _appStore;
    private string _selectedDevice;
    public string SelectedDevice
    {
        get => _selectedDevice;
        set
        {
            _selectedDevice = value;
            onPropertyChanged(nameof(SelectedDevice));
        }
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
    private string _description = null!;
    public string Description
    {
        get => _description;
        set
        {
            _description = value;
            onPropertyChanged(nameof(Description));
        }
    }

    private int _cycleNo = 1;
    public int CycleNo
    {
        get => _cycleNo;
        set
        {
            _cycleNo = value;
            onPropertyChanged(nameof(CycleNo));
        }
    }

    private IEnumerable<string> _devices;
    public IEnumerable<string> Devices
    {
        get => _devices;
        set
        {
            _devices = value;
            onPropertyChanged(nameof(Devices));
        }
    }

    public Action CloseCommand;


    public ICommand CreateCommand { get; }
    public ICommand CancelCommand { get; }

    public AddTestPlanViewModel(AppStore appStore, Action CloseModal)
    {
        _appStore = appStore;
        CloseCommand = CloseModal;
        Devices = _appStore.DUTs.Select(dut => dut.DutName);
        CreateCommand = new RelayCommand(CreateTestPlanHandler);
        CancelCommand = new RelayCommand((parameter) => CloseCommand());
    }

    private async void CreateTestPlanHandler(object? obj)
    {
        TEST_PLAN testPlan = new TEST_PLAN()
        {
            Name = Name,
            Description = Description,
            DUT = _appStore.DUTs.FirstOrDefault(dut => dut.DutName == SelectedDevice),
        };
        await _appStore.TestPlanStore.CreateTestPlan(testPlan);
        CloseCommand();
    }
}
