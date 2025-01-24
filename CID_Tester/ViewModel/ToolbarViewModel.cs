using CID_Tester.Model;
using CID_Tester.View.Windows;
using CID_Tester.ViewModel.Command;
using CID_Tester.ViewModel.Controls.AddTestPlan;
using CID_Tester.ViewModel.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CID_Tester.ViewModel
{
    public class ToolbarViewModel : BaseViewModel
    {
        private readonly Store _AppStore;

        public ToolbarViewModel(Store appStore)
        {
            _AppStore = appStore;
            _AppStore.OnTestPlanUpdated += updateTestPlanList;
            AddTestPlanCommand = new RelayCommand(AddTestPlanHandler);
            ImportTestPlanCommand = new RelayCommand(ImportTestPlanHandler);
        }

        #region Command Handlers
        private void AddTestPlanHandler(object? obj)
        {
            OpenTestPlanView testPlanDialog = new OpenTestPlanView();
            Action CloseDialog = () => testPlanDialog.Close();
            OpenTestPlanViewModel vm = new OpenTestPlanViewModel(_AppStore, CloseDialog, new AddTestPlanViewModel(_AppStore, CloseDialog));
            testPlanDialog.DataContext = vm;
            testPlanDialog.ShowDialog();
        }
        private void ImportTestPlanHandler(object? obj)
        {
            OpenTestPlanView testPlanDialog = new OpenTestPlanView();
            Action CloseDialog = () => testPlanDialog.Close();
            OpenTestPlanViewModel vm = new OpenTestPlanViewModel(_AppStore, CloseDialog, new AddTestPlanImporterViewModel(_AppStore, CloseDialog));
            testPlanDialog.DataContext = vm;
            testPlanDialog.ShowDialog();
        }


        #endregion

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
    
        public ICommand AddTestPlanCommand { get; }
        public ICommand ImportTestPlanCommand { get; }
    }
}
