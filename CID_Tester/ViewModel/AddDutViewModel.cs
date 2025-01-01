using CID_Tester.Command;
using CID_Tester.Model;
using CID_Tester.Service.DbCreator;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CID_Tester.ViewModel
{
    public class AddDutViewModel : BaseViewModel
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

        public AddDutViewModel(Store appStore, Action CloseDialog)
        {
            _AppStore = appStore;
            _closeDialog = CloseDialog;
            AddDutCommand = new RelayCommand(CreateDutHandler);
        }

        private async void CreateDutHandler(object? obj)
        {
            await _AppStore.CreateDut(new DUT(DutName, DutDescription));
            _closeDialog.Invoke();
        }
    }
}
