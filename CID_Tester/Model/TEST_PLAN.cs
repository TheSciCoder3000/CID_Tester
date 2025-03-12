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

            for (int dut_indx = 0; dut_indx < 0;  dut_indx++)
            {
                foreach (var parameter in TEST_PARAMETERS)
                {
                    Debug.WriteLine(parameter.Name);
                    // communicate with mainboard to setup relay
                    matrix.Start(parameter.ParseToParameterDictionary());

                    // communicate with PSU
                    

                    // communicate with measuring devices to start meeasuring
                    await measure.SetModeVoltage();
                    string value = await measure.GetMeasurement();
                }

                matrix.ChangeDut(dut_indx);
            }


            measure.Close();
            matrix.Close();
        }
    }
}
