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
        private float _frequency = 0;
        private float _amplitude = 0;
        private float _timebase = 0;
        private Imports.WaveType _signalType = Imports.WaveType.SINE;
        private bool _useFG1 = false;
        private bool _useFG2 = false;

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
            _timebase = float.Parse(configuration["timebase"]);
            _signalType = (Imports.WaveType)Enum.Parse(typeof(Imports.WaveType), configuration["signalType"]);
            _useFG1 = configuration["FG1"] == "ON";
            _useFG1 = configuration["FG2"] == "ON";
        }
        public void StartFunctionGen()
        {
            int offset = 0;
            uint pkToPk = (uint)(_amplitude * 2 * 1000) * 1000;
            uint sweeps = 0;
            Double startFreq = _frequency;
            Double stopFreq = startFreq;
            Double increment = 0;
            Double dwellTime = 0;
            Imports.SweepType sweeptype = Imports.SweepType.UP;
            Imports.WaveType wavetype = Imports.WaveType.SINE;

            int status = Imports.SetSigGenBuiltIn(1,
                                              offset,
                                              pkToPk,
                                              wavetype,
                                              (float)startFreq,
                                              (float)stopFreq,
                                              (float)increment,
                                              (float)dwellTime,
                                              sweeptype,
                                              sweeps);

            Thread.Sleep(1000);
            if (status == 0) Debug.WriteLine("Function gen error");
            if (_useFG1) OpenInvFG();
            else if (_useFG2) OpenNinvFG();
        }

        public void StopFunctionGen()
        {
            Imports.SetSigGenBuiltIn(1, 0, 0, Imports.WaveType.SINE, 0, 0, 0, 0, Imports.SweepType.UP, 0);
            if (_useFG1) CloseInvFG();
            else if (_useFG2) CloseNinvFG();
        }

        public string CaptureGraph(string filename)
        {
            PinnedArray<short>[] pinned = new PinnedArray<short>[4];
            int sampleCount = 1024*7;
            short timeUnit = 0;
            int timeIndisposed;
            short status = 0;
            ScottPlot.TickGenerators.NumericManual tickGen = new();

            int[] times = new int[sampleCount];
            PinnedArray<int> pinnedTimes = new PinnedArray<int>(times);

            // Channel buffers
            for (int i = 0; i < 2; i++)
            {
                short[] buffer = new short[sampleCount];
                pinned[i] = new PinnedArray<short>(buffer);
            }

            int timeInterval = 0;
            int maxSamples;

            do
            {
                status = Imports.GetTimebase(1, (short)_timebase, sampleCount, out timeInterval, out timeUnit, 1, out maxSamples);

                if (status != 1)
                {
                    Debug.WriteLine("Selected timebase {0} could not be used\n", _timebase);
                    _timebase++;
                }

            }
            while (status == 0);

            Debug.WriteLine("Timebase: {0}\toversample:{1}\n", _timebase, 1);

            /* Start the device collecting, then wait for completion*/

            Imports.RunBlock(1, sampleCount, (short)_timebase, 1, out timeIndisposed);

            short ready = 0;

            while (ready == 0)
            {
                ready = Imports.Isready(1);
                Thread.Sleep(1);
            }

            if (ready > 0)
            {
                short overflow;

                Imports.GetTimesAndValues(1, pinnedTimes, pinned[0], pinned[1], null, null, out overflow, timeUnit, sampleCount);


                for (int i = 0; i < ValuesOut.Length; i++)
                {

                    long y1 = adc_to_mv(pinned[0].Target[i], 20000);
                    long y2 = adc_to_mv(pinned[1].Target[i], 20000);
                    int x = pinnedTimes.Target[i];

                    ValuesOut[i] = (Convert.ToDouble(y1) / 1000);
                    ValuesIn[i] = (Convert.ToDouble(y2) / 1000);
                    tickGen.AddMajor(i, "");
                }
            }
            else
            {
                Console.WriteLine("Data collection aborted");
            }


            OscPlot.Plot.Axes.Bottom.TickGenerator = tickGen;
            OscPlot.Plot.Axes.AutoScale();
            OscPlot.Refresh();

            Imports.Stop(1);

            string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string resultsPath = Path.Combine(localAppData, "Results", filename);
            using (MemoryStream ms = new MemoryStream(OscPlot.Plot.GetImageBytes(400, 400)))
            {
                Bitmap bmp = new Bitmap(ms);
                bmp.Save(resultsPath, ImageFormat.Jpeg);
            }
            return resultsPath;
        }

        private long adc_to_mv(int raw, int ch)
        {
            return raw * ch / Imports.PS2000_MAX_VALUE;
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

