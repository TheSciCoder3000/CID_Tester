
using System.Windows.Input;
using CID_Tester.Model;
using CID_Tester.Store;
using CID_Tester.ViewModel.Command;
using CID_Tester.ViewModel.Controls.History;
using CID_Tester.ViewModel.Interfaces;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Fields;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.Rendering;
using PdfSharp.Pdf;
using CID_Tester.ViewModel.Document;

namespace CID_Tester.ViewModel.Document;

public class BatchDetailsViewModel : BaseViewModel, IDocument
{
    private readonly AppStore _AppStore;
    private RenderPDF renderHandler;
    public string Title { get; set; }
    public ICommand CloseCommand { get; }
    public ICommand ExportCommand { get; }

    private TEST_BATCH _batchDetails = null!;
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
            return Enumerable.Range(1, NumberOfDuts)
                .Select(dutIndx => new DutDetailViewModel(SelectedCycle, dutIndx, BatchDetails.TEST_OUTPUTS
                .Where(output => output.DutLocation == dutIndx && output.TEST_PARAMETER.Type == "DC").ToList()));
        }
    }

    public IEnumerable<DutGraphViewModel> AcDutList
    {
        get
        {
            // TODO: Replace with dynamic number of DUTs tested
            int NumberOfDuts = 4;
            return Enumerable.Range(1, NumberOfDuts)
                .Select(dutIndx => new DutGraphViewModel(SelectedCycle, dutIndx, BatchDetails.TEST_OUTPUTS
                .Where(output => output.DutLocation == dutIndx && output.TEST_PARAMETER.Type == "AC").ToList()));
        }
    }

    public BatchDetailsViewModel(AppStore appStore, TEST_BATCH batchDetails, string title = "Batch Details")
    {
        _AppStore = appStore;
        BatchDetails = batchDetails;
        Title = title;
        CloseCommand = new RelayCommand(CloseCommandHanlder);
        ExportCommand = new RelayCommand(ExportCommandHandler);
        renderHandler = new RenderPDF(_batchDetails, _AppStore);
    }

    private void ExportCommandHandler(object? obj)
    {
        
        renderHandler.Render();
    }

    private void CloseCommandHanlder(object? obj)
    {
        _AppStore.DocumentStore.RemoveDocument(this);
    }
}
