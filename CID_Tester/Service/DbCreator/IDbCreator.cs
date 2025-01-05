using CID_Tester.Model;

namespace CID_Tester.Service.DbCreator
{
    public interface IDbCreator
    {
        Task CreateUser(TEST_USER user);
        Task CreateDUT(DUT dut);
        Task DeleteDUT(DUT dut);
        Task CreateTestPlan(TEST_PLAN testPlan);
        Task CreateTestParameter(TEST_PARAMETER param);
        Task DeleteTestParameter(TEST_PARAMETER param);
        Task UpdateTestParameter(TEST_PARAMETER param);

    }
}
