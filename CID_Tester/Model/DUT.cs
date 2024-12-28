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
        public int DUT_CODE { get; set; }
        public string? DESCRIPTION { get; set; }

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
