using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CID_Tester.Exceptions
{
    public class ExcelFormatException : Exception
    {
        public ExcelFormatException(string msg) : base(msg) { }
    }
}
