
using CID_Tester.Model;

namespace CID_Tester.ViewModel.Controls.History;

public class DutGraphViewModel : BaseViewModel
{
    public string CycleNo { get; set; }
    public string DutLocation { get; set; }
    public IEnumerable<OutputGraphViewModel> OutputList { get; set; }

    public DutGraphViewModel(int cycleNo, int dutLocation, ICollection<TEST_OUTPUT> testOutput)
    {
        CycleNo = cycleNo.ToString();
        DutLocation = $"DUT {dutLocation}";
        OutputList = testOutput
            .Where(output => output.DutLocation == dutLocation)
            .Select((output) => new OutputGraphViewModel(cycleNo, dutLocation, output));
    }
}
