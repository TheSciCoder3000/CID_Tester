using CID_Tester.Model;
using Microsoft.EntityFrameworkCore;

namespace CID_Tester.Model.DbContexts;

public class TesterDbContext : DbContext
{
    public TesterDbContext(DbContextOptions options) : base(options) { }
    public DbSet<TEST_USER> TEST_USER { get; set; }
    public DbSet<TEST_PLAN> TEST_PLAN { get; set; }
    public DbSet<TEST_PARAMETER> TEST_PARAMETER { get; set; }
    public DbSet<DUT> DUT { get; set; }
    public DbSet<TEST_OUTPUT> TEST_OUTPUT { get; set; }
    public DbSet<TEST_BATCH> TEST_BATCH { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TEST_USER>()
            .HasMany(e => e.TEST_BATCHES)
            .WithOne(e => e.TEST_USER)
            .HasForeignKey("UserCode")
            .IsRequired();

        modelBuilder.Entity<DUT>()
            .HasMany(e => e.TEST_PLANS)
            .WithOne(e => e.DUT)
            .HasForeignKey("DutCode")
            .IsRequired();

        modelBuilder.Entity<TEST_PLAN>()
            .HasMany(e => e.TEST_PARAMETERS)
            .WithOne(e => e.TEST_PLAN)
            .HasForeignKey("TestCode")
            .IsRequired();

        modelBuilder.Entity<TEST_PLAN>()
            .HasMany(e => e.TEST_BATCHES)
            .WithOne(e => e.TEST_PLAN)
            .HasForeignKey("TestCode")
            .IsRequired();

        modelBuilder.Entity<TEST_PARAMETER>()
            .HasMany(e => e.TEST_OUTPUTS)
            .WithOne(e => e.TEST_PARAMETER)
            .HasForeignKey("ParamCode")
            .IsRequired();

        modelBuilder.Entity<TEST_PARAMETER>()
            .Property(param => param.Type)
            .HasDefaultValue("DC");

        modelBuilder.Entity<TEST_BATCH>()
            .HasMany(e => e.TEST_OUTPUTS)
            .WithOne(e => e.TEST_BATCH)
            .HasForeignKey("BatchCode")
            .IsRequired();

        modelBuilder.Entity<TEST_USER>()
            .HasData(
                new TEST_USER()
                {
                    UserCode=1,
                    FirstName="John Juvi", 
                    LastName="De Villa", 
                    Email="drjjdevilla2002@gmail.com", 
                    ProfileImage="", 
                    Username="neurocoder", 
                    Password="$2a$11$qoIl2jzkPJaUSAwzsv6QberbuzQ/khrBVqRjLN7j/Fi4kOgJIMRHK"
                }
            );
    }
}
