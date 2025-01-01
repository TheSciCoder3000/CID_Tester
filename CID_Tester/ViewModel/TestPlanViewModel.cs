using CID_Tester.Command;
using CID_Tester.Model;
using System.Windows.Input;

namespace CID_Tester.ViewModel
{
    public class TestPlanViewModel : BaseViewModel, IDocument
    {
        private readonly Store _AppStore;
        public string Title { get; }

        public ICommand CloseCommand { get; }

        public TestPlanViewModel(Store appStore)
        {
            _AppStore = appStore;
            Title = "Test Plan";
            CloseCommand = new RelayCommand(CloseCommandHanlder);
        }

        private void CloseCommandHanlder(object? parameter) => _AppStore.RemoveDocument(this);
    }
}
