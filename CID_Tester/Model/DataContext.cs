using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CID_Tester.Model
{
    public class DataContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            DotNetEnv.Env.TraversePath().Load();
            String data_source = DotNetEnv.Env.GetString("DATA_SOURCE");
            String database_name = DotNetEnv.Env.GetString("DATABASE");
            optionsBuilder.UseSqlServer($"Data Source ={data_source};Initial Catalog={database_name};Integrated Security=True;Encrypt=False;Trust Server Certificate=False");

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TEST_PARAMETER>()
                .HasKey(tp => tp.PARAM_CODE); // Explicitly define the primary key
            modelBuilder.Entity<DUT>()
                .HasKey(tp => tp.DUT_CODE);
            modelBuilder.Entity<TEST_USER>()
                .HasKey(tp => tp.USER_CODE);
            modelBuilder.Entity<TEST_PROCEDURE>()
                .HasKey(tp => tp.TEST_CODE);
        }

        public DbSet<DUT> DUT { get; set; }
        public DbSet<TEST_PARAMETER> TEST_PARAMETER { get; set; }
        public DbSet<TEST_USER> TEST_USER { get; set; }
        public DbSet<TEST_PROCEDURE> TEST_PROCEDURE { get; set; }


    }
}
