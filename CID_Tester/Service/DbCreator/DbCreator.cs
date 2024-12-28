using CID_Tester.DbContexts;
using CID_Tester.Model;
using CID_Tester.Model.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CID_Tester.Service.DbCreator
{
    public class DbCreator : IDbCreator
    {
        private readonly TesterDbContextFactory _dbContextFactory;

        public DbCreator(TesterDbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task CreateDUT(string description)
        {
            using (TesterDbContext context  = _dbContextFactory.CreateDbContext())
            {
                DutDTO dutDTO = new DutDTO()
                {
                    DESCRIPTION = description
                };

                context.DUT.Add(dutDTO);
                await context.SaveChangesAsync();
            }
        }

        public Task CreateTestParameter(TEST_PARAMETER param)
        {
            throw new NotImplementedException();
        }

        public async Task CreateTestPlan(TEST_PROCEDURE testPlan, TEST_USER user)
        {
            using (TesterDbContext context = _dbContextFactory.CreateDbContext())
            {
                UserDTO userDTO = user.ToDTO();
                DutDTO dutDTO = testPlan.DUT.ToDTO();
                context.Attach(userDTO);
                context.Attach(dutDTO);
                TestPlanDTO testPlanDTO = new TestPlanDTO()
                {
                    DATE = DateTime.Now,
                    CYCLE_NO = 3,
                    TEST_TIME = 0,
                    TEST_USER = userDTO,
                    DUT = dutDTO
                };
                context.TEST_PLAN.Add(testPlanDTO);
                await context.SaveChangesAsync();
            }
        }

        public Task CreateUser(TEST_USER user)
        {
            throw new NotImplementedException();
        }
    }
}
