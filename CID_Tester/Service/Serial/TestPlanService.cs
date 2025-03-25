using CID_Tester.Model;
using CID_Tester.Service.DbCreator;
using System.Diagnostics;
using System.Media;
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
    private TEST_BATCH _testBatch;
    private TEST_USER _testUser;

    public CancellationTokenSource? TokenSource;

    public TestPlanService(TEST_USER testUser, IDbCreator dbCreator)
    {
        _testUser = testUser;
        _dbCreator = dbCreator;
        Initialize();
    }

    public int Initialize()
    {
        _powerSupplyService = new PowerSupply();
        _measureService = new Measure();
        _functionSwitchService = new FunctionSwitchService();
        _switchMatrixService = new SwitchMatrix();

        return _measureService.Unconnected() + _powerSupplyService.Unconnected() + _functionSwitchService.Unconnected() + _switchMatrixService.Unconnected();
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
            _testBatch = new TEST_BATCH()
            {
                TEST_PLAN = TestPlan,
                Date = DateTime.Now,
                CycleNo = 3,        // TODO: MAKE THIS DYNAMIC
                TEST_USER = _testUser
            };
            SoundPlayer soundPlayer = new SoundPlayer("Start.wav");
            soundPlayer.Play();

            TokenSource = new CancellationTokenSource();
            var token = TokenSource.Token;


            for (int cycle = 0; cycle < 3; cycle++)
            {
                Debug.WriteLine($"Test Cycle: {cycle + 1}");
                await RunTests(token);
            }
            soundPlayer = new SoundPlayer("Alarm.wav");
            soundPlayer.Play();

        }
        catch (OperationCanceledException ex)
        {
            TokenSource?.Dispose();
            TokenSource = null;
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message, "Test Plan Unable to Start", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        await _dbCreator.CreateBatch(_testBatch);


        await Task.Delay(2000);
        OnTestComplete?.Invoke();
        CommandManager.InvalidateRequerySuggested();
    }

    private async Task RunTests(CancellationToken token)
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
                if (token.IsCancellationRequested)
                {
                    _powerSupplyService.ClosePMU();
                    _functionSwitchService.StopFunctionGen();
                    _switchMatrixService.Reset();
                    token.ThrowIfCancellationRequested();
                }
                // switch to dut num
                Debug.WriteLine($"Switching to DUT {dutNum}");
                _switchMatrixService.ChangeDut(dutNum);

                // turn on + power supply
                // supply.ToggleDPSPos(true);

                // turn on - power supply
                // supply.ToggleDPSNeg(true);

                // turn on input
                if (parameter.Type == "DC")
                {
                    // set voltage and open PMU
                    Debug.WriteLine($"Running dc test: {parameter.Name}");
                    _powerSupplyService.StartPMU(parameter.Type, parameter.InputConfiguration);

                    // start measurement
                    await _measureService.SetModeVoltage();
                    string rawValue = await _measureService.GetMeasurement();

                    TEST_OUTPUT result = new TEST_OUTPUT()
                    {
                        Measured = rawValue,
                        TEST_PARAMETER = parameter,
                        DutLocation = dutNum,
                    };
                    _testBatch.TEST_OUTPUTS.Add(result);

                    Debug.WriteLine($"Raw value: {rawValue}");

                    // close pmu
                    _powerSupplyService.ClosePMU();
                }
                else if (parameter.Type == "AC")
                {
                    // start function generator
                    _functionSwitchService.ParseInputConfiguration(parameter.InputConfiguration);
                    _functionSwitchService.StartFunctionGen();

                    // capture graph
                    string fullResultPath = _functionSwitchService.CaptureGraph($"D{dutNum}-P{parameter.ParamCode}-B{_testBatch.BatchCode}");

                    // stop function generator
                    _functionSwitchService.StopFunctionGen();

                    TEST_OUTPUT result = new TEST_OUTPUT()
                    {
                        Measured = fullResultPath,
                        TEST_PARAMETER = parameter,
                        DutLocation = dutNum,
                    };
                    _testBatch.TEST_OUTPUTS.Add(result);
                }

                // turn off - power supply
                // supply.ToggleDPSNeg(false);

                // turn off + power supply
                // supply.ToggleDPSPos(false);
            }

            // reset wiring
            _switchMatrixService.Reset();
            Thread.Sleep(1000);
        }

        // Close Services
        _powerSupplyService.Close();
        _measureService.Close();
        _switchMatrixService.Close();
        _functionSwitchService.Close();
    }
}
