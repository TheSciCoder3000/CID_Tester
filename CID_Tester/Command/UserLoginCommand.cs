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
        public event EventHandler? CloseLoginRequest;
        public UserLoginCommand(LoginViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override void Execute(object parameter)
        {
            using (var context = new DataContext())
            {
                foreach (var user in context.TEST_USER.ToList())
                {
                    if (_viewModel.Username == user.USER_NAME && _viewModel.Password == user.PASSWORD)
                    {

                        MainWindow main = new MainWindow()
                        {
                            DataContext = new MainViewModel(user)
                        };
                        main.Show();
                        CloseLoginRequest?.Invoke(this, EventArgs.Empty);
                    }

                }
            }
        }
    }
}
