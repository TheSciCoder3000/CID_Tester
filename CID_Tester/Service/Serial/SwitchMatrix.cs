using System.IO.Ports;

namespace CID_Tester.Service.Serial
{
    public class SwitchMatrix
    {
        private SerialPort _serialPort;

        public SwitchMatrix(string portName)
        {
            _serialPort = new SerialPort();
            _serialPort.PortName = portName;
            _serialPort.BaudRate = 9600;
            _serialPort.Open();
        }


    }
}
