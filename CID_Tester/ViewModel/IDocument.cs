﻿using System.Windows.Input;

namespace CID_Tester.ViewModel
{
    public interface IDocument
    {
        public string Title { get; }
        public ICommand CloseCommand { get; }
    }
}