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
using ScottPlot.WPF;
using ScottPlot.Plottables;
using ScottPlot;
using CID_Tester.Store;
namespace CID_Tester.ViewModel.Document;

public class DebugViewModel : BaseViewModel, IDocument, INotifyPropertyChanged
{
    private readonly AppStore _AppStore;

    public event PropertyChangedEventHandler PropertyChanged;

    private PS2000 Oscilloscope;
    private PS2000SigGen SigGen;
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

    private WpfPlot _OscDisplay;
    public WpfPlot OscDisplay
    {
        get { return _OscDisplay; }
        set
        {
            _OscDisplay = value;
            // Call OnPropertyChanged whenever the property is updated
            OnPropertyChanged();
        }
    }

    private DataStreamer _Streamer;
    public DataStreamer Streamer
    {
        get { return _Streamer; }
        set
        {
            _Streamer = value;
            // Call OnPropertyChanged whenever the property is updated
            OnPropertyChanged();
        }
    }

    public double[] ValuesOut = new double[100];

    public double[] ValuesIn = new double[100];

    private int _signalType;
    public int signalType
    {
        get { return _signalType; }
        set
        {
            SigGen.signalType = value;
            _signalType = value;
            OnPropertyChanged();
        }
    }

    private double _frequency;
    public double frequency
    {
        get { return _frequency; }
        set
        {
            SigGen.frequency = value;
            _frequency = value;
            OnPropertyChanged();
        }
    }

    private UInt32 _p2pVoltage;
    public UInt32 p2pVoltage
    {
        get { return _p2pVoltage; }
        set
        {
            SigGen.p2pVoltage = value;
            _p2pVoltage = value;
            OnPropertyChanged();
        }
    }

    private Int32 _offsetVoltage;
    public Int32 offsetVoltage
    {
        get { return _offsetVoltage; }
        set
        {
            SigGen.offsetVoltage = value;
            _offsetVoltage = value;
            OnPropertyChanged();
        }
    }

    private int _selectedTimebase;
    public int SelectedTimebase
    {
        get => _selectedTimebase;
        set
        {
            _selectedTimebase = value;
            OnPropertyChanged();
        }
    }
    private static readonly KeyValuePair<int, string>[] _timebaseList = {
        new KeyValuePair<int, string>(1, "10 ns"),
        new KeyValuePair<int, string>(2, "20 ns"),
        new KeyValuePair<int, string>(3, "40 ns"),
        new KeyValuePair<int, string>(4, "80 ns"),
        new KeyValuePair<int, string>(5, "160 ns"),
        new KeyValuePair<int, string>(6, "320 ns"),
        new KeyValuePair<int, string>(7, "640 ns"),
        new KeyValuePair<int, string>(8, "1.28 µs"),
        new KeyValuePair<int, string>(9, "2.56 µs"),
        new KeyValuePair<int, string>(10, "5.12 µs"),
        new KeyValuePair<int, string>(11, "10.24 µs"),
        new KeyValuePair<int, string>(12, "20.48 µs"),
        new KeyValuePair<int, string>(13, "40.96 µs"),
        new KeyValuePair<int, string>(14, "81.92 µs"),
        new KeyValuePair<int, string>(15, "163.84 µs"),
        new KeyValuePair<int, string>(16, "327.68 µs"),
        new KeyValuePair<int, string>(17, "655.36 µs"),
        new KeyValuePair<int, string>(18, "1.31 ms"),
        new KeyValuePair<int, string>(19, "2.62 ms"),
        new KeyValuePair<int, string>(20, "5.24 ms"),
        new KeyValuePair<int, string>(21, "10.49 ms"),
        new KeyValuePair<int, string>(22, "20.97 ms"),
    };
    public KeyValuePair<int, string>[] TimebaseList
    {
        get => _timebaseList;
    }

    public ICommand CloseCommand { get; }
    public ICommand CaptureMeasurement { get; }
    public ICommand GetInfo { get; }
    public ICommand StartSigGen { get; }
    public ICommand StopSigGen { get; }
    public ICommand Loaded { get; }

    public DebugViewModel(AppStore appStore, PS2000 oscilloscope, PS2000SigGen sigGen)
    {
        OscDisplay = new WpfPlot();
        OscDisplay.Plot.Add.Signal(ValuesOut);
        OscDisplay.Plot.Add.Signal(ValuesIn);
        ScottPlot.TickGenerators.NumericManual tickGen = new();

        ScottPlot.AxisPanels.Experimental.LeftAxisWithSubtitle customAxisY = new()
        {
            LabelText = "VOLTAGES",
            SubLabelText = "All units are in mV",
        };

        OscDisplay.Plot.Axes.Remove(OscDisplay.Plot.Axes.Left);
        OscDisplay.Plot.Axes.AddLeftAxis(customAxisY);

        for (int i = 0; i < ValuesOut.Length; i++)
        {
            tickGen.AddMajor(i, "");
        }
        OscDisplay.Plot.Axes.Bottom.TickGenerator = tickGen;
        

        Oscilloscope = oscilloscope;
        SigGen = sigGen;
        _AppStore = appStore;
        Title = "Debug";

        // Oscilloscope
        CloseCommand = new RelayCommand(CloseCommandHanlder);
        CaptureMeasurement = new RelayCommand((object? obj) => Task.Run(CaptureMeasurementHandler));
        GetInfo = new RelayCommand(GetInfoHandler);

        // Signal Generator
        StartSigGen = new RelayCommand(StartSigGenHandler);
        StopSigGen = new RelayCommand(StopSigGenHandler);
        Loaded = new RelayCommand(LoadedHandler);
        signalType = 0;
        SelectedTimebase = 9;
        frequency = 1000;
        p2pVoltage = 2000;
        offsetVoltage = 0;

    }

    private void CloseCommandHanlder(object? parameter)
    {
        _AppStore.DocumentStore.RemoveDocument(this);
    }

    private void CaptureMeasurementHandler()
    {
        Oscilloscope.Run();
        Oscilloscope.SetTimebase((short)SelectedTimebase);
        Oscilloscope.SetVoltages(8);
        Oscilloscope.CollectBlockImmediate();

        //Oscilloscope.Stream();

    }

    private void GetInfoHandler(object? obj)
    {
        Oscilloscope.Run();
        Oscilloscope.GetDeviceInfo();
        Oscilloscope.DisplaySettings();

    }

    private void LoadedHandler(object? obj)
    {
        //OscDisplay = new PlotModel();
        Debug.WriteLine("TEST");
    }


    private void StartSigGenHandler(object? obj)
    {
        SigGen.signalType = signalType;
        SigGen.frequency = frequency;
        SigGen.p2pVoltage = p2pVoltage;
        SigGen.offsetVoltage = offsetVoltage;
        SigGen.StartSignal();
        Task.Run(CaptureMeasurementHandler);
    }

    private void StopSigGenHandler(object? obj)
    {
        SigGen.signalType = 0;
        SigGen.frequency = 0;
        SigGen.p2pVoltage = 0;
        SigGen.offsetVoltage = 0;
        SigGen.StartSignal();
        Task.Run(CaptureMeasurementHandler);
    }


    protected void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

}
