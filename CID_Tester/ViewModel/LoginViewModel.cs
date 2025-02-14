using CID_Tester.Exceptions;
using CID_Tester.Model;
using CID_Tester.Service.DbCreator;
using CID_Tester.Service.DbProvider;
using CID_Tester.ViewModel.Command;
using System.Windows;
using System.Windows.Input;

namespace CID_Tester.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IDbProvider _dbProvider;
        private readonly IDbCreator _dbCreator;

        public event EventHandler? ClosingRequest;
        public ICommand LoginCommand { get; }

        private string _username = "neurocoder";
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

        private string _password = "password";
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

        public LoginViewModel(IDbProvider dbProvider, IDbCreator dbCreator)
        {
            _dbProvider = dbProvider;
            _dbCreator = dbCreator;
            UserLoginCommand command = new UserLoginCommand(this);
            command.LoginRequestEvent += async (sender, e) => await LoginRequestEventHandler();
            LoginCommand = command;
            
        }

        private async Task LoginRequestEventHandler()
        {
            try
            {
                TEST_USER? user = await _dbProvider.GetUser(_username, _password, _dbCreator);
                if (user != null)
                {
                    MainWindow main = new MainWindow()
                    {
                        DataContext = new MainViewModel(user, _dbProvider, _dbCreator)
                    };
                    main.Show();
                    ClosingRequest?.Invoke(this, EventArgs.Empty);
                }
            } catch (IncorrectLoginException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
