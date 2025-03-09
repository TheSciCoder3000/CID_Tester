using CID_Tester.Exceptions;
using CID_Tester.Service.DbCreator;
using CID_Tester.Service.DbProvider;
using CID_Tester.ViewModel;

namespace CID_Tester.Model
{
    public enum TestingMode
    {
        Start,
        Pause,
        Stop
    }

    public class Store
    {
        private readonly IDbProvider _dbProvider;
        private readonly IDbCreator _dbCreator;

        private TestingMode _testing = TestingMode.Stop;
        public TestingMode Testing
        {
            get => _testing;
            set
            {
                _testing = value;
                OnTesting?.Invoke(value);
                if (value == TestingMode.Start) TestPlan?.Start(() => Testing = TestingMode.Stop);
            }
        }
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

        public event Action<IEnumerable<DUT>>? OnDutCreated;
        public event Action<IEnumerable<DUT>>? OnDutUpdated;
        public event Action<IEnumerable<DUT>>? OnDutDeleted;

        public event Action<TEST_USER>? OnTestUserUpdated;

        public event Action<TEST_PLAN?>? OnTestPlanUpdated;
        public event Action<IEnumerable<TEST_PARAMETER>>? OnTestParameterUpdated;

        public event Action<IEnumerable<BaseViewModel>>? OnDocumentOpenned;
        public event Action<IEnumerable<BaseViewModel>>? OnDocumentClosed;
        public event Action<BaseViewModel>? OnActiveDocumentChanged;
        public event Action<IEnumerable<BaseViewModel>>? OnAnchorableAdded;
        public event Action<IEnumerable<BaseViewModel>>? OnAnchorableRemoved;

        public event Action<TestingMode>? OnTesting;

        #endregion

        public Store(IDbProvider dbProvider, IDbCreator dbCreator, TEST_USER testUser, IEnumerable<BaseViewModel> documents)
        {
            _dbProvider = dbProvider;
            _dbCreator = dbCreator;
            TestUser = testUser;
            Documents = documents;

            LoadDut();
        }

        #region Testing Functions
        public bool canTest
        {
            get => (TestPlan != null || TestPlan?.TEST_PARAMETERS.Count > 0);
        }
        #endregion

        #region Document Functions

        public void setActiveDocument(BaseViewModel document)
        {
            ActiveDocument = document;
            OnActiveDocumentChanged?.Invoke(document);
        }

        public void AddDocument<T>(BaseViewModel document)
        {
            ICollection<BaseViewModel> DocumentCollection = Documents.ToList();
            BaseViewModel? activeDocument = DocumentCollection.FirstOrDefault(d => d is T);

            if ( activeDocument == null)
            {
                DocumentCollection.Add(document);
                Documents = DocumentCollection;
                OnDocumentOpenned?.Invoke(Documents);
                setActiveDocument(document);
                ClearAnchorables();
            }
            if (ActiveDocument != activeDocument && activeDocument != null)
            {
                setActiveDocument(activeDocument);
                ClearAnchorables();
            }
        }

        public void RemoveDocument(BaseViewModel document)
        {
            ICollection<BaseViewModel> DocumentCollection = Documents.ToList();
            DocumentCollection.Remove(document);
            Documents = DocumentCollection;
            OnDocumentClosed?.Invoke(Documents);
            ClearAnchorables();
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
            OnAnchorableRemoved?.Invoke(Anchorables);
        }

        #endregion

        #region Test Plan Function

        public void setTestPlan(TEST_PLAN testPlan)
        {
            TestPlan = testPlan;
            OnTestPlanUpdated?.Invoke(TestPlan);
            OnTestParameterUpdated?.Invoke(TestPlan.TEST_PARAMETERS);
        }

        public async Task CreateTestPlan(TEST_PLAN testPlan)
        {
            await _dbCreator.CreateTestPlan(testPlan);
            TestPlan = testPlan;
            OnTestPlanUpdated?.Invoke(TestPlan);
            OnTestParameterUpdated?.Invoke(TestPlan.TEST_PARAMETERS);
        }

        public async Task CreateTestParameter(TEST_PARAMETER testParameter)
        {
            if (TestPlan == null) throw new TestParameterException("Cannot CREATE test parameter if test plan is null");
            await _dbCreator.CreateTestParameter(testParameter);
            OnTestParameterUpdated?.Invoke(TestPlan.TEST_PARAMETERS);
        }

        public async Task DeleteTestParameter(TEST_PARAMETER testParameter)
        {
            if (TestPlan == null) throw new TestParameterException("Cannot DELETE test parameter if test plan is null");
            await _dbCreator.DeleteTestParameter(testParameter);
            OnTestParameterUpdated?.Invoke(TestPlan.TEST_PARAMETERS);
        }

        public async Task UpdateTestParameter(TEST_PARAMETER testParameter)
        {
            if (TestPlan == null) throw new TestParameterException("Cannot UPDATE test parameter if test plan is null");
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
