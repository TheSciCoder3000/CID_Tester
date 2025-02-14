using CID_Tester.ViewModel.Command;
using CID_Tester.Model;
using CID_Tester.View;
using CID_Tester.ViewModel.Interfaces;
using System.Windows;
using System.Windows.Input;
using CID_Tester.ViewModel.Windows;
using System.Diagnostics;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CID_Tester.ViewModel.DebugSDK;
using OxyPlot;
using OxyPlot.Series;
namespace CID_Tester.ViewModel.Document;

public class DebugViewModel : BaseViewModel, IDocument, INotifyPropertyChanged
{
    private readonly Store _AppStore;

    public event PropertyChangedEventHandler PropertyChanged;

    private PS2000 Oscilloscope;
    public string Title { get; }

    private string _ConsoleString;
    public string ConsoleString
    {
        get { return _ConsoleString; }
        set
        {
            _ConsoleString = value;
            // Call OnPropertyChanged whenever the property is updated
            Debug.WriteLine(value);
            OnPropertyChanged();
        }
    }

    private PlotModel _OscDisplay;
    public PlotModel OscDisplay
    {
        get { return _OscDisplay; }
        set
        {
            _OscDisplay = value;
            // Call OnPropertyChanged whenever the property is updated
            OnPropertyChanged();
        }
    }

    public ICommand CloseCommand { get; }
    public ICommand CaptureMeasurement { get; }
    public ICommand GetInfo { get; }

    public DebugViewModel(Store appStore, PS2000 oscilloscope)
    {
        Oscilloscope = oscilloscope;
        _AppStore = appStore;
        Title = "Debug";
        CloseCommand = new RelayCommand(CloseCommandHanlder);
        CaptureMeasurement = new RelayCommand(CaptureMeasurementHandler);
        GetInfo = new RelayCommand(GetInfoHandler);
        ConsoleString = "Starting...\n";

        OscDisplay = new PlotModel();

    }

    private void CloseCommandHanlder(object? parameter) => _AppStore.RemoveDocument(this);

    private void CaptureMeasurementHandler(object? obj)
    {
        
        
        Oscilloscope.Run();
        Oscilloscope.CollectBlockImmediate();

    }

    private void GetInfoHandler(object? obj)
    {
        Oscilloscope.GetDeviceInfo();

    }

    protected void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

}
