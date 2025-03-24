using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CID_Tester.Service.Serial
{
    public class BaseSerial
    {
        private string _deviceId;
        protected SerialPort? _serialPort;
        protected void SendCommand(string command)
        {
            if (_serialPort?.IsOpen ?? false)
            {
                Debug.WriteLine($"Send: {command}");
                _serialPort.WriteLine(command);
            }
            else
            {
                Debug.WriteLine(command);
            }

        }

        public BaseSerial(string deviceId, int baudrate = 9600, string command = "WHOU")
        {
            _deviceId = deviceId;
            MessageBoxResult connectAgain = MessageBoxResult.Yes;
            while (connectAgain == MessageBoxResult.Yes)
            {
                try
                {
                    string[] portNames = SerialPort.GetPortNames();
                    string? portName = GetPortFromDeviceId(deviceId, baudrate, portNames, command);
                    _serialPort = new SerialPort(portName, baudrate);
                }
                catch (Exception ex)
                {
                    connectAgain = MessageBox.Show($"{ex.Message}, Would you like to try again?", "Cannot Find Device", MessageBoxButton.YesNo, MessageBoxImage.Question);
                }
            }
        }

        private string? GetPortFromDeviceId(string deviceId, int baudrate, string[] portNames, string command)
        {
            foreach (var portName in portNames)
            {
                SerialPort serialPortChecker = new SerialPort(portName, baudrate);
                try
                {
                    serialPortChecker.Open();
                    int retries = 5;
                    string response = string.Empty;
                    Debug.WriteLine($"Checking device {deviceId}");
                    while (response != deviceId && retries != 0)
                    {
                        serialPortChecker.WriteLine(command);
                        Thread.Sleep(200);
                        response = serialPortChecker.ReadExisting().Trim();
                        Debug.WriteLine($"Response {retries}: {response}");
                        retries--;
                    }
                    serialPortChecker.Close();
                    if (response == deviceId) return portName;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Unable to communicate with serial port {portName}");
                    Debug.WriteLine(ex.Message);
                }
            }

            throw new Exception($"Unable to find device with id '{deviceId}'");
        }

        public void Open()
        {
            if (_serialPort == null) throw new Exception($"Make sure the device '{_deviceId}' is connected");
            _serialPort.Open();
        }

        public void Close() => _serialPort?.Close();
    }
}
