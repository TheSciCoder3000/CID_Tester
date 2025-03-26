
using CID_Tester.Model;
using CID_Tester.Service.Serial;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CID_Tester.ViewModel.Controls.History;

public class OutputGraphViewModel : BaseViewModel
{
    private TEST_OUTPUT _testOutput;

    private ImageSource _imageSource;
    public ImageSource ImageDir
    {
        get => _imageSource;
        set
        {
            _imageSource = value;
            onPropertyChanged(nameof(ImageDir));
        }
    }

    public OutputGraphViewModel(int cycleNo, int dutLocation, TEST_OUTPUT testOutput)
    {
        _testOutput = testOutput;

        //string appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MyApp");
        string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        string imagePath = Path.Combine(appDataPath, testOutput.Measured);

        ImageDir = new BitmapImage(new Uri(imagePath));
    }
}
