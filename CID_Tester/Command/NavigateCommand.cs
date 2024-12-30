using CID_Tester.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CID_Tester.Command
{
    class NavigateCommand : CommandBase
    {
        private readonly MainViewModel _viewModel;
        private readonly BaseViewModel _destViewModel;

        public NavigateCommand(MainViewModel viewModel, BaseViewModel destinationViewModel)
        {
            _viewModel = viewModel;
            _destViewModel = destinationViewModel;
        }

        public override void Execute(object? parameter)
        {
            //_viewModel.NavigateTo(_destViewModel);
            if (!_viewModel.Documents.Contains(_destViewModel))
            {
                _viewModel.Documents.Add(_destViewModel);
            }
            _viewModel.ActiveDocument = _viewModel.Documents[_viewModel.Documents.IndexOf(_destViewModel)];
        }
    }
}
