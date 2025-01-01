using CID_Tester.Model;
using CID_Tester.Service.DbCreator;

namespace CID_Tester.Service.DbProvider
{
    public interface IDbProvider
    {
        Task<IEnumerable<DUT>> GetAllDuts();
        Task<IEnumerable<TEST_PLAN>> GetAllTestPlans();
        Task<IEnumerable<TEST_PARAMETER>> GetAllTestParameters();
        Task<TEST_USER?> GetUser(string username, string password, IDbCreator dbCreator);
    }
}
