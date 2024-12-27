using CID_Tester.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CID_Tester.Service.DbCreator
{
    public interface IDbCreator
    {
        Task CreateUser(TEST_USER user);
        Task CreateDUT(string description);
        Task CreateTestPlan(TEST_PROCEDURE testPlan, TEST_USER USER);
        Task CreateTestParameter(TEST_PARAMETER param);
    }
}
