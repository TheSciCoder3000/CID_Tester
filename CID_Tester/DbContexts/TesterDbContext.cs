using CID_Tester.DbContexts.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CID_Tester.DbContexts
{
    public class TesterDbContext : DbContext
    {
        public TesterDbContext(DbContextOptions options) : base(options) { }
        public DbSet<UserDTO> TEST_USER { get; set; }
        public DbSet<TestPlanDTO> TEST_PLAN { get; set; }
        public DbSet<ParameterDTO> TEST_PARAMETER { get; set; }
        public DbSet<DutDTO> DUT { get; set; }
    }
}
