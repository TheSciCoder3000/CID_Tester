using CID_Tester.DbContexts;
using CID_Tester.Model;
using CID_Tester.Model.DTO;
using CID_Tester.Service.DbCreator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Diagnostics;

namespace CID_Tester.Service.DbProvider
{
    public class DbProvider : IDbProvider
    {
        private readonly TesterDbContextFactory _dbContextFactory;

        public DbProvider(TesterDbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task<IEnumerable<DUT>> GetAllDuts()
        {
            using (TesterDbContext context = _dbContextFactory.CreateDbContext())
            {
                IEnumerable<DutDTO> dutDTOs = await context.DUT.ToListAsync();
                return dutDTOs.Select(r => new DUT(r.DUT_CODE, r.DUT_NAME, r.DESCRIPTION));

            }
        }

        public Task<IEnumerable<TEST_PARAMETER>> GetAllTestParameters()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TEST_PROCEDURE>> GetAllTestPlans()
        {
            throw new NotImplementedException();
        }

        public async Task<TEST_USER?> GetUser(string username, string password, IDbCreator dbCreator)
        {
            using(TesterDbContext context = _dbContextFactory.CreateDbContext())
            {
                UserDTO? userDTO = await context.TEST_USER
                    .Where(r => r.USER_NAME == username && r.PASSWORD == password)
                    .Include(u => u.TEST_PLANS)
                        .ThenInclude(tp => tp.DUT)
                    .Include(u => u.TEST_PLANS)
                        .ThenInclude(tp => tp.TEST_PARAMETERS)
                    .FirstOrDefaultAsync();

                if (userDTO == null) return null;

                TEST_USER user = new TEST_USER(
                    dbCreator,
                    this, 
                    userDTO.USER_CODE,
                    userDTO.FIRST_NAME,
                    userDTO.LAST_NAME,
                    userDTO.EMAIL,
                    userDTO.PROFILE_IMAGE,
                    userDTO.USER_NAME,
                    userDTO.PASSWORD,
                    generateTestPlan(userDTO.TEST_PLANS)
                );

                Debug.WriteLine($"{user.TEST_PLANS.Count}");

                return user;
            }

        }
        public ICollection<TEST_PROCEDURE> generateTestPlan(ICollection<TestPlanDTO> testPlans)
        {
            return testPlans.Select(tp => 
                new TEST_PROCEDURE(
                    tp.TEST_CODE, 
                    tp.DATE, 
                    tp.TEST_USER.USER_CODE, 
                    tp.DUT.DUT_CODE, 
                    tp.CYCLE_NO,
                    tp.TEST_TIME,
                    convertToDUT(tp.DUT)
                )
            ).ToList();
        }

        public DUT convertToDUT(DutDTO dut)
        {
            return new DUT(dut.DUT_CODE, dut.DUT_NAME, dut.DESCRIPTION);
        }
    }
}
