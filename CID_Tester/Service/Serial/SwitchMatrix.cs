using CID_Tester.Model;
using System.Diagnostics;
using System.IO.Ports;
using System.Windows;

namespace CID_Tester.Service.Serial
{
    public class SwitchMatrix : BaseSerial
    {
        public bool IsOpen { get => _serialPort?.IsOpen ?? false; }

        public SwitchMatrix() : base("MATRIX", 9600) { }

        public void Start(Dictionary<string, bool> parameterDictionary)
        {
            SendCommand("ALL0");
            foreach (var command in parameterDictionary)
            {
                if (command.Value == false) continue;
                SendCommand($"RLAY {command.Key}");
            }
        }

        public void ChangeDut(int dut_indx)
        {
            SendCommand($"DUTS DUT_{dut_indx}");
        }

        public void Reset()
        {
            SendCommand("ALL0");
        }
    }
}
