using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CID_Tester.ViewModel
{
    public class DashboardViewModel : BaseViewModel
    {
        private string _fullname;
        public string Fullname 
        {
            get { return _fullname; }
            set 
            { 
                _fullname = value;
                onPropertyChanged(nameof(Fullname));
            }
        }

        public DashboardViewModel(string fullname)
        {
            _fullname = fullname;
        }
    }
}
