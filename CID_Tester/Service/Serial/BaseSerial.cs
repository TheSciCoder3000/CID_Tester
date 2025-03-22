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
        protected SerialPort? _serialPort;
        public BaseSerial(string deviceId, int baudrate = 9600, string command = "WHOU")
        {
            try
            {
                string[] portNames = SerialPort.GetPortNames();
                string? portName = GetPortFromDeviceId(deviceId, baudrate, portNames, command);
                _serialPort = new SerialPort(portName, baudrate);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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

            throw new Exception($"Unable to find device with id {deviceId}");
        }

        public bool Open()
        {
            try
            {
                if (_serialPort == null) throw new Exception("Serial Port is not initialized");
                _serialPort.Open();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public void Close() => _serialPort?.Close();

        protected void SendCommand(string command)
        {
            if (_serialPort?.IsOpen ?? false)
            {
                Debug.WriteLine($"Send: {command}");
                _serialPort.WriteLine(command);
            } else
            {
                Debug.WriteLine(command);
            }

        }
    }
}
