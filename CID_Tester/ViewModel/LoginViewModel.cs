using CID_Tester.Command;
using CID_Tester.DbContexts;
using CID_Tester.Model;
using CID_Tester.Service.DbProvider;
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
        private readonly DbProvider _dbProvider;

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

        public LoginViewModel(DbProvider dbProvider)
        {
            _dbProvider = dbProvider;
            UserLoginCommand command = new UserLoginCommand(this);
            command.LoginRequestEvent += async (sender, e) => await LoginRequestEventHandler();
            LoginCommand = command;
            
        }

        private async Task LoginRequestEventHandler()
        {
            TEST_USER? user = await _dbProvider.GetUser(_username, _password);
            if (user != null)
            {
                MainWindow main = new MainWindow()
                {
                    DataContext = new MainViewModel(user)
                };
                main.Show();
                ClosingRequest?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
