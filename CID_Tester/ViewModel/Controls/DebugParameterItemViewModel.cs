using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CID_Tester.ViewModel.Controls;

public class DebugParameterItemViewModel : BaseViewModel
{
    
    private bool _isParameterActive = false;
    public bool IsParameterActive
    {
        get => _isParameterActive;
        set
        {
            _isParameterActive= value;
            onPropertyChanged(nameof(IsParameterActive));
        }
    }

    
}
