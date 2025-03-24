using CID_Tester.ViewModel.DebugSDK;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CID_Tester.Service.Serial
{
    public class FunctionSwitchService : BaseSerial
    {
        private float _frequency = 0;
        private float _amplitude = 0;
        private float _timebase = 0;
        private Imports.WaveType _signalType = Imports.WaveType.SINE;
        private bool _useFG1 = false;
        private bool _useFG2 = false;

        public void ParseInputConfiguration(string configurationString)
        {
            string[] configurationList = configurationString.Split(", ");
            if (configurationList.Length != 6) throw new Exception("Invalid configuration string");

            Dictionary<string, string> configuration = new Dictionary<string, string>(configurationList.Select(config =>
            {
                string[] keyValue = config.Split('=');
                return new KeyValuePair<string, string>(keyValue[0], keyValue[1]);
            }));

            _frequency = float.Parse(configuration["frequency"]);
            _amplitude = float.Parse(configuration["amplitude"]);
            _timebase = float.Parse(configuration["timebase"]);
            _signalType = (Imports.WaveType)Enum.Parse(typeof(Imports.WaveType), configuration["signalType"]);
            _useFG1 = configuration["FG1"] == "ON";
            _useFG1 = configuration["FG2"] == "ON";
        }
        public void StartFunctionGen()
        {

            int offset = 0;
            uint pkToPk = (uint)(_amplitude * 2 * 1000) * 1000;
            uint sweeps = 0;
            Double startFreq = _frequency;
            Double stopFreq = startFreq;
            Double increment = 0;
            Double dwellTime = 0;
            Imports.SweepType sweeptype = Imports.SweepType.UP;
            Imports.WaveType wavetype = Imports.WaveType.SINE;

            int status = Imports.SetSigGenBuiltIn(1,
                                              offset,
                                              pkToPk,
                                              wavetype,
                                              (float)startFreq,
                                              (float)stopFreq,
                                              (float)increment,
                                              (float)dwellTime,
                                              sweeptype,
                                              sweeps);


            if (status == 0) Debug.WriteLine("Function gen error");
            if (_useFG1) OpenInvFG();
            else if (_useFG2) OpenNinvFG();
        }

        public void StopFunctionGen()
        {
            Imports.SetSigGenBuiltIn(0, 0, 0, Imports.WaveType.SINE, 0, 0, 0, 0, Imports.SweepType.UP, 0);
            if (_useFG1) CloseInvFG();
            else if (_useFG2) CloseNinvFG();
        }

        public FunctionSwitchService() : base("FGRELAY", 9600) { }

        #region Function Gen Relay Functions
        private void OpenInvFG()
        {
            SendCommand("ON1");
        }

        private void OpenNinvFG()
        {
            SendCommand("ON2");
        }

        private void CloseInvFG()
        {
            SendCommand("OFF1");
        }

        private void CloseNinvFG()
        {
            SendCommand("OFF2");
        }

        public void CloseAll()
        {
            SendCommand("ALLOFF");
        }
        #endregion
    }
}

