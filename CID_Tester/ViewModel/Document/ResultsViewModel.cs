using CID_Tester.ViewModel.Command;
using CID_Tester.Model;
using CID_Tester.ViewModel.Interfaces;
using System.Windows.Input;
using CID_Tester.Store;

namespace CID_Tester.ViewModel.Document;

public class ResultsViewModel : BaseViewModel, IDocument
{
    private readonly AppStore _AppStore;
    public string Title { get; }

    public ICommand CloseCommand { get; }

    public ResultsViewModel(AppStore appStore)
    {
        _AppStore = appStore;
        Title = "Results Overview";
        CloseCommand = new RelayCommand(CloseCommandHanlder);
    }

    private void CloseCommandHanlder(object? parameter) => _AppStore.RemoveDocument(this);
}
