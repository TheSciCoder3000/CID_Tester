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
            OnPropertyChanged();
        }
    }

    public ICommand CloseCommand { get; }
    public ICommand CaptureMeasurement { get; }

    public DebugViewModel(Store appStore, PS2000 oscilloscope)
    {
        Oscilloscope = oscilloscope;
        _AppStore = appStore;
        Title = "Debug";
        CloseCommand = new RelayCommand(CloseCommandHanlder);
        CaptureMeasurement = new RelayCommand(CaptureMeasurementHandler);
        ConsoleString = "Starting&#x0a;";
    }

    private void CloseCommandHanlder(object? parameter) => _AppStore.RemoveDocument(this);

    private void CaptureMeasurementHandler(object? obj)
    {

        foreach (var data in Oscilloscope.Captured.Target)
        {
            Debug.WriteLine(data);
        }

    }

    protected void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

}
