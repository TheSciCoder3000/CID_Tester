﻿using CID_Tester.Model.DTO;
using Microsoft.EntityFrameworkCore;

namespace CID_Tester.DbContexts
{
    public class TesterDbContext : DbContext
    {
        public TesterDbContext(DbContextOptions options) : base(options) { }
        public DbSet<UserDTO> TEST_USER { get; set; }
        public DbSet<TestPlanDTO> TEST_PLAN { get; set; }
        public DbSet<ParameterDTO> TEST_PARAMETER { get; set; }
        public DbSet<DutDTO> DUT { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserDTO>()
                .HasMany(e => e.TEST_PLANS)
                .WithOne(e => e.TEST_USER)
                .HasForeignKey("USER_CODE")
                .IsRequired();

            modelBuilder.Entity<DutDTO>()
                .HasMany(e => e.TEST_PLANS)
                .WithOne(e => e.DUT)
                .HasForeignKey("DUT_CODE")
                .IsRequired();

            modelBuilder.Entity<TestPlanDTO>()
                .HasMany(e => e.TEST_PARAMETERS)
                .WithOne(e => e.TEST_PLAN)
                .HasForeignKey("TEST_CODE")
                .IsRequired();
        }
    }
}
