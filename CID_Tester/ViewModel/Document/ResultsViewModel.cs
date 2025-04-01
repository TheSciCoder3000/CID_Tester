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
    public string path { get; set; }

    public ICommand CloseCommand { get; }

    public ResultsViewModel(AppStore appStore, string pathToDoc)
    {
        _AppStore = appStore;
        Title = "Results Overview";
        CloseCommand = new RelayCommand(CloseCommandHanlder);
        path = pathToDoc;
    }

    private void CloseCommandHanlder(object? parameter) => _AppStore.DocumentStore.RemoveDocument(this);
}
