using AvalonDock.Controls;
using CID_Tester.Model;
using CID_Tester.Service.DbCreator;
using CID_Tester.Service.DbProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CID_Tester.ViewModel
{
    public class DevicesViewModel : BaseViewModel
    {
        private readonly IDbCreator _dbCreator;
        private readonly IDbProvider _dbProvider;

        public string Title { get; set; }

        private ICollection<DUT> _devices;
        public ICollection<DUT> Devices
        {
            get { return _devices; }
            set
            {
                _devices = value;
                onPropertyChanged(nameof(Devices));
            }
        }

        public DevicesViewModel(string title, IDbProvider dbProvider, IDbCreator dbCreator)
        {
            Title = title;
            _dbCreator = dbCreator;
            _dbProvider = dbProvider;
            Load();
        }

        private async void Load()
        {
            var devices = await _dbProvider.GetAllDuts();
            Devices = devices.ToList();
        }
    }
}
