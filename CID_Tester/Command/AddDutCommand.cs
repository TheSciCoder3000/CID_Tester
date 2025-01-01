

using CID_Tester.Model;
using CID_Tester.Service.DbCreator;
using CID_Tester.ViewModel;

namespace CID_Tester.Command
{
    public class AddDutCommand : CommandBase
    {
        private readonly IDbCreator _dbCreator;
        private readonly AddDutViewModel _viewModel;
        public event EventHandler ExecutionFinished;

        public AddDutCommand(AddDutViewModel viewModel, IDbCreator dbCreator)
        {
            _dbCreator = dbCreator;
            _viewModel = viewModel;
        }

        public override async void Execute(object? parameter)
        {
            await _dbCreator.CreateDUT(new DUT(_viewModel.DutName, _viewModel.DutDescription));
            ExecutionFinished?.Invoke(this, EventArgs.Empty);
        }
    }
}
