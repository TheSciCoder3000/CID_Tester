using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CID_Tester.ViewModel.Controls.DebugControls;

public class SwitchMatrixPanelViewModel
{
    public ICommand ToggleDut1Command { get; }
    public ICommand ToggleDut2Command { get; }
    public ICommand ToggleDut3Command { get; }
    public ICommand ToggleDut4Command { get; }
    public ICommand ToggleOffCommand { get; }

    public IEnumerable<DebugParameterItemViewModel> ParameterItems { get; set; }
}
