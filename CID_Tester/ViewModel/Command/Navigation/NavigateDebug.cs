﻿using CID_Tester.Model;
using CID_Tester.ViewModel.Document;
using CID_Tester.ViewModel.DebugSDK;

namespace CID_Tester.ViewModel.Command.Navigation;

public class NavigateDebug(Store appStore, PS2000 oscilloscope) : CommandBase
{
    private readonly Store _AppStore = appStore;

    public override void Execute(object? parameter)
    {
        _AppStore.AddDocument<DebugViewModel>(new DebugViewModel(appStore, oscilloscope));
    }
}
