using CID_Tester.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CID_Tester.ViewModel
{
    public class DashboardMetricViewModel : BaseViewModel
    {
        private string _dutName;
        public string DUTName
        {
            get { return _dutName; }
            set
            {
                _dutName = value;
                onPropertyChanged(nameof(DUTName));
            }
        }

        private string _dutDescription;
        public string DUTDescription
        {
            get { return _dutDescription; }
            set
            {
                _dutDescription = value;
                onPropertyChanged(nameof(DUTDescription));
            }
        }

        public DashboardMetricViewModel(TEST_PLAN testPlan)
        {
            DUTName = testPlan.DUT.Description;
            DUTDescription = testPlan.DUT.Description;
        }
    }
}
