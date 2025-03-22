using CID_Tester.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

    public TestPlanService()
    {
        _powerSupplyService = new PowerSupply("");
        _switchMatrixService = new SwitchMatrix("COM8");
        _measureService = new Measure("");
        _functionSwitchService = new FunctionSwitchService("COM12");
    }

    public async void Start(Action? OnTestComplete = null)
    {
        if (_testPlan == null)
        {
            Debug.WriteLine("Test plan is null");
            return;
        }

        for (int cycle = 0; cycle < _testPlan.CycleNo; cycle++)
        {
            Debug.WriteLine($"Test: {cycle + 1}");
            await RunTests();
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

            for (int dutNum = 1; dutNum <= 4; dutNum++)
            {
                // switch to dut num
                _switchMatrixService.ChangeDut(dutNum);

                // turn on + power supply
                // supply.ToggleDPSPos(true);

                // turn on - power supply
                // supply.ToggleDPSNeg(true);

                // turn on input

                // start measurement
                await _measureService.SetModeVoltage();
                string rawValue = await _measureService.GetMeasurement();

                // turn off input

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
