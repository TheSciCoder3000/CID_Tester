using CID_Tester.Model;
using ScottPlot.Colormaps;
using System.Diagnostics;
using System.IO.Ports;
using System.Windows;

namespace CID_Tester.Service.Serial
{
    public class SwitchMatrix : BaseSerial
    {
        public bool IsOpen { get => _serialPort?.IsOpen ?? false; }

        public SwitchMatrix() : base("SWITCH", 9600) { }

        public async Task Start(Dictionary<string, bool> parameterDictionary)
        {
            SendCommand("ALL0");
            foreach (var command in parameterDictionary)
            {
                if (command.Value == false) continue;
                SendCommand($"RLAY {command.Key} 1");
                await Task.Delay(100);
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
