
using CID_Tester.Model;
using CID_Tester.Store;
using CID_Tester.ViewModel.Command;
using System.Diagnostics;
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
        DoubleClickCommand = new RelayCommand(DoubleClickCommandHandler);
    }


    public ICollection<TEST_BATCH> Batches
    {
        get => _AppStore.TestUser.TEST_BATCHES;
    }
    
    private TEST_BATCH _selectedBatch;
    public TEST_BATCH SelectedBatch
    {
        get => _selectedBatch;
        set
        {
            _selectedBatch = value;
            onPropertyChanged(nameof(SelectedBatch));
        }
    }
    public ICommand DoubleClickCommand { get; }

    #region Command Handlers
    private void CloseCommandHanlder(object? obj)
    {
        _AppStore.DocumentStore.RemoveDocument(this);
    }
    private void DoubleClickCommandHandler(object? obj)
    {
        Debug.WriteLine($"Batch: {SelectedBatch.BatchCode}");
        _AppStore.DocumentStore.AddDocument<BatchDetailsViewModel>(new BatchDetailsViewModel(_AppStore, SelectedBatch));
    }
    #endregion
}
