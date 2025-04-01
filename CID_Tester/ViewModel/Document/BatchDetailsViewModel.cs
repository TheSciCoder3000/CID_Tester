using CID_Tester.Model;
using CID_Tester.Store;
using CID_Tester.ViewModel.Command;
using CID_Tester.ViewModel.Controls.History;
using CID_Tester.ViewModel.Interfaces;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows;
using System.Windows.Input;

using System.IO;
namespace CID_Tester.ViewModel.Document;

public class BatchDetailsViewModel : BaseViewModel, IDocument
{
    private readonly AppStore _AppStore;

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
    }

    private void ExportCommandHandler(object? obj)
    {
        using (PdfDocument document = new PdfDocument())
        {
            string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string resultsPath = Path.Combine(localAppData, "Results/PDF", "temp.pdf");
            //Add a page to the document.
            PdfPage page = document.Pages.Add();
            //Create PDF graphics for a page.
            PdfGraphics graphics = page.Graphics;
            //Set the standard font.
            PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 20);
            //Draw the text.
            graphics.DrawString("Hello World!!!", font, PdfBrushes.Black, new PointF(0, 0));
            //Save the document.
            document.Save(resultsPath);
        }
    }

    private void CloseCommandHanlder(object? obj)
    {
        _AppStore.DocumentStore.RemoveDocument(this);
    }
}
