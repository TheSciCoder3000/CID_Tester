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
        protected SerialPort _serialPort;
        public BaseSerial(string portName, int baudrate = 9600)
        {
            _serialPort = new SerialPort(portName, baudrate);
        }

        public bool Open()
        {
            try
            {
                _serialPort.Open();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public void Close() => _serialPort.Close();

        protected void SendCommand(string command)
        {
            //if (_serialPort.IsOpen)
            //{
            //    _serialPort.WriteLine(command);
            //}

            Debug.WriteLine(command);
        }
    }
}
