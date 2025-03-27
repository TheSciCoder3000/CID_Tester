using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CID_Tester.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DUT",
                columns: table => new
                {
                    DutCode = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DutName = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    NumberOfOpAmps = table.Column<int>(type: "INTEGER", nullable: false),
                    PartNumber = table.Column<string>(type: "TEXT", nullable: true),
                    PackageType = table.Column<string>(type: "TEXT", nullable: true),
                    ManufacturerNumber = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DUT", x => x.DutCode);
                });

            migrationBuilder.CreateTable(
                name: "TEST_USER",
                columns: table => new
                {
                    UserCode = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(150)", nullable: false),
                    ProfileImage = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TEST_USER", x => x.UserCode);
                });

            migrationBuilder.CreateTable(
                name: "TEST_PLAN",
                columns: table => new
                {
                    TestCode = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    DutCode = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TEST_PLAN", x => x.TestCode);
                    table.ForeignKey(
                        name: "FK_TEST_PLAN_DUT_DutCode",
                        column: x => x.DutCode,
                        principalTable: "DUT",
                        principalColumn: "DutCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TEST_BATCH",
                columns: table => new
                {
                    BatchCode = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CycleNo = table.Column<int>(type: "INTEGER", nullable: false),
                    TestTime = table.Column<int>(type: "INTEGER", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TestCode = table.Column<int>(type: "INTEGER", nullable: false),
                    UserCode = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TEST_BATCH", x => x.BatchCode);
                    table.ForeignKey(
                        name: "FK_TEST_BATCH_TEST_PLAN_TestCode",
                        column: x => x.TestCode,
                        principalTable: "TEST_PLAN",
                        principalColumn: "TestCode",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TEST_BATCH_TEST_USER_UserCode",
                        column: x => x.UserCode,
                        principalTable: "TEST_USER",
                        principalColumn: "UserCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TEST_PARAMETER",
                columns: table => new
                {
                    ParamCode = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Type = table.Column<string>(type: "TEXT", nullable: false, defaultValue: "DC"),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Metric = table.Column<string>(type: "TEXT", nullable: false),
                    Target = table.Column<decimal>(type: "TEXT", nullable: false),
                    Pass = table.Column<bool>(type: "INTEGER", nullable: true),
                    Parameters = table.Column<string>(type: "TEXT", nullable: false),
                    InputConfiguration = table.Column<string>(type: "TEXT", nullable: false),
                    TestCode = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TEST_PARAMETER", x => x.ParamCode);
                    table.ForeignKey(
                        name: "FK_TEST_PARAMETER_TEST_PLAN_TestCode",
                        column: x => x.TestCode,
                        principalTable: "TEST_PLAN",
                        principalColumn: "TestCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TEST_OUTPUT",
                columns: table => new
                {
                    OutputCode = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Measured = table.Column<string>(type: "TEXT", nullable: false),
                    DutLocation = table.Column<int>(type: "INTEGER", nullable: false),
                    ParamCode = table.Column<int>(type: "INTEGER", nullable: false),
                    BatchCode = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TEST_OUTPUT", x => x.OutputCode);
                    table.ForeignKey(
                        name: "FK_TEST_OUTPUT_TEST_BATCH_BatchCode",
                        column: x => x.BatchCode,
                        principalTable: "TEST_BATCH",
                        principalColumn: "BatchCode",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TEST_OUTPUT_TEST_PARAMETER_ParamCode",
                        column: x => x.ParamCode,
                        principalTable: "TEST_PARAMETER",
                        principalColumn: "ParamCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "TEST_USER",
                columns: new[] { "UserCode", "Email", "FirstName", "LastName", "Password", "ProfileImage", "Username" },
                values: new object[] { 1, "drjjdevilla2002@gmail.com", "John Juvi", "De Villa", "$2a$11$qoIl2jzkPJaUSAwzsv6QberbuzQ/khrBVqRjLN7j/Fi4kOgJIMRHK", "", "neurocoder" });

            migrationBuilder.CreateIndex(
                name: "IX_TEST_BATCH_TestCode",
                table: "TEST_BATCH",
                column: "TestCode");

            migrationBuilder.CreateIndex(
                name: "IX_TEST_BATCH_UserCode",
                table: "TEST_BATCH",
                column: "UserCode");

            migrationBuilder.CreateIndex(
                name: "IX_TEST_OUTPUT_BatchCode",
                table: "TEST_OUTPUT",
                column: "BatchCode");

            migrationBuilder.CreateIndex(
                name: "IX_TEST_OUTPUT_ParamCode",
                table: "TEST_OUTPUT",
                column: "ParamCode");

            migrationBuilder.CreateIndex(
                name: "IX_TEST_PARAMETER_TestCode",
                table: "TEST_PARAMETER",
                column: "TestCode");

            migrationBuilder.CreateIndex(
                name: "IX_TEST_PLAN_DutCode",
                table: "TEST_PLAN",
                column: "DutCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TEST_OUTPUT");

            migrationBuilder.DropTable(
                name: "TEST_BATCH");

            migrationBuilder.DropTable(
                name: "TEST_PARAMETER");

            migrationBuilder.DropTable(
                name: "TEST_USER");

            migrationBuilder.DropTable(
                name: "TEST_PLAN");

            migrationBuilder.DropTable(
                name: "DUT");
        }
    }
}
