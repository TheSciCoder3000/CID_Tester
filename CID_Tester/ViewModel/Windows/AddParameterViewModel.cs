﻿using CID_Tester.Model;
using CID_Tester.ViewModel.Command;
using System.Windows.Input;

namespace CID_Tester.ViewModel.Windows;

public class AddParameterViewModel : BaseViewModel
{
    private readonly Store _AppStore;
    private Action _closeWindow;
    public AddParameterViewModel(Store appStore, Action closeWindowCommand)
    {
        _AppStore = appStore;

        _closeWindow = closeWindowCommand;
        AddCommand = new RelayCommand(AddCommandHanlder);
        CancelCommand = new RelayCommand((object? sender) => closeWindowCommand());
        DoubleClickCommand = new RelayCommand(DoubleClickHandler);
    }

    private void DoubleClickHandler(object? obj)
    {
        throw new NotImplementedException();
    }

    private async void AddCommandHanlder(object? obj)
    {
        if (_AppStore.TestPlan != null)
            await _AppStore.CreateTestParameter(
                    new TEST_PARAMETER()
                    {
                        Name=Name, 
                        Description=Description, 
                        Metric= Metric, 
                        Target=Target, 
                        Parameters="",
                        TestPlan = _AppStore.TestPlan
                    }
                );
        _closeWindow();
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

    private string _description;
    public string Description
    {
        get => _description;
        set
        {
            _description = value;
            onPropertyChanged(nameof(Description));
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

    private int _target;
    public int Target
    {
        get => _target;
        set
        {
            _target = value;
            onPropertyChanged(nameof(Target));
        }
    }

    public ICommand AddCommand { get; }
    public ICommand CancelCommand { get; }
    public ICommand DoubleClickCommand { get; }
}
