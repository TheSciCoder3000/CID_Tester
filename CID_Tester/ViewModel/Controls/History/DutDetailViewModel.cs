using CID_Tester.Model;

namespace CID_Tester.ViewModel.Controls.History;

public class DutDetailViewModel : BaseViewModel
{
    public string CycleNo { get; set; }
    public string DutLocation { get; set; }
    public IEnumerable<OutputDetailViewModel> OutputList { get; set; }

    public DutDetailViewModel(int cycleNo, int dutLocation, ICollection<TEST_OUTPUT> testOutput)
    {
        CycleNo = cycleNo.ToString();
        DutLocation = $"DUT {dutLocation}";
        OutputList = testOutput
            .Where(output => output.DutLocation == dutLocation)
            .Select((output) => new OutputDetailViewModel(cycleNo, dutLocation, output));
    }
}
