using CID_Tester.ViewModel.Command;
using CID_Tester.Model;
using CID_Tester.ViewModel.Interfaces;
using System.Windows.Input;

namespace CID_Tester.ViewModel.Windows;

public class AddDutViewModel : BaseViewModel, IDocument
{
    private readonly Action _closeDialog;

    private readonly Store _AppStore;
    private string _dutName = null!;
    public string DutName
    {
        get { return _dutName; }
        set
        {
            _dutName = value;
            onPropertyChanged(nameof(DutName));
        }
    }

    private string _dutDescription = null!;
    public string DutDescription
    {
        get { return _dutDescription; }
        set
        {
            _dutDescription = value;
            onPropertyChanged(nameof(DutDescription));
        }
    }

    public ICommand AddDutCommand { get; }

    public string Title { get; }

    public ICommand CloseCommand { get; }

    public AddDutViewModel(Store appStore)
    {
        Title = "Add Devices";
        _AppStore = appStore;
        AddDutCommand = new RelayCommand(CreateDutHandler);
        CloseCommand = new RelayCommand(RemoveAnchorable);
    }

    private void RemoveAnchorable(object? obj)
    {
        _AppStore.RemoveAnchorable(this);
    }

    private async void CreateDutHandler(object? obj)
    {
        await _AppStore.CreateDut(new DUT(DutName, DutDescription));
        //_closeDialog.Invoke();
    }
}
