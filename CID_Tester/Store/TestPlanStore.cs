using CID_Tester.Exceptions;
using CID_Tester.Model;
using CID_Tester.Service.DbCreator;
using CID_Tester.Service.DbProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CID_Tester.Store;

public class TestPlanStore
{
    private readonly IDbProvider _dbProvider;
    private readonly IDbCreator _dbCreator;

    #region Events
    public event Action<TEST_PLAN?>? OnTestPlanUpdated;
    public event Action<IEnumerable<TEST_PARAMETER>>? OnTestParameterUpdated;

    #endregion

    public ICollection<TEST_PLAN> TestPlans { get; private set; } = [];
    public TEST_PLAN? SelectedTestPlan { get; private set; }


    public async void RefreshTestPlans()
    {
        TestPlans = (ICollection<TEST_PLAN>)await _dbProvider.GetAllTestPlans();
    }

    public void SelectTestPlan(TEST_PLAN testPlan)
    {
        SelectedTestPlan = testPlan;
        OnTestPlanUpdated?.Invoke(SelectedTestPlan);
        OnTestParameterUpdated?.Invoke(SelectedTestPlan.TEST_PARAMETERS);
    }

    public async Task CreateTestPlan(TEST_PLAN testPlan)
    {
        await _dbCreator.CreateTestPlan(testPlan);
        SelectedTestPlan = testPlan;
        OnTestPlanUpdated?.Invoke(SelectedTestPlan);
        OnTestParameterUpdated?.Invoke(SelectedTestPlan.TEST_PARAMETERS);
    }

    public TestPlanStore(IDbProvider dbProvider, IDbCreator dbCreator)
    {
        _dbProvider = dbProvider;
        _dbCreator = dbCreator;

        RefreshTestPlans();
    }

    public async Task CreateTestParameter(TEST_PARAMETER testParameter)
    {
        if (SelectedTestPlan == null) throw new TestParameterException("Cannot CREATE test parameter if test plan is null");
        await _dbCreator.CreateTestParameter(testParameter);
        OnTestParameterUpdated?.Invoke(SelectedTestPlan.TEST_PARAMETERS);
    }

    public async Task DeleteTestParameter(TEST_PARAMETER testParameter)
    {
        if (SelectedTestPlan == null) throw new TestParameterException("Cannot DELETE test parameter if test plan is null");
        await _dbCreator.DeleteTestParameter(testParameter);
        OnTestParameterUpdated?.Invoke(SelectedTestPlan.TEST_PARAMETERS);
    }

    public async Task UpdateTestParameter(TEST_PARAMETER testParameter)
    {
        if (SelectedTestPlan == null) throw new TestParameterException("Cannot UPDATE test parameter if test plan is null");
        await _dbCreator.UpdateTestParameter(testParameter);
        OnTestParameterUpdated?.Invoke(SelectedTestPlan.TEST_PARAMETERS);
    }
}