using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CID_Tester.Model
{
    internal class TEST_PARAMETER
    {
        public int PARAM_CODE { get; set; }
        public int TEST_CODE { get; set; }
        public string DESCRIPTION {  get; set; }
        public string METRIC { get; set; }
        public decimal VALUE { get; set; }
        public string UNIT { get; set; }

    }
}
