using CID_Tester.Model;
using CID_Tester.Store;
using CID_Tester.ViewModel.Command;
using CID_Tester.ViewModel.Interfaces;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;

namespace CID_Tester.ViewModel.Anchorables;

public class TestParameterPropertiesViewModel : BaseViewModel, IDocument
{
    private readonly AppStore _AppStore;
    private readonly TEST_PARAMETER _testParameter;

    public ObservableCollection<ParametersItemViewModel> ParameterItems { get; set; } = new ObservableCollection<ParametersItemViewModel>();
    public TestParameterPropertiesViewModel(AppStore appStore, TEST_PARAMETER testParameter)
    {
        _AppStore = appStore;
        _AppStore.OnTestParameterUpdated += VerifyParameterProperties;

        _testParameter = testParameter;
        Title = "Test Parameter Properties";
        CloseCommand = new RelayCommand(CloseAnchorable);
        PropertyChanged += ChangeHandler;
        ParseParameters(testParameter.Parameters);

        ToggleDisplayParameters = new RelayCommand((obj) => ShowParameters = !ShowParameters);
    }

    private void ParseParameters(string parameters)
    {
        string[] parametersArray = parameters.Split(", ");
        foreach (var parameter in parametersArray)
        {
            ParameterItems.Add(new ParametersItemViewModel(parameter, UpdateParameters));
        }
    }

    private async void UpdateParameters(string RelayName, bool RelayState)
    {
        string[] parameterArray = _testParameter.Parameters.Split(", ");
        int parameterIndx = parameterArray.ToList().FindIndex(r => r.Contains(RelayName));
        parameterArray[parameterIndx] = $"{RelayName}={RelayState}";
        _testParameter.Parameters = string.Join(", ", parameterArray);
        await _AppStore.TestPlanStore.UpdateTestParameter(_testParameter);
    }

    private void VerifyParameterProperties(IEnumerable<TEST_PARAMETER> testParameters)
    {
        bool parameterExists = testParameters.Contains(_testParameter);
        if (!parameterExists) CloseCommand.Execute(null);
    }

    private async void ChangeHandler(object? sender, PropertyChangedEventArgs e)
    {
        await _AppStore.TestPlanStore.UpdateTestParameter(_testParameter);
    }

    private void CloseAnchorable(object? obj)
    {
        _AppStore.DocumentStore.RemoveAnchorable(this);
    }

    public string Name
    {
        get => _testParameter.Name;
        set
        {
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

    public decimal Target
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
        set
        {
            _testParameter.Description = value;
            onPropertyChanged(nameof(Description));
        }
    }

    private bool _showParameters = false;
    public bool ShowParameters
    {
        get => _showParameters;
        set
        {
            _showParameters = value;
            onPropertyChanged(nameof(ShowParameters));
        }
    }

    public string Title { get; }

    public ICommand CloseCommand { get; }
    public ICommand ToggleDisplayParameters { get; }
}
