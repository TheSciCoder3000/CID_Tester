using CID_Tester.Exceptions;
using CID_Tester.Model;
using CID_Tester.Service.DbCreator;
using CID_Tester.Service.DbProvider;
using CID_Tester.Service.Serial;
using CID_Tester.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CID_Tester.Store;

public enum TestingMode
{
    Start,
    Pause,
    Stop
}

public class AppStore
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
            if (value == TestingMode.Start) _testPlanService?.Start(() => Testing = TestingMode.Stop);
            if (value == TestingMode.Stop && _testPlanService?.TokenSource != null)
            {
                Debug.WriteLine("");
                _testPlanService?.TokenSource?.Cancel();
            }
        }
    }
    public IEnumerable<DUT> DUTs { get; private set; } = [];
    public TEST_USER TestUser { get; private set; }

    private TestPlanService _testPlanService;

    public TestPlanStore TestPlanStore { get; }
    public IEnumerable<BaseViewModel> Documents { get; private set; }
    public object ActiveDocument { get; set; } = null!;
    public IEnumerable<BaseViewModel> Anchorables { get; private set; } = [];

    #region Events

    public event Action<IEnumerable<DUT>>? OnDutCreated;
    public event Action<IEnumerable<DUT>>? OnDutUpdated;
    public event Action<IEnumerable<DUT>>? OnDutDeleted;

    public event Action<TEST_USER>? OnTestUserUpdated;

    public event Action<IEnumerable<TEST_PARAMETER>>? OnTestParameterUpdated;

    public event Action<IEnumerable<BaseViewModel>>? OnDocumentOpenned;
    public event Action<IEnumerable<BaseViewModel>>? OnDocumentClosed;
    public event Action<BaseViewModel>? OnActiveDocumentChanged;
    public event Action<IEnumerable<BaseViewModel>>? OnAnchorableAdded;
    public event Action<IEnumerable<BaseViewModel>>? OnAnchorableRemoved;

    public event Action<TestingMode>? OnTesting;

    #endregion

    public AppStore(IDbProvider dbProvider, IDbCreator dbCreator, TEST_USER testUser, IEnumerable<BaseViewModel> documents)
    {
        _dbProvider = dbProvider;
        _dbCreator = dbCreator;
        TestUser = testUser;
        Documents = documents;

        _testPlanService = new TestPlanService(dbCreator);
        TestPlanStore = new TestPlanStore(dbProvider, dbCreator);

        LoadDut();

    }

    #region Testing Functions
    public bool canTest
    {
        get => (TestPlanStore.SelectedTestPlan != null || TestPlanStore.SelectedTestPlan?.TEST_PARAMETERS.Count > 0);
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

        if (activeDocument == null)
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
    public void ReinitializeTestDevices()
    {
        int unconnectedDevices = _testPlanService.initialize();
        if (unconnectedDevices == 0)
        {
            MessageBox.Show("All devices are connected", "Device Connection", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        else
        {
            MessageBox.Show($"{unconnectedDevices} devices are not connected", "Device Connection", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
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
