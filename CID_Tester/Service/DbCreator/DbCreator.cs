﻿using CID_Tester.DbContexts;
using CID_Tester.Model;
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

        public Task CreateTestParameter(TEST_PARAMETER param)
        {
            throw new NotImplementedException();
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
    }
}
