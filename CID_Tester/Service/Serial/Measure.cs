﻿using System.IO.Ports;

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
            _serialPort.BaudRate = 9600;
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
        public void SetModeVoltage()
        {
            _serialPort.WriteLine("CONF:SCAL:VOLT:DC");
        }

        #endregion

        public async Task<string> GetMeasurement(int timeout_delay = 10000)
        {
            if (!_serialPort.IsOpen) throw new Exception("Serial Port is not open");

            _taskCompletionSource = new TaskCompletionSource<string>();

            _serialPort.WriteLine("MEAS1?");

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
