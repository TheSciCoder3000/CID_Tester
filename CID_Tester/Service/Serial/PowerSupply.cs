
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
            SendCommand($"1SETV{voltage}");
        }
        public void SetDPS2Voltage(int voltage)
        {
            SendCommand($"2SETV{voltage}");
        }
        public void SetPMU1Voltage(int voltage)
        {
            SendCommand($"3SETV{voltage}");
        }
        public void SetPMU2Voltage(int voltage)
        {
            SendCommand($"2SETV{voltage}");
        }

        public void SetDPS1Current(int current)
        {
            SendCommand($"1SETA{current}");
        }
        public void SetDPS2Current(int current)
        {
            SendCommand($"2SETA{current}");
        }
        public void SetPMU1Current(int current)
        {
            SendCommand($"3SETA{current}");
        }
        public void SetPMU2Current(int current)
        {
            SendCommand($"2SETA{current}");
        }

        public void ToggleDPSPos(bool state)
        {
            SendCommand($"1TOGG {(state ? 1 : 0)}");
        }

        public void ToggleDPSNeg(bool state)
        {
            SendCommand($"2TOGG {(state ? 1 : 0)}");
        }

        public void TogglePMUInput(OpAmpInput inputPin, bool state)
        {
            switch (inputPin)
            {
                case OpAmpInput.Inverting:
                    SendCommand($"3TOGG {(state ? 1 : 0)}");
                    break;
                case OpAmpInput.NonInverting:
                    SendCommand($"4TOGG {(state ? 1 : 0)}");
                    break;
                case OpAmpInput.Both:
                    SendCommand($"3TOGG {(state ? 1 : 0)}");
                    SendCommand($"4TOGG {(state ? 1 : 0)}");
                    break;
                default:
                    SendCommand("Incorrect supply input Pin");
                    break;
            }
        }
    }
}
