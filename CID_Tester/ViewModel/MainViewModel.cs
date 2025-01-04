﻿using CID_Tester.ViewModel.Command;
using CID_Tester.Model;
using CID_Tester.Service.DbCreator;
using CID_Tester.Service.DbProvider;
using System.Windows.Input;

namespace CID_Tester.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private readonly Store _AppStore;

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

        private object _activeDocument;
        public object ActiveDocument
        {
            get => _activeDocument;
            set
            {
                _activeDocument = value;
                onPropertyChanged(nameof(ActiveDocument));
            }
        }

        public ICommand NavigateToDashboard { get; } = null!;
        public ICommand NavigateToDevices { get; } = null!;
        public ICommand NavigateToTestPlan { get; } = null!;
        public ICommand NavigateToResults { get; } = null!;
        public ICommand NavigateToSettings { get; } = null!;

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

            // Initialize Navigation Commands
            NavigateToTestPlan  = new NavigateCommand(AddDocumentHandler, new TestPlanViewModel (_AppStore));
            NavigateToDevices   = new NavigateCommand(AddDocumentHandler, new DevicesViewModel  (_AppStore));
            NavigateToDashboard = new NavigateCommand(AddDocumentHandler, new DashboardViewModel(_AppStore, NavigateToTestPlan));
            NavigateToResults   = new NavigateCommand(AddDocumentHandler, new ResultsViewModel  (_AppStore));
            NavigateToSettings  = new NavigateCommand(AddDocumentHandler, new ResultsViewModel  (_AppStore));
        }

        private void UpdateActiveDocument(BaseViewModel activeDocument) => ActiveDocument = activeDocument;

        private void LoadDocuments(IEnumerable<BaseViewModel> documents) => Documents = documents;
        private void LoadAnchorables(IEnumerable<BaseViewModel> anchorables) => Anchorables = anchorables;
        private void AddDocumentHandler(BaseViewModel viewModel) => _AppStore.AddDocument(viewModel);
    }
}
