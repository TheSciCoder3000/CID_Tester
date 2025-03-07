using CID_Tester.ViewModel.Command;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CID_Tester.ViewModel.Anchorables
{
    public class ParametersItemViewModel : BaseViewModel
    {
        private string _RelayName = string.Empty;
        private bool _IsRelayOn;
        
        public string RelayName
        {
            get => _RelayName;
            set
            {
                _RelayName = value;
                onPropertyChanged(nameof(RelayName));
            }
        }
        public bool IsRelayOn
        {
            get => _IsRelayOn;
            set
            {
                _IsRelayOn = value;
                onPropertyChanged(nameof(IsRelayOn));
            }
        }
        public ICommand ToggleRelayStateCommand { get; }
        public Action<string, bool>? RelayStateChanged;

        public ParametersItemViewModel(string relayString, Action<string, bool> relayAction)
        {
            var relayStringSplit = relayString.Split('=');
            RelayName = relayStringSplit[0];
            IsRelayOn = relayStringSplit[1] == "True";
            ToggleRelayStateCommand = new RelayCommand(ToggleRelayState);
            RelayStateChanged = relayAction;
        }

        private void ToggleRelayState(object? obj)
        {
            RelayStateChanged?.Invoke(_RelayName, !_IsRelayOn);
            IsRelayOn = !IsRelayOn;
        }
    }
}
