using CID_Tester.Exceptions;
using CID_Tester.Model;
using CID_Tester.Model.DbContexts;
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
                return await context.DUT.ToListAsync();

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
                var user = await context.TEST_USER
                    .Where(r => r.Username == username)
                    .Select(u => new { u.UserCode, u.Username, u.Password })
                    .FirstOrDefaultAsync();

                if (user == null) return null;

                if (!BCrypt.Net.BCrypt.Verify(password, user.Password)) throw new IncorrectLoginException();

                TEST_USER? verifiedUser = await context.TEST_USER
                    .Where(r => r.UserCode == user.UserCode)
                    .Include(u => u.TEST_PLANS)
                        .ThenInclude(tp => tp.DUT)
                    .Include(u => u.TEST_PLANS)
                        .ThenInclude(tp => tp.TEST_PARAMETERS)
                    .FirstOrDefaultAsync();

                return verifiedUser;
            }

        }
    }
}
