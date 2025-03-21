using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CID_Tester.Service.Serial;
using System.Diagnostics;
using System.Windows.Input;

namespace CID_Tester.Model
{
    public class TEST_PLAN
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TestCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public int CycleNo { get; set; }
        public int TestTime { get; set; }

        public DUT DUT { get; set; } = null!;

        public TEST_USER TEST_USER { get; set; } = null!;

        public ICollection<TEST_PARAMETER> TEST_PARAMETERS { get; set; } = [];

        public async void Start(Action? OnTestComplete = null)
        {
            for (int cycle = 0; cycle < CycleNo; cycle++)
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
            Measure measure = new Measure("COM10");
            SwitchMatrix matrix = new SwitchMatrix("COM8");
            PowerSupply supply = new PowerSupply("COM7");

            foreach (var parameter in TEST_PARAMETERS)
            {
                // setup wiring

                for (int dutNum = 1; dutNum <= 4; dutNum++)
                {
                    // switch to dut num
                    matrix.ChangeDut(dutNum);

                    // turn on + power supply
                    // supply.ToggleDPSPos(true);

                    // turn on - power supply
                    // supply.ToggleDPSNeg(true);

                    // turn on input

                    // start measurement
                    await measure.SetModeVoltage();
                    string rawValue = await measure.GetMeasurement();

                    // turn off input

                    // turn off - power supply
                    // supply.ToggleDPSNeg(false);

                    // turn off + power supply
                    // supply.ToggleDPSPos(false);
                }

                // reset wiring
                matrix.Reset();
            }


            measure.Close();
            matrix.Close();
        }
    }
}
