﻿// <auto-generated />
using System;
using CID_Tester.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CID_Tester.Migrations
{
    [DbContext(typeof(TesterDbContext))]
    partial class TesterDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.0");

            modelBuilder.Entity("CID_Tester.DbContexts.DTO.DutDTO", b =>
                {
                    b.Property<int>("DUT_CODE")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("DESCRIPTION")
                        .HasColumnType("TEXT");

                    b.HasKey("DUT_CODE");

                    b.ToTable("DUT");
                });

            modelBuilder.Entity("CID_Tester.DbContexts.DTO.ParameterDTO", b =>
                {
                    b.Property<int>("PARAM_CODE")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("DESCRIPTION")
                        .HasColumnType("TEXT");

                    b.Property<string>("METRIC")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("PASS")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("TARGET")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("VALUE")
                        .HasColumnType("TEXT");

                    b.HasKey("PARAM_CODE");

                    b.ToTable("TEST_PARAMETER");
                });

            modelBuilder.Entity("CID_Tester.DbContexts.DTO.TestPlanDTO", b =>
                {
                    b.Property<int>("TEST_CODE")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CYCLE_NO")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DATE")
                        .HasColumnType("TEXT");

                    b.Property<int>("DUT_CODE")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TEST_TIME")
                        .HasColumnType("INTEGER");

                    b.Property<int>("USER_CODE")
                        .HasColumnType("INTEGER");

                    b.HasKey("TEST_CODE");

                    b.HasIndex("DUT_CODE");

                    b.HasIndex("USER_CODE");

                    b.ToTable("TEST_PLAN");
                });

            modelBuilder.Entity("CID_Tester.DbContexts.DTO.UserDTO", b =>
                {
                    b.Property<int>("USER_CODE")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("EMAIL")
                        .IsRequired()
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("FIRST_NAME")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("LAST_NAME")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("PASSWORD")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("PROFILE_IMAGE")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("USER_NAME")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("USER_CODE");

                    b.ToTable("TEST_USER");
                });

            modelBuilder.Entity("CID_Tester.DbContexts.DTO.ParameterDTO", b =>
                {
                    b.HasOne("CID_Tester.DbContexts.DTO.TestPlanDTO", "TEST_PLAN")
                        .WithMany("TEST_PARAMETERS")
                        .HasForeignKey("PARAM_CODE")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TEST_PLAN");
                });

            modelBuilder.Entity("CID_Tester.DbContexts.DTO.TestPlanDTO", b =>
                {
                    b.HasOne("CID_Tester.DbContexts.DTO.DutDTO", "DUT")
                        .WithMany("TEST_PLANS")
                        .HasForeignKey("DUT_CODE")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CID_Tester.DbContexts.DTO.UserDTO", "TEST_USER")
                        .WithMany("TEST_PLANS")
                        .HasForeignKey("USER_CODE")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DUT");

                    b.Navigation("TEST_USER");
                });

            modelBuilder.Entity("CID_Tester.DbContexts.DTO.DutDTO", b =>
                {
                    b.Navigation("TEST_PLANS");
                });

            modelBuilder.Entity("CID_Tester.DbContexts.DTO.TestPlanDTO", b =>
                {
                    b.Navigation("TEST_PARAMETERS");
                });

            modelBuilder.Entity("CID_Tester.DbContexts.DTO.UserDTO", b =>
                {
                    b.Navigation("TEST_PLANS");
                });
#pragma warning restore 612, 618
        }
    }
}
