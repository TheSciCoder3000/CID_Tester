using ScottPlot.Triangulation;
using System.Diagnostics;
using System.Globalization;
using System.IO.Ports;

namespace CID_Tester.Service.Serial
{
    public class Measure : BaseSerial
    {
        private TaskCompletionSource<string> _taskCompletionSource;

        public Measure() : base("OWON,XDM1241,22320320,V3.8.0,3", 115200, "*IDN?")
        {
            if (_serialPort != null) _serialPort.DataReceived += OnDataReceived;
        }

        private void OnDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string data = sp.ReadLine().Trim();
            _taskCompletionSource?.TrySetResult(data);
        }

        #region Set Mode Functions
        public async Task SetModeVoltage()
        {
            try
            {
                string mode = await GetFunc();
                if (mode != "\"VOLT\"")
                {
                    SendCommand("CONF:VOLT:DC 50");
                    Thread.Sleep(150);
                    SendCommand("RATE F");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public async Task<string> GetFunc(int timeout_delay = 10000)
        {
            if (!_serialPort?.IsOpen ?? true) throw new Exception("Serial Port is not open");

            _taskCompletionSource = new TaskCompletionSource<string>();

            SendCommand("FUNC?");

            Task timeoutTask = Task.Delay(timeout_delay);
            Task<string> responseTask = _taskCompletionSource.Task;

            Task completedTask = await Task.WhenAny(responseTask, timeoutTask);

            if (completedTask == responseTask) return responseTask.Result;
            throw new TimeoutException("TImed out in waiting for serial response");
        }

        #endregion

        public async Task<double> GetMeasurement(int timeout_delay = 10000)
        {
            if (!_serialPort?.IsOpen ?? true) throw new Exception("Serial Port is not open");

            _taskCompletionSource = new TaskCompletionSource<string>();

            await Task.Delay(2500);
            SendCommand("MEAS?");

            Task timeoutTask = Task.Delay(timeout_delay);
            Task<string> responseTask = _taskCompletionSource.Task;

            Task completedTask = await Task.WhenAny(responseTask, timeoutTask);

            if (completedTask == responseTask) return ConvertRawReadingToFloat(responseTask.Result);
            throw new TimeoutException("TImed out in waiting for serial response");
        }

        private double ConvertRawReadingToFloat(string rawValue)
        {
            return double.Parse(rawValue, CultureInfo.InvariantCulture);
        }
    }
}
