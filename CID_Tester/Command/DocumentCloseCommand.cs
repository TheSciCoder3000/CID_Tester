using CID_Tester.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CID_Tester.Command
{
    public class DocumentCloseCommand : CommandBase
    {
        public DocumentCloseCommand(Action<BaseViewModel> closeFunction)
        {
            _closeFunction = closeFunction;
        }

        private Action<BaseViewModel> _closeFunction;

        public override void Execute(object? parameter)
        {
            throw new NotImplementedException();
        }
    }
}
