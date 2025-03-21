
using System.Diagnostics;
using System.IO.Ports;

namespace CID_Tester.Service.Serial
{

    public enum OpAmpInput
    {
        Inverting,
        NonInverting,
        Both
    }

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

        public void ToggleDPSPos(bool state)
        {
            Debug.WriteLine($"1TOGG {(state ? 1 : 0)}");
        }

        public void ToggleDPSNeg(bool state)
        {
            Debug.WriteLine($"2TOGG {(state ? 1 : 0)}");
        }

        public void TogglePMUInput(OpAmpInput inputPin, bool state)
        {
            switch (inputPin)
            {
                case OpAmpInput.Inverting:
                    Debug.WriteLine($"3TOGG {(state ? 1 : 0)}");
                    break;
                case OpAmpInput.NonInverting:
                    Debug.WriteLine($"4TOGG {(state ? 1 : 0)}");
                    break;
                case OpAmpInput.Both:
                    Debug.WriteLine($"3TOGG {(state ? 1 : 0)}");
                    Debug.WriteLine($"4TOGG {(state ? 1 : 0)}");
                    break;
                default:
                    Debug.WriteLine("Incorrect supply input Pin");
                    break;
            }
        }
    }
}
