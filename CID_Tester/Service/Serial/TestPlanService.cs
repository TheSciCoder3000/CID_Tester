using CID_Tester.Model;
using CID_Tester.Service.DbCreator;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace CID_Tester.Service.Serial;

public class TestPlanService
{
    private TEST_PLAN? _testPlan;
    public TEST_PLAN? TestPlan
    {
        get => _testPlan;
        set
        {
            _testPlan = value;
        }
    }

    private PowerSupply _powerSupplyService;
    private SwitchMatrix _switchMatrixService;
    private Measure _measureService;
    private FunctionSwitchService _functionSwitchService;
    private IDbCreator _dbCreator;

    public TestPlanService(IDbCreator dbCreator)
    {
        _dbCreator = dbCreator;
        initialize();
    }

    public void initialize()
    {
        _powerSupplyService = new PowerSupply();
        _measureService = new Measure();
        _functionSwitchService = new FunctionSwitchService();
        _switchMatrixService = new SwitchMatrix();
    }

    public async void Start(Action? OnTestComplete = null)
    {
        if (_testPlan == null)
        {
            Debug.WriteLine("Test plan is null");
            return;
        }

        try
        {
            for (int cycle = 0; cycle < _testPlan.CycleNo; cycle++)
            {
                Debug.WriteLine($"Test: {cycle + 1}");
                await RunTests();
            }
        } catch (Exception e)
        {
            MessageBox.Show(e.Message, "Test Plan Unable to Start", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        await Task.Delay(2000);
        OnTestComplete?.Invoke();
        CommandManager.InvalidateRequerySuggested();
    }

    private async Task RunTests()
    {
        // Open Services
        _powerSupplyService.Open();
        _switchMatrixService.Open();
        _measureService.Open();
        _functionSwitchService.Open();

        foreach (var parameter in _testPlan!.TEST_PARAMETERS)
        {
            // setup matrix configuration
            _switchMatrixService.Start(parameter.ParseToParameterDictionary());

            for (int dutNum = 1; dutNum <= 4; dutNum++)
            {
                // switch to dut num
                _switchMatrixService.ChangeDut(dutNum);

                // turn on + power supply
                // supply.ToggleDPSPos(true);

                // turn on - power supply
                // supply.ToggleDPSNeg(true);

                // turn on input
                if (parameter.Type == "DC")
                {
                    // set voltage
                    _powerSupplyService.StartPMU(parameter.Type, parameter.InputConfiguration);
                }
                else if (parameter.Type == "AC")
                {
                    // start function generator
                    _functionSwitchService.ParseInputConfiguration(parameter.InputConfiguration);
                    _functionSwitchService.StartFunctionGen();
                }

                // start measurement
                await _measureService.SetModeVoltage();
                string rawValue = await _measureService.GetMeasurement();
                Debug.WriteLine($"Raw value: {rawValue}");

                // turn off input
                if (parameter.Type == "DC")
                {
                    // set voltage
                    _powerSupplyService.ClosePMU();
                }
                else if (parameter.Type == "AC")
                {
                    // start function generator
                    _functionSwitchService.StopFunctionGen();
                }

                // turn off - power supply
                // supply.ToggleDPSNeg(false);

                // turn off + power supply
                // supply.ToggleDPSPos(false);
            }

            // reset wiring
            _switchMatrixService.Reset();
        }

        // Close Services
        _powerSupplyService.Close();
        _measureService.Close();
        _switchMatrixService.Close();
        _functionSwitchService.Close();
    }
}
