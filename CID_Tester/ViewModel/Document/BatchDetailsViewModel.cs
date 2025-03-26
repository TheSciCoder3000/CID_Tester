using CID_Tester.Model;
using CID_Tester.Store;
using CID_Tester.ViewModel.Command;
using CID_Tester.ViewModel.Controls.History;
using System.Windows.Input;

namespace CID_Tester.ViewModel.Document;

public class BatchDetailsViewModel : BaseViewModel
{
    private readonly AppStore _AppStore;

    public string Title { get; set; }
    public ICommand CloseCommand { get; }

    private TEST_BATCH _batchDetails;
    public TEST_BATCH BatchDetails
    {
        get => _batchDetails;
        set
        {
            _batchDetails = value;
            onPropertyChanged(nameof(BatchDetails));
        }
    }

    public IEnumerable<int> CycleNoList
    {
        get => Enumerable.Range(1, BatchDetails.CycleNo);

    }
    
    private int _selectedCycle = 1;
    public int SelectedCycle
    {
        get => _selectedCycle;
        set
        {
            _selectedCycle = value;
            onPropertyChanged(nameof(SelectedCycle));
        }
    }

    public IEnumerable<DutDetailViewModel> DcDutList
    {
        get
        {
            // TODO: Replace with dynamic number of DUTs tested
            int NumberOfDuts = 4;
            return Enumerable.Range(1,NumberOfDuts)
                .Select(dutIndx => new DutDetailViewModel(SelectedCycle, dutIndx, BatchDetails.TEST_OUTPUTS
                .Where(output => output.DutLocation == dutIndx && output.TEST_PARAMETER.Type == "DC").ToList()));
        }
    }

    public BatchDetailsViewModel(AppStore appStore, TEST_BATCH batchDetails)
    {
        _AppStore = appStore;
        BatchDetails = batchDetails;
        Title = "Batch Details";
        CloseCommand = new RelayCommand(CloseCommandHanlder);
    }

    private void CloseCommandHanlder(object? obj)
    {
        _AppStore.DocumentStore.RemoveDocument(this);
    }
}
