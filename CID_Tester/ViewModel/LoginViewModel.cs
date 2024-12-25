using CID_Tester.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CID_Tester.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        public event EventHandler? ClosingRequest;
        public ICommand LoginCommand { get; }

        private string _username;
        public string Username
        {
            get
            {
                return _username;
            }
            set
            {
                _username = value;
                onPropertyChanged(nameof(Username));
            }
        }

        private string _password;
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
                onPropertyChanged(nameof(Password));
            }
        }

        public LoginViewModel()
        {
            UserLoginCommand command = new UserLoginCommand(this);
            command.CloseLoginRequest += (sender, e) => ClosingRequest?.Invoke(sender, EventArgs.Empty);
            LoginCommand = command;
            
        }
    }
}
