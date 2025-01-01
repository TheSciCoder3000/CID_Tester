using CID_Tester.Command;
using CID_Tester.Model;
using CID_Tester.View;
using System.Windows;
using System.Windows.Input;

namespace CID_Tester.ViewModel
{
    public class DevicesViewModel : BaseViewModel, IDocument
    {
        private readonly Store _AppStore;

        public string Title { get; set; }

        private ICollection<DUT> _devices = [];
        public ICollection<DUT> Devices
        {
            get { return _devices; }
            set
            {
                _devices = value;
                onPropertyChanged(nameof(Devices));
            }
        }

        public ICommand AddDutCommand { get; }
        public ICommand CloseCommand { get; }

        public DevicesViewModel(Store appStore)
        {
            Title = "Devices";

            _AppStore = appStore;
            _AppStore.OnDutUpdated += Load;

            CloseCommand = new RelayCommand(CloseCommandHanlder);
            AddDutCommand = new RelayCommand(ShowDutForm);
            Load();
        }

        private void ShowDutForm(object? obj)
        {
            Window addDutView = new AddDutView();
            BaseViewModel vm = new AddDutViewModel(_AppStore, () => addDutView.Close());
            addDutView.DataContext = vm;
            addDutView.ShowDialog();
        }

        private void CloseCommandHanlder(object? parameter) => _AppStore.RemoveDocument(this);

        private async void Load(IEnumerable<DUT>? devices = null)
        {
            if (devices == null)
            {
                devices = await _AppStore.GetAllDuts();
            }
            Devices = devices.ToList();
        }
    }
}
