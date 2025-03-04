using CID_Tester.Model;
using CID_Tester.Service.DbCreator;
using CID_Tester.Service.DbProvider;
using CID_Tester.ViewModel.Command;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Input;

namespace CID_Tester.ViewModel;

internal class RegisterViewModel : BaseViewModel
{
    private readonly IDbProvider _dbProvider;
    private readonly IDbCreator _dbCreator;

    public event EventHandler? ClosingRequest;

    public ICommand LoginCommand { get; }
    public ICommand RegisterCommand { get; }

    private string _username = null!;
    private string _email = null!;
    private string _firstName = null!;
    private string _lastName = null!;
    private string _password = null!;
    private string _confirmPassword = null!;

    public string Username
    {
        get => _username;
        set
        {
            _username = value;
            onPropertyChanged(nameof(Username));
        }
    }
    public string Email
    {
        get => _email;
        set
        {
            _email = value;
            onPropertyChanged(nameof(Email));
        }
    }
    public string FirstName
    {
        get => _firstName;
        set
        {
            _firstName = value;
            onPropertyChanged(nameof(FirstName));
        }
    }
    public string LastName
    {
        get => _lastName;
        set
        {
            _lastName = value;
            onPropertyChanged(nameof(LastName));
        }
    }
    public string Password
    {
        get => _password;
        set
        {
            _password = value;
            onPropertyChanged(nameof(Password));
        }
    }
    public string ConfirmPassword
    {
        get => _confirmPassword;
        set
        {
            _confirmPassword = value;
            onPropertyChanged(nameof(ConfirmPassword));
        }
    }

    public RegisterViewModel(IDbProvider dbProvider, IDbCreator dbCreator)
    {
        _dbProvider = dbProvider;
        _dbCreator = dbCreator;
        LoginCommand = new RelayCommand(OpenLoginWindow);
        RegisterCommand = new RelayCommand(RegisterUser);
    }

    private async void RegisterUser(object? obj)
    {
        try
        {
            if (_password != _confirmPassword)  new Exception("Passwords do not match");

            TEST_USER user = new TEST_USER
            {
                Username = _username,
                Email = _email,
                FirstName = _firstName,
                LastName = _lastName,
                ProfileImage = "",
                Password = BCrypt.Net.BCrypt.HashPassword(_password)
            };
            await _dbCreator.CreateUser(user);
            MessageBox.Show($"Successfuly created user: {Username}");
            OpenLoginWindow(null);
        } catch (Exception e)
        {
            MessageBox.Show(e.Message);
        }
    }

    private void OpenLoginWindow(object? obj)
    {
        LoginViewModel vm = new LoginViewModel(_dbProvider, _dbCreator);
        Login login = new Login();
        vm.ClosingRequest += (sender, e) => login.Close();
        login.DataContext = vm;
        login.Show();
        ClosingRequest?.Invoke(obj, EventArgs.Empty);
    }


}
