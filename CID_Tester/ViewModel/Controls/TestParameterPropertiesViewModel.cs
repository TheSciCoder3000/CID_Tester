using CID_Tester.Model;
using CID_Tester.ViewModel.Command;
using CID_Tester.ViewModel.Interfaces;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace CID_Tester.ViewModel.Controls;

public class TestParameterPropertiesViewModel : BaseViewModel, IDocument
{
    private readonly Store _AppStore;
    private readonly TEST_PARAMETER _testParameter;
    public TestParameterPropertiesViewModel(Store appStore, TEST_PARAMETER testParameter)
    {
        _AppStore = appStore;
        _testParameter = testParameter;
        Title = "Test Parameter Properties";
        CloseCommand = new RelayCommand(CloseAnchorable);
        PropertyChanged += ChangeHandler;
    }

    private async void ChangeHandler(object? sender, PropertyChangedEventArgs e)
    {
        await _AppStore.UpdateTestParameter(_testParameter);
    }

    private void CloseAnchorable(object? obj)
    {
        _AppStore.RemoveAnchorable(this);
    }

    public string Name
    {
        get => _testParameter.Name;
        set        {
            _testParameter.Name = value;
            onPropertyChanged(nameof(Name));
        }
    }

    public string Metric
    {
        get => _testParameter.Metric;
        set
        {
            _testParameter.Metric = value;
            onPropertyChanged(nameof(Metric));
        }
    }

    public Decimal Target
    {
        get => _testParameter.Target;
        set
        {
            _testParameter.Target = value;
            onPropertyChanged(nameof(Target));
        }
    }

    public string Description
    {
        get => _testParameter.Description;
        set {
            _testParameter.Description = value;
            onPropertyChanged(nameof(Description));
        }
    }

    public string Title { get; }

    public ICommand CloseCommand { get; }
}
