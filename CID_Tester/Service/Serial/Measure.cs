using System.Diagnostics;
using System.IO.Ports;

namespace CID_Tester.Service.Serial
{
    public class Measure
    {
        private SerialPort _serialPort;
        private TaskCompletionSource<string> _taskCompletionSource;

        public Measure(string portName)
        {
            _serialPort = new SerialPort();
            _serialPort.PortName = portName;  // TODO: replace with actual port
            _serialPort.BaudRate = 115200;
            _serialPort.DataReceived += OnDataReceived;
            _serialPort.Open();
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
            string mode = await GetFunc();
            if (mode != "\"VOLT\"") _serialPort.WriteLine("CONF:VOLT:DC");
        }

        public async Task<string> GetFunc(int timeout_delay = 10000)
        {
            if (!_serialPort.IsOpen) throw new Exception("Serial Port is not open");

            _taskCompletionSource = new TaskCompletionSource<string>();

            _serialPort.WriteLine("FUNC?");

            Task timeoutTask = Task.Delay(timeout_delay);
            Task<string> responseTask = _taskCompletionSource.Task;

            Task completedTask = await Task.WhenAny(responseTask, timeoutTask);

            if (completedTask == responseTask) return responseTask.Result;
            throw new TimeoutException("TImed out in waiting for serial response");
        }

        #endregion

        public async Task<string> GetMeasurement(int timeout_delay = 10000)
        {
            if (!_serialPort.IsOpen) throw new Exception("Serial Port is not open");

            _taskCompletionSource = new TaskCompletionSource<string>();

            _serialPort.WriteLine("MEAS?");

            Task timeoutTask = Task.Delay(timeout_delay);
            Task<string> responseTask = _taskCompletionSource.Task;

            Task completedTask = await Task.WhenAny(responseTask, timeoutTask);

            if (completedTask == responseTask) return responseTask.Result;
            throw new TimeoutException("TImed out in waiting for serial response");
        }

        public void Close()
        {
            _serialPort.Close();
        }
    }
}
