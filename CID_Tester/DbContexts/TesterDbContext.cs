using CID_Tester.Model;
using Microsoft.EntityFrameworkCore;

namespace CID_Tester.DbContexts
{
    public class TesterDbContext : DbContext
    {
        public TesterDbContext(DbContextOptions options) : base(options) { }
        public DbSet<TEST_USER> TEST_USER { get; set; }
        public DbSet<TEST_PLAN> TEST_PLAN { get; set; }
        public DbSet<TEST_PARAMETER> TEST_PARAMETER { get; set; }
        public DbSet<DUT> DUT { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TEST_USER>()
                .HasKey("UserCode");
            modelBuilder.Entity<TEST_USER>()
                .HasMany(e => e.TEST_PLANS)
                .WithOne(e => e.TEST_USER)
                .HasForeignKey("UserCode")
                .IsRequired();

            modelBuilder.Entity<DUT>()
                .HasKey("DutCode");
            modelBuilder.Entity<DUT>()
                .HasMany(e => e.TEST_PLANS)
                .WithOne(e => e.DUT)
                .HasForeignKey("DutCode")
                .IsRequired();

            modelBuilder.Entity<TEST_PLAN>()
                .HasKey("TestCode");
            modelBuilder.Entity<TEST_PLAN>()
                .HasMany(e => e.TEST_PARAMETERS)
                .WithOne(e => e.TestPlan)
                .HasForeignKey("TestCode")
                .IsRequired();

            modelBuilder.Entity<TEST_PARAMETER>()
                .HasKey("ParamCode");

            //modelBuilder.Entity<TEST_USER>()
            //    .HasData(
            //        new TEST_USER("John Juvi", "De Villa", "drjjdevilla2002@gmail.com", "", "neurocoder", "$2a$11$qoIl2jzkPJaUSAwzsv6QberbuzQ/khrBVqRjLN7j/Fi4kOgJIMRHK")
            //    );
        }
    }
}
