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
namespace CID_Tester.ViewModel.Document;

public class DebugViewModel : BaseViewModel, IDocument, INotifyPropertyChanged
{
    private readonly Store _AppStore;

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

    public double[] Values = new double[100];


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

    public ICommand CloseCommand { get; }
    public ICommand CaptureMeasurement { get; }
    public ICommand GetInfo { get; }
    public ICommand StartSigGen { get; }
    public ICommand StopSigGen { get; }
    public ICommand Loaded { get; }

    public DebugViewModel(Store appStore, PS2000 oscilloscope, PS2000SigGen sigGen)
    {
        OscDisplay = new WpfPlot();
        OscDisplay.Plot.Add.Signal(Values);
        ScottPlot.TickGenerators.NumericManual tickGen = new();

        ScottPlot.AxisPanels.Experimental.LeftAxisWithSubtitle customAxisY = new()
        {
            LabelText = "VOLTAGES",
            SubLabelText = "All units are in mV",
        };

        OscDisplay.Plot.Axes.Remove(OscDisplay.Plot.Axes.Left);
        OscDisplay.Plot.Axes.AddLeftAxis(customAxisY);

        for (int i = 0; i < Values.Length; i++)
        {
            tickGen.AddMajor(i, "");
        }

        OscDisplay.Plot.Axes.Bottom.TickLabelStyle.Rotation = -45;
        OscDisplay.Plot.Axes.Bottom.TickLabelStyle.Alignment = Alignment.MiddleRight;


        Oscilloscope = oscilloscope;
        SigGen = sigGen;
        _AppStore = appStore;
        Title = "Debug";

        // Oscilloscope
        CloseCommand = new RelayCommand(CloseCommandHanlder);
        CaptureMeasurement = new RelayCommand(CaptureMeasurementHandler);
        GetInfo = new RelayCommand(GetInfoHandler);

        // Signal Generator
        StartSigGen = new RelayCommand(StartSigGenHandler);
        StopSigGen = new RelayCommand(StopSigGenHandler);
        Loaded = new RelayCommand(LoadedHandler);
        signalType = 0;
        frequency = 1000;
        p2pVoltage = 2000;
        offsetVoltage = 0;

    }

    private void CloseCommandHanlder(object? parameter)
    {
        _AppStore.RemoveDocument(this);
    }

    private void CaptureMeasurementHandler(object? obj)
    {
        Oscilloscope.Run();
        Oscilloscope.SetTimebase(8);
        Oscilloscope.SetVoltages(8);
        Oscilloscope.CollectBlockImmediate();

        //Oscilloscope.Stream();

    }

    private void GetInfoHandler(object? obj)
    {
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
        SigGen.StartSignal();
    }

    private void StopSigGenHandler(object? obj)
    {
        SigGen.signalType = 8;
        SigGen.frequency = 0;
        SigGen.p2pVoltage = 0;
        SigGen.offsetVoltage = 0;
        SigGen.StartSignal();
    }


    protected void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

}
