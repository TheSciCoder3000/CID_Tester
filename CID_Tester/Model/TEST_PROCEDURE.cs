using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CID_Tester.Model
{
    public class TEST_PROCEDURE
    {
        public TEST_PROCEDURE(int testCode, DateTime date, int userCode, int DutCode, int cycleNo, int testTime, DUT dut)
        {
            TEST_CODE = testCode;
            DATE = date;
            USER_CODE = userCode;
            DUT_CODE = DutCode;
            CYCLE_NO = cycleNo;
            TEST_TIME = testTime;
            DUT = dut;
        }

        public int TEST_CODE { get; }
        public DateTime DATE {  get; }
        public int USER_CODE { get; }
        public int DUT_CODE { get; }
        public int CYCLE_NO { get; }
        public int TEST_TIME { get; }

        public DUT DUT { get; } = null!;

        

    }
}
