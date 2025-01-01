using CID_Tester.ViewModel;

namespace CID_Tester.Command
{
    public class UserLoginCommand : CommandBase
    {
        private readonly LoginViewModel _viewModel;
        public event EventHandler? LoginRequestEvent;
        public UserLoginCommand(LoginViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override void Execute(object? parameter)
        {
            LoginRequestEvent?.Invoke(this, EventArgs.Empty);
        }
    }
}
