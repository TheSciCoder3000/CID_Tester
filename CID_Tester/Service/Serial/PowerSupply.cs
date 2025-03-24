
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
        public PowerSupply() : base("SUPP", 115200) { }

        #region Power Supply Configuration Commands
        private void SetDPS1Voltage(int voltage)
        {
            SendCommand($"1SETV{voltage}");
        }
        private void SetDPS2Voltage(int voltage)
        {
            SendCommand($"2SETV{voltage}");
        }
        private void SetPMU1Voltage(float voltage)
        {
            SendCommand($"3SETV{voltage}");
        }
        private void SetPMU2Voltage(float voltage)
        {
            SendCommand($"6SETV{voltage}");
        }

        private void SetDPS1Current(int current)
        {
            SendCommand($"1SETA{current}");
        }
        private void SetDPS2Current(int current)
        {
            SendCommand($"2SETA{current}");
        }
        private void SetPMU1Current(float current)
        {
            SendCommand($"3SETA{current}");
        }
        private void SetPMU2Current(float current)
        {
            SendCommand($"6SETA{current}");
        }

        #endregion

        #region Power Supply Control Commands
        private void ToggleDPSPos(bool state)
        {
            SendCommand($"1TOGG");
        }

        private void ToggleDPSNeg(bool state)
        {
            SendCommand($"2TOGG");
        }

        private void OpenPMUInv()
        {
            SendCommand("3TOGG 1");
        }

        private  void OpenPMUNinv()
        {
            SendCommand("6TOGG 1");
        }

        #endregion

        public void StartPMU(string type, string config)
        {
            if (type != "DC") return;

            string[] configList = config.Split(", ");
            Dictionary<string, string> configuration = new Dictionary<string, string>(configList.Select(configItem =>
            {
                string[] keyValue = configItem.Split('=');
                return new KeyValuePair<string, string>(keyValue[0], keyValue[1]);
            }));

            bool usePMU1 = configuration["PMU1"] == "ON";
            bool usePMU2 = configuration["PMU2"] == "ON";
            float voltage = float.Parse(configuration["Input"]);

            if (usePMU1)
            {
                SetPMU1Voltage((int)voltage);
                SetPMU1Current(0.1f);
                OpenPMUInv();
            }

            if (usePMU2)
            {
                SetPMU2Voltage((int)voltage);
                SetPMU2Current(0.1f);
                OpenPMUNinv();
            }
        }


        public void ClosePMU()
        {
            SendCommand("3TOGG 0");
            SendCommand("6TOGG 0");
        }
    }
}
