using CID_Tester.Model;
using CID_Tester.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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

        public override void Execute(object parameter)
        {
            LoginRequestEvent?.Invoke(this, EventArgs.Empty);
        }
    }
}
