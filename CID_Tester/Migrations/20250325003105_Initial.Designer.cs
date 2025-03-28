﻿// <auto-generated />
using System;
using CID_Tester.Model.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CID_Tester.Migrations
{
    [DbContext(typeof(TesterDbContext))]
    [Migration("20250325003105_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.2");

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

                    b.Property<string>("ManufacturerNumber")
                        .HasColumnType("TEXT");

                    b.Property<int>("NumberOfOpAmps")
                        .HasColumnType("INTEGER");

                    b.Property<string>("PackageType")
                        .HasColumnType("TEXT");

                    b.Property<string>("PartNumber")
                        .HasColumnType("TEXT");

                    b.HasKey("DutCode");

                    b.ToTable("DUT");
                });

            modelBuilder.Entity("CID_Tester.Model.TEST_BATCH", b =>
                {
                    b.Property<int>("BatchCode")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CycleNo")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Date")
                        .HasColumnType("TEXT");

                    b.Property<int>("TestCode")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TestTime")
                        .HasColumnType("INTEGER");

                    b.Property<int>("UserCode")
                        .HasColumnType("INTEGER");

                    b.HasKey("BatchCode");

                    b.HasIndex("TestCode");

                    b.HasIndex("UserCode");

                    b.ToTable("TEST_BATCH");
                });

            modelBuilder.Entity("CID_Tester.Model.TEST_OUTPUT", b =>
                {
                    b.Property<int>("OutputCode")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("BatchCode")
                        .HasColumnType("INTEGER");

                    b.Property<int>("DutLocation")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Measured")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("ParamCode")
                        .HasColumnType("INTEGER");

                    b.HasKey("OutputCode");

                    b.HasIndex("BatchCode");

                    b.HasIndex("ParamCode");

                    b.ToTable("TEST_OUTPUT");
                });

            modelBuilder.Entity("CID_Tester.Model.TEST_PARAMETER", b =>
                {
                    b.Property<int>("ParamCode")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("InputConfiguration")
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

                    b.HasKey("ParamCode");

                    b.HasIndex("TestCode");

                    b.ToTable("TEST_PARAMETER");
                });

            modelBuilder.Entity("CID_Tester.Model.TEST_PLAN", b =>
                {
                    b.Property<int>("TestCode")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<int>("DutCode")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("TestCode");

                    b.HasIndex("DutCode");

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

            modelBuilder.Entity("CID_Tester.Model.TEST_BATCH", b =>
                {
                    b.HasOne("CID_Tester.Model.TEST_PLAN", "TEST_PLAN")
                        .WithMany("TEST_BATCHES")
                        .HasForeignKey("TestCode")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CID_Tester.Model.TEST_USER", "TEST_USER")
                        .WithMany("TEST_BATCHES")
                        .HasForeignKey("UserCode")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TEST_PLAN");

                    b.Navigation("TEST_USER");
                });

            modelBuilder.Entity("CID_Tester.Model.TEST_OUTPUT", b =>
                {
                    b.HasOne("CID_Tester.Model.TEST_BATCH", "TEST_BATCH")
                        .WithMany("TEST_OUTPUTS")
                        .HasForeignKey("BatchCode")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CID_Tester.Model.TEST_PARAMETER", "TEST_PARAMETER")
                        .WithMany("TEST_OUTPUTS")
                        .HasForeignKey("ParamCode")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TEST_BATCH");

                    b.Navigation("TEST_PARAMETER");
                });

            modelBuilder.Entity("CID_Tester.Model.TEST_PARAMETER", b =>
                {
                    b.HasOne("CID_Tester.Model.TEST_PLAN", "TEST_PLAN")
                        .WithMany("TEST_PARAMETERS")
                        .HasForeignKey("TestCode")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TEST_PLAN");
                });

            modelBuilder.Entity("CID_Tester.Model.TEST_PLAN", b =>
                {
                    b.HasOne("CID_Tester.Model.DUT", "DUT")
                        .WithMany("TEST_PLANS")
                        .HasForeignKey("DutCode")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DUT");
                });

            modelBuilder.Entity("CID_Tester.Model.DUT", b =>
                {
                    b.Navigation("TEST_PLANS");
                });

            modelBuilder.Entity("CID_Tester.Model.TEST_BATCH", b =>
                {
                    b.Navigation("TEST_OUTPUTS");
                });

            modelBuilder.Entity("CID_Tester.Model.TEST_PARAMETER", b =>
                {
                    b.Navigation("TEST_OUTPUTS");
                });

            modelBuilder.Entity("CID_Tester.Model.TEST_PLAN", b =>
                {
                    b.Navigation("TEST_BATCHES");

                    b.Navigation("TEST_PARAMETERS");
                });

            modelBuilder.Entity("CID_Tester.Model.TEST_USER", b =>
                {
                    b.Navigation("TEST_BATCHES");
                });
#pragma warning restore 612, 618
        }
    }
}
