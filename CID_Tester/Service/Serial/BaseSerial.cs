using System;
using System.Collections.Generic;
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
            try
            {
                _serialPort.Open();
            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Close() => _serialPort.Close();
    }
}
