using CID_Tester.Service.DbCreator;
using CID_Tester.Service.DbProvider;
using CID_Tester.ViewModel;

namespace CID_Tester.Model
{
    public class Store
    {
        private readonly IDbProvider _dbProvider;
        private readonly IDbCreator _dbCreator;

        public IEnumerable<DUT> DUTs { get; private set; } = [];
        public TEST_USER TestUser { get; private set; }

        private TEST_PLAN? _testPlan;
        public TEST_PLAN? TestPlan
        {
            get => _testPlan;
            set
            {
                _testPlan = value;
                OnTestPlanUpdated?.Invoke(value);
            }
        }
        public IEnumerable<BaseViewModel> Documents { get; private set; }
        public object ActiveDocument { get; set; } = null!;
        public IEnumerable<BaseViewModel> Anchorables { get; private set; } = [];

        #region Events

        public event Action<IEnumerable<DUT>> OnDutCreated;
        public event Action<IEnumerable<DUT>> OnDutUpdated;
        public event Action<IEnumerable<DUT>> OnDutDeleted;

        public event Action<TEST_USER> OnTestUserUpdated;

        public event Action<TEST_PLAN?> OnTestPlanUpdated;
        public event Action<IEnumerable<TEST_PARAMETER>> OnTestParameterUpdated;

        public event Action<IEnumerable<BaseViewModel>> OnDocumentOpenned;
        public event Action<IEnumerable<BaseViewModel>> OnDocumentClosed;
        public event Action<BaseViewModel> OnActiveDocumentChanged;
        public event Action<IEnumerable<BaseViewModel>> OnAnchorableAdded;
        public event Action<IEnumerable<BaseViewModel>> OnAnchorableRemoved;

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

        public void setActiveDocument(BaseViewModel document)
        {
            ActiveDocument = document;
            OnActiveDocumentChanged?.Invoke(document);
        }

        public void AddDocument(BaseViewModel document)
        {
            ICollection<BaseViewModel> DocumentCollection = Documents.ToList();
            BaseViewModel? activeDocument = DocumentCollection.FirstOrDefault(d => d == document);

            if ( activeDocument == null)
            {
                DocumentCollection.Add(document);
                Documents = DocumentCollection;
                OnDocumentOpenned?.Invoke(Documents);
            }
            if (ActiveDocument != document)
            {
                setActiveDocument(document);
                ClearAnchorables();
                OnAnchorableRemoved?.Invoke(Anchorables);
            }
        }

        public void RemoveDocument(BaseViewModel document)
        {
            ICollection<BaseViewModel> DocumentCollection = Documents.ToList();
            DocumentCollection.Remove(document);
            Documents = DocumentCollection;
            OnDocumentClosed?.Invoke(Documents);
        }

        public void AddAnchorables(BaseViewModel anchorable)
        {
            ICollection<BaseViewModel> AnchorableCollection = Anchorables.ToList();
            BaseViewModel? ancorableExist = AnchorableCollection.FirstOrDefault(d => d == anchorable);

            if (ancorableExist == null)
            {
                AnchorableCollection.Add(anchorable);
                Anchorables = AnchorableCollection;
                OnAnchorableAdded?.Invoke(Anchorables);
            }
        }

        public void RemoveAnchorable(BaseViewModel anchorable)
        {
            ICollection<BaseViewModel> AnchorableCollection = Anchorables.ToList();
            AnchorableCollection.Remove(anchorable);
            Anchorables = AnchorableCollection;
            OnAnchorableRemoved?.Invoke(Anchorables);
        }

        public void ClearAnchorables()
        {
            Anchorables = [];
        }

        #endregion

        #region Test Plan Function

        public void setTestPlan(TEST_PLAN testPlan)
        {
            TestPlan = testPlan;
            OnTestParameterUpdated?.Invoke(TestPlan.TEST_PARAMETERS);
        }

        public async Task CreateTestPlan(TEST_PLAN testPlan)
        {
            await _dbCreator.CreateTestPlan(testPlan);
            TestPlan = testPlan;
            OnTestParameterUpdated?.Invoke(TestPlan.TEST_PARAMETERS);
        }

        public async Task CreateTestParameter(TEST_PARAMETER testParameter)
        {
            await _dbCreator.CreateTestParameter(testParameter);
            OnTestParameterUpdated?.Invoke(TestPlan.TEST_PARAMETERS);
        }

        public async Task DeleteTestParameter(TEST_PARAMETER testParameter)
        {
            await _dbCreator.DeleteTestParameter(testParameter);
            OnTestParameterUpdated?.Invoke(TestPlan.TEST_PARAMETERS);
        }

        public async Task UpdateTestParameter(TEST_PARAMETER testParameter)
        {
            await _dbCreator.UpdateTestParameter(testParameter);
            OnTestParameterUpdated?.Invoke(TestPlan.TEST_PARAMETERS);
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

        public async Task DeleteDut(DUT dut)
        {
            await _dbCreator.DeleteDUT(dut);
            LoadDut();
            OnDutDeleted?.Invoke(DUTs);
        }
        #endregion
    }
}
