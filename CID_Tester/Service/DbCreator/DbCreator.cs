using CID_Tester.DbContexts;
using CID_Tester.DbContexts.DTO;
using CID_Tester.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CID_Tester.Service.DbCreator
{
    public class DbCreator : IDbCreator
    {
        private readonly TesterDbContextFactory _dbContextFactory;

        public DbCreator(TesterDbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task CreateDUT(DUT dut)
        {
            using (TesterDbContext context  = _dbContextFactory.CreateDbContext())
            {
                DutDTO dutDTO = new DutDTO()
                {
                    DUT_CODE = dut.DUT_CODE,
                    DESCRIPTION = dut.DESCRIPTION,
                };

                context.DUT.Add(dutDTO);
                await context.SaveChangesAsync();
            }
        }

        public Task CreateTestParameter(TEST_PARAMETER param)
        {
            throw new NotImplementedException();
        }

        public Task CreateTestPlan(TEST_PROCEDURE testPlan)
        {
            throw new NotImplementedException();
        }

        public Task CreateUser(TEST_USER user)
        {
            throw new NotImplementedException();
        }
    }
}
