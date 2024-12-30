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
                    Description = table.Column<string>(type: "TEXT", nullable: false)
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
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CycleNo = table.Column<int>(type: "INTEGER", nullable: false),
                    TestTime = table.Column<int>(type: "INTEGER", nullable: false),
                    DUT_CODE = table.Column<int>(type: "INTEGER", nullable: false),
                    USER_CODE = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TEST_PLAN", x => x.TestCode);
                    table.ForeignKey(
                        name: "FK_TEST_PLAN_DUT_DUT_CODE",
                        column: x => x.DUT_CODE,
                        principalTable: "DUT",
                        principalColumn: "DutCode",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TEST_PLAN_TEST_USER_USER_CODE",
                        column: x => x.USER_CODE,
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
                    TEST_CODE = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Metric = table.Column<string>(type: "TEXT", nullable: false),
                    Value = table.Column<decimal>(type: "TEXT", nullable: false),
                    Target = table.Column<decimal>(type: "TEXT", nullable: false),
                    Pass = table.Column<int>(type: "INTEGER", nullable: false),
                    Parameters = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TEST_PARAMETER", x => x.ParamCode);
                    table.ForeignKey(
                        name: "FK_TEST_PARAMETER_TEST_PLAN_TEST_CODE",
                        column: x => x.TEST_CODE,
                        principalTable: "TEST_PLAN",
                        principalColumn: "TestCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "TEST_USER",
                columns: new[] { "UserCode", "Email", "FirstName", "LastName", "Password", "ProfileImage", "Username" },
                values: new object[] { 1, "drjjdevilla2002@gmail.com", "John Juvi", "De Villa", "password", "", "neurocoder" });

            migrationBuilder.CreateIndex(
                name: "IX_TEST_PARAMETER_TEST_CODE",
                table: "TEST_PARAMETER",
                column: "TEST_CODE");

            migrationBuilder.CreateIndex(
                name: "IX_TEST_PLAN_DUT_CODE",
                table: "TEST_PLAN",
                column: "DUT_CODE");

            migrationBuilder.CreateIndex(
                name: "IX_TEST_PLAN_USER_CODE",
                table: "TEST_PLAN",
                column: "USER_CODE");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TEST_PARAMETER");

            migrationBuilder.DropTable(
                name: "TEST_PLAN");

            migrationBuilder.DropTable(
                name: "DUT");

            migrationBuilder.DropTable(
                name: "TEST_USER");
        }
    }
}
