
using CID_Tester.Model;

namespace CID_Tester.ViewModel.Controls.History;

public class OutputDetailViewModel
{
    private TEST_OUTPUT _testOutput;

    private float ratioTolerance = 1;
    public string CycleNo { get; set; }
    public string DutLocation { get; set; }
    public string Name { get; set; }
    public string Target { get; set; }
    public string Metric { get; set; }
    public string Measured { get; set; }
    public string Pass { get; set; }

    public OutputDetailViewModel(int cycleNo, int dutLocation, TEST_OUTPUT testOutput)
    {
        _testOutput = testOutput;
        CycleNo = cycleNo.ToString();
        DutLocation = $"DUT {DutLocation}";
        Name = testOutput.TEST_PARAMETER.Name;
        Target = testOutput.TEST_PARAMETER.Target.ToString();
        Metric = testOutput.TEST_PARAMETER.Metric;
        Measured = testOutput.Measured;
        Pass = CheckAccuracy(Double.Parse(Measured)) ? "PASS" : "FAIL";
    }

    private bool CheckAccuracy(double value)
    {
        double upperLimit = (double)(_testOutput.TEST_PARAMETER.Target + (decimal)ratioTolerance);
        double lowerLimit = (double)(_testOutput.TEST_PARAMETER.Target - (decimal)ratioTolerance);
        return value >= lowerLimit && value <= upperLimit;
    }
}
