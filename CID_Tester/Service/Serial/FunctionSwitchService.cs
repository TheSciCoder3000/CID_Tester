using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CID_Tester.Service.Serial
{
    public class FunctionSwitchService : BaseSerial
    {
        public FunctionSwitchService(string portName) : base(portName) { }

        public void OpenInvFG()
        {
            SendCommand("ON1");
        }

        public void OpenNinvFG()
        {
            SendCommand("ON2");
        }

        public void CloseInvFG()
        {
            SendCommand("OFF1");
        }

        public void CloseNinvFG()
        {
            SendCommand("OFF2");
        }

        public void CloseAll()
        {
            SendCommand("ALLOFF");
        }
    }
}
}
