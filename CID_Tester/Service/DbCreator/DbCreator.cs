using CID_Tester.Model;
using CID_Tester.Model.DbContexts;
using Microsoft.EntityFrameworkCore;

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

                context.DUT.Add(dut);
                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteDUT(DUT dut)
        {
            using (TesterDbContext context = _dbContextFactory.CreateDbContext())
            {
                context.DUT.Remove(dut);
                await context.SaveChangesAsync();
            }
        }

        public async Task CreateTestParameter(TEST_PARAMETER param)
        {
            using (TesterDbContext context = _dbContextFactory.CreateDbContext())
            {
                context.Entry(param.TestPlan).State = EntityState.Unchanged;

                context.TEST_PARAMETER.Add(param);

                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteTestParameter(TEST_PARAMETER param)
        {
            using (TesterDbContext context = _dbContextFactory.CreateDbContext())
            {
                context.Entry(param.TestPlan).State = EntityState.Unchanged;

                context.TEST_PARAMETER.Remove(param);

                await context.SaveChangesAsync();
            }
        }

        public async Task CreateTestPlan(TEST_PLAN testPlan)
        {
            using (TesterDbContext context = _dbContextFactory.CreateDbContext())
            {
                context.Entry(testPlan.DUT).State = EntityState.Unchanged;
                context.Entry(testPlan.TEST_USER).State = EntityState.Unchanged;

                context.TEST_PLAN.Add(testPlan);
                
                await context.SaveChangesAsync();
            }
        }

        public Task CreateUser(TEST_USER user)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateTestParameter(TEST_PARAMETER param)
        {
            using (TesterDbContext context = _dbContextFactory.CreateDbContext())
            {
                context.Entry(param).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }
        }
    }
}
