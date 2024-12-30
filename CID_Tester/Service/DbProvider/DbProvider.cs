using CID_Tester.DbContexts;
using CID_Tester.Model;
using CID_Tester.Service.DbCreator;
using Microsoft.EntityFrameworkCore;
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
                IEnumerable<DUT> dutDTOs = await context.DUT.ToListAsync();
                return dutDTOs.Select(r => new DUT(r.DutCode, r.DutName, r.Description));

            }
        }

        public Task<IEnumerable<TEST_PARAMETER>> GetAllTestParameters()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TEST_PLAN>> GetAllTestPlans()
        {
            throw new NotImplementedException();
        }

        public async Task<TEST_USER?> GetUser(string username, string password, IDbCreator dbCreator)
        {
            using(TesterDbContext context = _dbContextFactory.CreateDbContext())
            {
                TEST_USER? userDTO = await context.TEST_USER
                    .Where(r => r.Username == username && r.Password == password)
                    .Include(u => u.TEST_PLANS)
                        .ThenInclude(tp => tp.DUT)
                    .Include(u => u.TEST_PLANS)
                        .ThenInclude(tp => tp.TEST_PARAMETERS)
                    .FirstOrDefaultAsync();

                if (userDTO == null) return null;

                TEST_USER user = new TEST_USER(
                    userDTO.UserCode,
                    userDTO.FirstName,
                    userDTO.LastName,
                    userDTO.Email,
                    userDTO.ProfileImage,
                    userDTO.Username,
                    userDTO.Password
                )
                {
                    TEST_PLANS = userDTO.TEST_PLANS
                };

                Debug.WriteLine($"{user.TEST_PLANS.Count}");

                return user;
            }

        }
    }
}
