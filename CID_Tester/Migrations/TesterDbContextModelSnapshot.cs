﻿// <auto-generated />
using System;
using CID_Tester.Model.DbContexts;
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

            modelBuilder.Entity("CID_Tester.Model.DUT", b =>
                {
                    b.Property<int>("DutCode")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("DutName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("DutCode");

                    b.ToTable("DUT");
                });

            modelBuilder.Entity("CID_Tester.Model.TEST_PARAMETER", b =>
                {
                    b.Property<int>("ParamCode")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Metric")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Parameters")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool?>("Pass")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Target")
                        .HasColumnType("TEXT");

                    b.Property<int>("TestCode")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Type")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasDefaultValue("DC");

                    b.Property<decimal?>("Value")
                        .HasColumnType("TEXT");

                    b.HasKey("ParamCode");

                    b.HasIndex("TestCode");

                    b.ToTable("TEST_PARAMETER");
                });

            modelBuilder.Entity("CID_Tester.Model.TEST_PLAN", b =>
                {
                    b.Property<int>("TestCode")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CycleNo")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Date")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("DutCode")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("TestTime")
                        .HasColumnType("INTEGER");

                    b.Property<int>("UserCode")
                        .HasColumnType("INTEGER");

                    b.HasKey("TestCode");

                    b.HasIndex("DutCode");

                    b.HasIndex("UserCode");

                    b.ToTable("TEST_PLAN");
                });

            modelBuilder.Entity("CID_Tester.Model.TEST_USER", b =>
                {
                    b.Property<int>("UserCode")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("ProfileImage")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("UserCode");

                    b.ToTable("TEST_USER");

                    b.HasData(
                        new
                        {
                            UserCode = 1,
                            Email = "drjjdevilla2002@gmail.com",
                            FirstName = "John Juvi",
                            LastName = "De Villa",
                            Password = "$2a$11$qoIl2jzkPJaUSAwzsv6QberbuzQ/khrBVqRjLN7j/Fi4kOgJIMRHK",
                            ProfileImage = "",
                            Username = "neurocoder"
                        });
                });

            modelBuilder.Entity("CID_Tester.Model.TEST_PARAMETER", b =>
                {
                    b.HasOne("CID_Tester.Model.TEST_PLAN", "TestPlan")
                        .WithMany("TEST_PARAMETERS")
                        .HasForeignKey("TestCode")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TestPlan");
                });

            modelBuilder.Entity("CID_Tester.Model.TEST_PLAN", b =>
                {
                    b.HasOne("CID_Tester.Model.DUT", "DUT")
                        .WithMany("TEST_PLANS")
                        .HasForeignKey("DutCode")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CID_Tester.Model.TEST_USER", "TEST_USER")
                        .WithMany("TEST_PLANS")
                        .HasForeignKey("UserCode")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DUT");

                    b.Navigation("TEST_USER");
                });

            modelBuilder.Entity("CID_Tester.Model.DUT", b =>
                {
                    b.Navigation("TEST_PLANS");
                });

            modelBuilder.Entity("CID_Tester.Model.TEST_PLAN", b =>
                {
                    b.Navigation("TEST_PARAMETERS");
                });

            modelBuilder.Entity("CID_Tester.Model.TEST_USER", b =>
                {
                    b.Navigation("TEST_PLANS");
                });
#pragma warning restore 612, 618
        }
    }
}
