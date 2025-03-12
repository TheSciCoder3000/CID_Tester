
using System.Diagnostics;
using System.IO.Ports;

namespace CID_Tester.Service.Serial
{
    public class PowerSupply : BaseSerial
    {

        public PowerSupply(string portName) : base(portName, 115200) { }

        public void SetDPS1Voltage(int voltage)
        {
            Debug.WriteLine($"1SETV{voltage}");
        }
        public void SetDPS2Voltage(int voltage)
        {
            Debug.WriteLine($"2SETV{voltage}");
        }
        public void SetPMU1Voltage(int voltage)
        {
            Debug.WriteLine($"3SETV{voltage}");
        }
        public void SetPMU2Voltage(int voltage)
        {
            Debug.WriteLine($"2SETV{voltage}");
        }

        public void SetDPS1Current(int current)
        {
            Debug.WriteLine($"1SETA{current}");
        }
        public void SetDPS2Current(int current)
        {
            Debug.WriteLine($"2SETA{current}");
        }
        public void SetPMU1Current(int current)
        {
            Debug.WriteLine($"3SETA{current}");
        }
        public void SetPMU2Current(int current)
        {
            Debug.WriteLine($"2SETA{current}");
        }

        public void ToggleSupply()
        {

        }
    }
}
