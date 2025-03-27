using CID_Tester.ViewModel.DebugSDK;
using ScottPlot.WPF;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection.Metadata;
namespace CID_Tester.Service.Serial
{
    public class FunctionSwitchService : BaseSerial
    {
        private PS2000 Oscilloscope = new PS2000();
        private PS2000SigGen FuncGen = new PS2000SigGen();
        private float _frequency = 0;
        private float _amplitude = 0;
        private short _timebase = 7;
        private short _oversample = 1;
        private Imports.WaveType _signalType = Imports.WaveType.SINE;
        private bool _useFG1 = false;
        private bool _useFG2 = false;
        private uint _range = 8;
        private short _handle = 1;

        private WpfPlot OscPlot = new WpfPlot();
        public double[] ValuesOut = new double[100];
        public double[] ValuesIn = new double[100];

        public void ParseInputConfiguration(string configurationString)
        {
            string[] configurationList = configurationString.Split(", ");
            if (configurationList.Length != 6) throw new Exception("Invalid configuration string");

            Dictionary<string, string> configuration = new Dictionary<string, string>(configurationList.Select(config =>
            {
                string[] keyValue = config.Split('=');
                return new KeyValuePair<string, string>(keyValue[0], keyValue[1]);
            }));

            _frequency = float.Parse(configuration["frequency"]);
            _amplitude = float.Parse(configuration["amplitude"]);
            //_timebase = short.Parse(configuration["timebase"]);
            _range = (uint)Imports.Range.Range_5V;
            _timebase = 4;
            _signalType = (Imports.WaveType)Enum.Parse(typeof(Imports.WaveType), configuration["signalType"]);
            _useFG1 = configuration["FG1"] == "ON";
            _useFG2 = configuration["FG2"] == "ON";

            //if (configuration["range"] == "1V") _range = (uint)Imports.Range.Range_1V;
            //if (configuration["range"] == "2V") _range = (uint)Imports.Range.Range_2V;
            //if (configuration["range"] == "5V") _range = (uint)Imports.Range.Range_5V;
            //if (configuration["range"] == "10V") _range = (uint)Imports.Range.Range_10V;
            //if (configuration["range"] == "20V") _range = (uint)Imports.Range.Range_20V;
        }
        public void StartFunctionGen()
        {
            int offset = 0;
            uint pkToPk = 2000;

            FuncGen.StartSignal((int)_signalType, offset, _frequency, pkToPk);
            Thread.Sleep(1000);

            if (_useFG1) OpenInvFG();
            else if (_useFG2) OpenNinvFG();
        }

        public void StopFunctionGen()
        {
            FuncGen.StartSignal(0,0,0,0);
            CloseAll();
        }

        public string CaptureGraph(string filename)
        {
            WpfPlot chart = Oscilloscope.GetDataGenerate(_timebase, _range);

            string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string resultsPath = Path.Combine(localAppData, "Results", filename);
            using (MemoryStream ms = new MemoryStream(chart.Plot.GetImageBytes(1000, 400)))
            {
                Bitmap bmp = new Bitmap(ms);
                bmp.Save(resultsPath, ImageFormat.Jpeg);
            }
            return resultsPath;
        }

        public FunctionSwitchService() : base("FGRELAY", 9600)
        {
            OscPlot.Plot.Add.Signal(ValuesOut);
            OscPlot.Plot.Add.Signal(ValuesIn);
            ScottPlot.TickGenerators.NumericManual tickGen = new();

            ScottPlot.AxisPanels.Experimental.LeftAxisWithSubtitle customAxisY = new()
            {
                LabelText = "VOLTAGES",
                SubLabelText = "All units are in mV",
            };

            OscPlot.Plot.Axes.Remove(OscPlot.Plot.Axes.Left);
            OscPlot.Plot.Axes.AddLeftAxis(customAxisY);

            for (int i = 0; i < ValuesOut.Length; i++)
            {
                tickGen.AddMajor(i, "");
            }
            OscPlot.Plot.Axes.Bottom.TickGenerator = tickGen;
        }

        #region Function Gen Relay Functions
        private void OpenInvFG()
        {
            SendCommand("ON1");
        }

        private void OpenNinvFG()
        {
            SendCommand("ON2");
        }

        private void CloseInvFG()
        {
            SendCommand("OFF1");
        }

        private void CloseNinvFG()
        {
            SendCommand("OFF2");
        }

        public void CloseAll()
        {
            SendCommand("ALLOFF");
        }
        #endregion
    }
}

