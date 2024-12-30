using CID_Tester.Model;
using CID_Tester.Service.DbCreator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
