using CID_Tester.Model;
using OfficeOpenXml;
using System.IO;
using Microsoft.Win32;
using System.Windows;
using CID_Tester.Exceptions;
using System.Windows.Input;
using CID_Tester.ViewModel.Command;
using System.Globalization;
using System.Diagnostics;

namespace CID_Tester.ViewModel.Controls.AddTestPlan;

public class AddTestPlanImporterViewModel : BaseViewModel
{
    private readonly Store _AppStore;
    private readonly Action _closeDialog;
    private TEST_PLAN? _testPlan;

    public IEnumerable<TEST_PARAMETER>? TestParameters
    {
        get => _testPlan?.TEST_PARAMETERS;
    }

    public ICommand SaveCommand { get; }
    public ICommand ImportCommand { get; }

    public AddTestPlanImporterViewModel(Store appStore, Action closeDialog)
    {
        _AppStore = appStore;
        _closeDialog = closeDialog;
        ImportCommand = new RelayCommand((obj) => ImportFileDialog());
        SaveCommand = new RelayCommand(SaveTestPlan, (obj) => _testPlan != null);

        ImportFileDialog();
    }

    private async void SaveTestPlan(object? obj)
    {
        if (_testPlan != null)
        {
            await _AppStore.CreateTestPlan(_testPlan);
            _closeDialog();
        }
    }

    private void ImportFileDialog()
    {
        OpenFileDialog openFileDialog = new OpenFileDialog();
        openFileDialog.Filter = "Excel Files (*.xlsx;*.xls)|*.xlsx;*.xls";
        openFileDialog.Multiselect = false;
        openFileDialog.Title = "Import Excel File";

        if (openFileDialog.ShowDialog() == true)
        {
            readExcelFile(openFileDialog.FileName);
            onPropertyChanged(nameof(TestParameters));
        }
    }

    private void readExcelFile(string filePath)
    {
        // Ensure EPPlus can use non-commercial license
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        using (var package = new ExcelPackage(new FileInfo(filePath)))
        {
            try
            {

                var worksheet = package.Workbook.Worksheets[0];
                int rows = worksheet.Dimension.Rows;

                // Test Plan Data
                string testName = worksheet.Cells[1, 2].Text;

                string deviceName = worksheet.Cells[3, 3].Text;
                DUT? device = _AppStore.DUTs.FirstOrDefault(dut => dut.DutName == deviceName) ?? throw new ExcelFormatException("Device in the Test Plan does not exist in the database");

                int testCycNo = int.Parse(worksheet.Cells[4, 3].Text);
                string testDesc = worksheet.Cells[5, 3].Text;

                _testPlan = new TEST_PLAN()
                {
                    Name = testName,
                    Description = testDesc,
                    Date = DateTime.Now,
                    CycleNo = testCycNo,
                    TestTime = 0,
                    TEST_USER = _AppStore.TestUser,
                    DUT = device,
                };

                // ==================== DC PARAMETERS ==================== 
                int row = 11;
                while (worksheet.Cells[row, 1].Text != "")
                {
                    string paramName = worksheet.Cells[row, 1].Text;
                    string nonInv = worksheet.Cells[row, 2].Text;
                    string inv = worksheet.Cells[row, 3].Text;
                    string rIn = worksheet.Cells[row, 4].Text;
                    string rF = worksheet.Cells[row, 5].Text;
                    var (target, unit) = parseRawTarget(worksheet.Cells[row, 6].Text);
                    string parameters = ParseParameterToString(worksheet, row);
                    string inputConfiguration = ParseInputConfiguration("DC", worksheet, row);

                    _testPlan.TEST_PARAMETERS.Add(new TEST_PARAMETER()
                    {
                        Name = paramName,
                        Description = $"Inverting={inv},\nNon-Inverting={nonInv},\nRin={rIn},\nRF={rF}",
                        Metric = unit,
                        Target = (decimal)target,
                        Parameters = parameters,
                        InputConfiguration = inputConfiguration
                    });

                    row += 12;
                }

                // Loop through images in the worksheet
                //foreach (var drawing in worksheet.Drawings)
                //{
                //    if (drawing is ExcelPicture picture)
                //    {
                //        Debug.WriteLine($"Image Name: {picture.Name}");
                //        Debug.WriteLine($"Position: Row {picture.From.Row}, Column {picture.From.Column}");

                //        // Save the image to a file
                //        string outputPath = $@"C:\Users\drjjd\source\repos\CID_Tester\CID_Tester\images\samples\{picture.Name}.png";
                //        using (var imageStream = new MemoryStream(picture.Image.ImageBytes))
                //        {
                //            var image = Image.FromStream(imageStream);
                //            image.Save(outputPath);
                //            Debug.WriteLine($"Image saved to: {outputPath}");
                //        }
                //    }
                //}

                // ==================== AC PARAMETERS ==================== 
                var worksheetAc = package.Workbook.Worksheets[1];
                row = 11;
                while (worksheetAc.Cells[row, 1].Text != "")
                {
                    string paramName = worksheetAc.Cells[row, 1].Text;
                    string nonInv = worksheetAc.Cells[row, 2].Text;
                    string inv = worksheetAc.Cells[row, 3].Text;
                    string rIn = worksheetAc.Cells[row, 4].Text;
                    string rF = worksheetAc.Cells[row, 5].Text;
                    string parameters = ParseParameterToString(worksheetAc, row);
                    string inputConfiguration = ParseInputConfiguration("AC", worksheetAc, row);


                    _testPlan.TEST_PARAMETERS.Add(new TEST_PARAMETER()
                    {
                        Name = paramName,
                        Description = $"Inverting={inv},\nNon-Inverting={nonInv},\nRin={rIn},\nRF={rF}",
                        Metric = "GRAPH",
                        Target = 0,
                        Parameters = parameters,
                        Type = "AC",
                        InputConfiguration = inputConfiguration
                    });

                    row += 12;
                }
            }
            catch (ExcelFormatException fmtEx)
            {
                MessageBox.Show(fmtEx.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in parsing Excel file, please check the format");
                Debug.WriteLine(ex.Message);
            }
        }
    }

    private string ParseInputConfiguration(string type, ExcelWorksheet worksheet, int row)
    {
        if (type == "DC")
        {
            float InputVoltage = float.Parse(worksheet.Cells[row, 30].Text);
            string PMU1 = worksheet.Cells[row, 25].Text == "ON" ? "ON" : "OFF";
            string PMU2 = worksheet.Cells[row, 26].Text == "ON" ? "ON": "OFF";

            return $"PMU1={PMU1}, PMU2={PMU2}, Input={InputVoltage}";
        }
        else if (type == "AC")
        {
            float frequency = float.Parse(worksheet.Cells[row, 30].Text);
            float amplitude = float.Parse(worksheet.Cells[row, 31].Text);
            float timebase = float.Parse(worksheet.Cells[row, 32].Text);
            string signalType = worksheet.Cells[row, 33].Text;
            string FG1 = worksheet.Cells[row, 25].Text == "ON" ? "ON" : "OFF";
            string FG2 = worksheet.Cells[row, 26].Text == "ON" ? "ON" : "OFF";
            return $"FG1={FG1}, FG2={FG2}, frequency={frequency}, amplitude={amplitude}, timebase={timebase}, signalType={signalType}";
        }
        else
        {
            throw new ExcelFormatException("Incorrect Test Type");
        }
    }

    private string ParseParameterToString(ExcelWorksheet worksheet, int row)
    {
        Dictionary<string, bool> RelayColumnPairs = new Dictionary<string, bool>();

        // generate RelayColumnPairs data
        for (int col = 8; col <= 23; col++)
        {
            string relayName = worksheet.Cells[10, col].Text;
            bool relayState = worksheet.Cells[row, col].Text == "ON";
            RelayColumnPairs.Add(relayName, relayState);
        }

        return string.Join(", ", RelayColumnPairs.Select(pair => $"{pair.Key}={pair.Value}"));
    }

    private (float target, string unit) parseRawTarget(string text)
    {
        string rawTarget = string.Empty;
        string unit = string.Empty;

        foreach (char c in text)
        {
            if (char.IsDigit(c) || new[] { '.', '+', '-' }.Contains(c))
            {
                rawTarget += c;
            }
            else
            {
                unit += c;
            }
        }

        if (float.TryParse(rawTarget, NumberStyles.Float, CultureInfo.InvariantCulture, out float target))
        {
            return (target, unit);
        }
        else throw new ExcelFormatException($"Incorrect Number format for Target Parameter \"{text}\"");
    }
}
