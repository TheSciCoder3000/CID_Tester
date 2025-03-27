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

        #region DUT
        public async Task CreateDUT(DUT dut)
        {
            using (TesterDbContext context = _dbContextFactory.CreateDbContext())
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

        #endregion

        #region Test Parameter
        public async Task CreateTestParameter(TEST_PARAMETER param)
        {
            using (TesterDbContext context = _dbContextFactory.CreateDbContext())
            {
                context.Entry(param.TEST_PLAN).State = EntityState.Unchanged;

                context.TEST_PARAMETER.Add(param);

                await context.SaveChangesAsync();
            }
        }

        public async Task UpdateTestParameter(TEST_PARAMETER param)
        {
            using (TesterDbContext context = _dbContextFactory.CreateDbContext())
            {
                context.Entry(param).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteTestParameter(TEST_PARAMETER param)
        {
            using (TesterDbContext context = _dbContextFactory.CreateDbContext())
            {
                context.Entry(param.TEST_PLAN).State = EntityState.Unchanged;

                context.TEST_PARAMETER.Remove(param);

                await context.SaveChangesAsync();
            }
        }

        #endregion


        public async Task CreateTestPlan(TEST_PLAN testPlan)
        {
            using (TesterDbContext context = _dbContextFactory.CreateDbContext())
            {
                context.Entry(testPlan.DUT).State = EntityState.Unchanged;

                context.TEST_PLAN.Add(testPlan);

                await context.SaveChangesAsync();
            }
        }
        

        public async Task CreateUser(TEST_USER user)
        {
            using (TesterDbContext context = _dbContextFactory.CreateDbContext())
            {
                context.TEST_USER.Add(user);
                await context.SaveChangesAsync();
            }
        }


        public async Task CreateTestOutput(TEST_OUTPUT output)
        {
            using (TesterDbContext context = _dbContextFactory.CreateDbContext())
            {
                context.Entry(output.TEST_BATCH).State = EntityState.Unchanged;
                context.Entry(output.TEST_PARAMETER).State = EntityState.Unchanged;
                context.TEST_OUTPUT.Add(output);
                await context.SaveChangesAsync();
            }
        }

        public async Task UpdateTestOutput(TEST_OUTPUT output)
        {
            using (TesterDbContext context = _dbContextFactory.CreateDbContext())
            {
                context.Entry(output).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }
        }

        public async Task CreateBatch(TEST_BATCH batch)
        {
            using (TesterDbContext context = _dbContextFactory.CreateDbContext())
            {
                context.Entry(batch.TEST_PLAN.DUT).State = EntityState.Unchanged;
                context.Entry(batch.TEST_PLAN).State = EntityState.Unchanged;
                context.Entry(batch.TEST_USER).State = EntityState.Unchanged;

                foreach (TEST_OUTPUT testOutput in batch.TEST_OUTPUTS)
                {
                    context.Entry(testOutput.TEST_PARAMETER).State = EntityState.Unchanged;
                    context.Entry(testOutput).State = EntityState.Added;
                }

                context.TEST_BATCH.Add(batch);
                await context.SaveChangesAsync();
            }
        }
    }
}
