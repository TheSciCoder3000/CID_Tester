using CID_Tester.Service.DbCreator;
using CID_Tester.Service.DbProvider;
using CID_Tester.ViewModel;
using Microsoft.Identity.Client;
using System.Reflection.Metadata;

namespace CID_Tester.Model
{
    public class Store
    {
        private readonly IDbProvider _dbProvider;
        private readonly IDbCreator _dbCreator;

        public IEnumerable<DUT> DUTs { get; private set; } = [];
        public TEST_USER TestUser { get; private set; }
        public TEST_PLAN? TestPlan { get; private set; }
        public IEnumerable<BaseViewModel> Documents { get; private set; }
        public object ActiveDocument { get; set; } = null!;
        public IEnumerable<BaseViewModel> Anchorables { get; private set; } = [];

        #region Events

        public event Action<IEnumerable<DUT>> OnDutCreated;
        public event Action<IEnumerable<DUT>> OnDutUpdated;
        public event Action<IEnumerable<DUT>> OnDutDeleted;

        public event Action<TEST_USER> OnTestUserUpdated;

        public event Action<IEnumerable<BaseViewModel>> OnDocumentOpenned;
        public event Action<IEnumerable<BaseViewModel>> OnDocumentClosed;
        public event Action<BaseViewModel> OnActiveDocumentChanged;

        #endregion

        public Store(IDbProvider dbProvider, IDbCreator dbCreator, TEST_USER testUser, IEnumerable<BaseViewModel> documents)
        {
            _dbProvider = dbProvider;
            _dbCreator = dbCreator;
            TestUser = testUser;
            Documents = documents;

            LoadDut();
        }

        #region Document Functions

        public void AddDocument(BaseViewModel document)
        {
            ICollection<BaseViewModel> DocumentCollection = Documents.ToList();
            BaseViewModel? activeDocument = DocumentCollection.FirstOrDefault(d => d == document);

            if ( activeDocument == null)
            {
                DocumentCollection.Add(document);
                Documents = DocumentCollection;
                ActiveDocument = document;
                OnDocumentOpenned?.Invoke(Documents);
            }
            ActiveDocument = document;
            OnActiveDocumentChanged?.Invoke(document);
        }

        public void RemoveDocument(BaseViewModel document)
        {
            ICollection<BaseViewModel> DocumentCollection = Documents.ToList();
            DocumentCollection.Remove(document);
            Documents = DocumentCollection;
            OnDocumentClosed?.Invoke(Documents);
        }

        #endregion

        #region DUT Functions

        private async void LoadDut()
        {
            DUTs = await _dbProvider.GetAllDuts();
            OnDutUpdated?.Invoke(DUTs);
        }

        public async Task<IEnumerable<DUT>> GetAllDuts()
        {
            return await _dbProvider.GetAllDuts();
        }

        public async Task CreateDut(DUT dut)
        {
            await _dbCreator.CreateDUT(dut);
            LoadDut();
            OnDutCreated?.Invoke(DUTs);
        }

        #endregion
    }
}
