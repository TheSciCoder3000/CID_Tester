using CID_Tester.Model;
using System.Diagnostics;
using System.IO.Ports;
using System.Windows;

namespace CID_Tester.Service.Serial
{
    public class SwitchMatrix : BaseSerial
    {
        public bool IsOpen { get => _serialPort.IsOpen; }

        public SwitchMatrix(string portName) : base(portName, 9600) { }

        public void Start(Dictionary<string, bool> parameterDictionary)
        {
            foreach (var command in parameterDictionary)
            {
                if (command.Value == false) continue;
                Debug.WriteLine("ALL0");
                Debug.WriteLine($"RLAY {command.Key}");
            }
        }

        public void ChangeDut(int dut_indx)
        {
            Debug.WriteLine($"DUTS DUT_{dut_indx}");
        }

        public void Reset()
        {
            Debug.WriteLine("ALL0");
        }
    }
}
