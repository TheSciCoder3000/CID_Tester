using Microsoft.EntityFrameworkCore;

namespace CID_Tester.DbContexts
{
    public class TesterDbContextFactory
    {
        private readonly string _connectionString;

        public TesterDbContextFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public TesterDbContext CreateDbContext()
        {
            DbContextOptions options = new DbContextOptionsBuilder().UseSqlite(_connectionString).Options;
            return new TesterDbContext(options);
        }
    }
}
