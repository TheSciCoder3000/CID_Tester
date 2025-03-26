using CID_Tester.Model;
using CID_Tester.Service.DbCreator;
using CID_Tester.Service.DbProvider;
using CID_Tester.Service.Serial;
using CID_Tester.ViewModel;
using System.Diagnostics;
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
            if (value == TestingMode.Start) _testPlanService?.Start(TestPlanStore.SelectedTestPlan, () => Testing = TestingMode.Stop);
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
    public DocumentStore DocumentStore { get; }

    #region Events

    public event Action<IEnumerable<DUT>>? OnDutCreated;
    public event Action<IEnumerable<DUT>>? OnDutUpdated;
    public event Action<IEnumerable<DUT>>? OnDutDeleted;

    public event Action<TEST_USER>? OnTestUserUpdated;

    public event Action<IEnumerable<TEST_PARAMETER>>? OnTestParameterUpdated;

    

    public event Action<TestingMode>? OnTesting;

    #endregion

    public AppStore(IDbProvider dbProvider, IDbCreator dbCreator, TEST_USER testUser, IEnumerable<BaseViewModel> documents)
    {
        _dbProvider = dbProvider;
        _dbCreator = dbCreator;
        TestUser = testUser;

        _testPlanService = new TestPlanService(testUser, dbCreator);
        DocumentStore = new DocumentStore(documents);
        TestPlanStore = new TestPlanStore(dbProvider, dbCreator);

        LoadDut();

    }


    public bool canTest
    {
        get => (TestPlanStore.SelectedTestPlan != null || TestPlanStore.SelectedTestPlan?.TEST_PARAMETERS.Count > 0);
    }
    public void ReinitializeTestDevices()
    {
        int unconnectedDevices = _testPlanService.Initialize();
        if (unconnectedDevices == 0)
        {
            MessageBox.Show("All devices are connected", "Device Connection", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        else
        {
            MessageBox.Show($"{unconnectedDevices} devices are not connected", "Device Connection", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }

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
