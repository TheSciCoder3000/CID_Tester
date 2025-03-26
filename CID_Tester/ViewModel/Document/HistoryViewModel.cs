
using CID_Tester.Model;
using CID_Tester.Store;
using CID_Tester.ViewModel.Command;
using System.Windows.Input;

namespace CID_Tester.ViewModel.Document;

public class HistoryViewModel : BaseViewModel
{
    private readonly AppStore _AppStore;

    public string Title { get; set; }
    public ICommand CloseCommand { get; }

    public HistoryViewModel(AppStore appStore)
    {
        _AppStore = appStore;
        Title = "History";
        CloseCommand = new RelayCommand(CloseCommandHanlder);
    }

    public ICollection<TEST_BATCH> Batches
    {
        get => _AppStore.TestUser.TEST_BATCHES;
    }

    private void CloseCommandHanlder(object? obj)
    {
        _AppStore.DocumentStore.RemoveDocument(this);
    }
}
