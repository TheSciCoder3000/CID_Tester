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
        Task CreateDUT(DUT dut);
        Task CreateTestPlan(TEST_PLAN testPlan);
        Task CreateTestParameter(TEST_PARAMETER param);
    }
}
