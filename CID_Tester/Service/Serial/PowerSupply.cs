
using System.IO.Ports;

namespace CID_Tester.Service.Serial
{
    public class PowerSupply
    {
        private SerialPort _serialPort;

        public PowerSupply(string portName)
        {
            _serialPort = new SerialPort(portName);
            _serialPort.BaudRate = 9600;
            _serialPort.Open();
        }
    }
}
