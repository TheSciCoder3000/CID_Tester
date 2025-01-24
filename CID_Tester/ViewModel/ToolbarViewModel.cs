using CID_Tester.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CID_Tester.ViewModel
{
    public class ToolbarViewModel : BaseViewModel
    {
        private readonly Store _AppStore;

        public ToolbarViewModel(Store appStore)
        {
            _AppStore = appStore;
            _AppStore.OnTestPlanUpdated += updateTestPlanList;
        }

        private void updateTestPlanList(TEST_PLAN? testPlan)
        {
            onPropertyChanged(nameof(TestPlans));
            onPropertyChanged(nameof(SelectedTestPlan));
        }

        public IEnumerable<TEST_PLAN> TestPlans { get => _AppStore.TestUser.TEST_PLANS; }
        public TEST_PLAN? SelectedTestPlan
        {
            get => _AppStore.TestPlan;
            set
            {
                if (value != null) _AppStore.setTestPlan(value);
                onPropertyChanged(nameof(SelectedTestPlan));
            }
        }
    }
}
