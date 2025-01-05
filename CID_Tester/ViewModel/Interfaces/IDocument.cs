using System.Windows.Input;

namespace CID_Tester.ViewModel.Interfaces;

public interface IDocument
{
    public string Title { get; }
    public ICommand CloseCommand { get; }
}
