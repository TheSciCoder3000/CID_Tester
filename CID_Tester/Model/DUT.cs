using CID_Tester.Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CID_Tester.Model
{
    public class DUT
    {
        public int DUT_CODE { get; }
        public string DESCRIPTION { get; }

        public string DUT_NAME { get; } 

        public DUT(int dutCode, string dutName, string description)
        {
            DUT_CODE = dutCode;
            DUT_NAME = dutName;
            DESCRIPTION = description;
        }

        public DutDTO ToDTO()
        {
            return new DutDTO()
            {
                DUT_CODE = DUT_CODE,
                DESCRIPTION = DESCRIPTION,
            };
        }

    }
}
