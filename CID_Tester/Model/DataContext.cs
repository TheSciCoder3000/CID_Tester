using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CID_Tester.Model
{
    internal class DataContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source =localhost;Initial Catalog=master;Integrated Security=True;Encrypt=False;Trust Server Certificate=False");

        }

        public DbSet<DUT> DUT { get; set; }
    }
}
