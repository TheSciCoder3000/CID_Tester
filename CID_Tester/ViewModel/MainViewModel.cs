using CID_Tester.ViewModel.Command;
using CID_Tester.Model;
using CID_Tester.Service.DbCreator;
using CID_Tester.Service.DbProvider;
using System.Windows.Input;
using CID_Tester.ViewModel.Command.Navigation;
using CID_Tester.ViewModel.DebugSDK;

namespace CID_Tester.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private readonly Store _AppStore;
        private PS2000 Oscilloscope;
        private PS2000SigGen SigGen;

        private IEnumerable<BaseViewModel> _documents = [];
        public IEnumerable<BaseViewModel> Documents
        {
            get => _documents;
            set
            {
                _documents = value;
                onPropertyChanged(nameof(Documents));
            }
        }

        private IEnumerable<BaseViewModel> _anchorables = [];
        public IEnumerable<BaseViewModel> Anchorables
        {
            get => _anchorables;
            set
            {
                _anchorables = value;
                onPropertyChanged(nameof(Anchorables));
            }
        }

        private object _activeDocument = null!;
        public object ActiveDocument
        {
            get => _activeDocument;
            set
            {
                _activeDocument = value;
                onPropertyChanged(nameof(ActiveDocument));
            }
        }

        public ToolbarViewModel toolbarViewModel { get; }

        public ICommand NavigateToDashboard { get; } = null!;
        public ICommand NavigateToDevices { get; } = null!;
        public ICommand NavigateToTestPlan { get; } = null!;
        public ICommand NavigateToResults { get; } = null!;
        public ICommand NavigateToSettings { get; } = null!;
        public ICommand NavigateToDebug { get; } = null!;

        public ICommand AddDutCommand { get; } = null!;
        public ICommand CloseCommand { get; } = null!;


        public MainViewModel(TEST_USER user, IDbProvider dbProvider, IDbCreator dbCreator)
        {
            _AppStore = new Store(dbProvider, dbCreator, user, []);
            _AppStore.OnDocumentOpenned         += LoadDocuments;
            _AppStore.OnDocumentClosed          += LoadDocuments;
            _AppStore.OnActiveDocumentChanged   += UpdateActiveDocument;
            _AppStore.OnAnchorableAdded         += LoadAnchorables;
            _AppStore.OnAnchorableRemoved       += LoadAnchorables;

            toolbarViewModel = new ToolbarViewModel(_AppStore);

            Oscilloscope = new PS2000();
            SigGen = new PS2000SigGen();
            Oscilloscope.Start();

            // Initialize Navigation Commands
            NavigateToTestPlan  = new NavigateTestPlan(_AppStore);
            NavigateToDashboard = new NavigateDashboard(_AppStore, NavigateToTestPlan);
            NavigateToDevices   = new NavigateDevices(_AppStore);
            NavigateToResults   = new NavigateResults(_AppStore);
            NavigateToDebug = new NavigateDebug(_AppStore, Oscilloscope, SigGen);


        }

        private void UpdateActiveDocument(BaseViewModel activeDocument) => ActiveDocument = activeDocument;
        private void LoadDocuments(IEnumerable<BaseViewModel> documents) => Documents = documents;
        private void LoadAnchorables(IEnumerable<BaseViewModel> anchorables) => Anchorables = anchorables;
    }
}
