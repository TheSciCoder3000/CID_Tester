using CID_Tester.Command;
using CID_Tester.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CID_Tester.ViewModel
{
    public class ResultsViewModel : BaseViewModel, IDocument
    {
        private readonly Store _AppStore;
        public string Title { get; }

        public ICommand CloseCommand {  get; }

        public ResultsViewModel(Store appStore)
        {
            _AppStore = appStore;
            Title = "Results Overview";
            CloseCommand = new RelayCommand(CloseCommandHanlder);
        }

        private void CloseCommandHanlder(object? parameter) => _AppStore.RemoveDocument(this);
    }
}
