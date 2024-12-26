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
                    DUT_CODE = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DESCRIPTION = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DUT", x => x.DUT_CODE);
                });

            migrationBuilder.CreateTable(
                name: "TEST_USER",
                columns: table => new
                {
                    USER_CODE = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FIRST_NAME = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    LAST_NAME = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    EMAIL = table.Column<string>(type: "nvarchar(150)", nullable: false),
                    PROFILE_IMAGE = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    USER_NAME = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    PASSWORD = table.Column<string>(type: "nvarchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TEST_USER", x => x.USER_CODE);
                });

            migrationBuilder.CreateTable(
                name: "TEST_PLAN",
                columns: table => new
                {
                    TEST_CODE = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DATE = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CYCLE_NO = table.Column<int>(type: "INTEGER", nullable: false),
                    TEST_TIME = table.Column<int>(type: "INTEGER", nullable: false),
                    DUT_CODE = table.Column<int>(type: "INTEGER", nullable: false),
                    TEST_USERUSER_CODE = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TEST_PLAN", x => x.TEST_CODE);
                    table.ForeignKey(
                        name: "FK_TEST_PLAN_DUT_DUT_CODE",
                        column: x => x.DUT_CODE,
                        principalTable: "DUT",
                        principalColumn: "DUT_CODE",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TEST_PLAN_TEST_USER_TEST_USERUSER_CODE",
                        column: x => x.TEST_USERUSER_CODE,
                        principalTable: "TEST_USER",
                        principalColumn: "USER_CODE",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TEST_PARAMETER",
                columns: table => new
                {
                    PARAM_CODE = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TEST_PLANTEST_CODE = table.Column<int>(type: "INTEGER", nullable: false),
                    DESCRIPTION = table.Column<string>(type: "TEXT", nullable: true),
                    METRIC = table.Column<string>(type: "TEXT", nullable: false),
                    VALUE = table.Column<decimal>(type: "TEXT", nullable: false),
                    TARGET = table.Column<decimal>(type: "TEXT", nullable: false),
                    PASS = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TEST_PARAMETER", x => x.PARAM_CODE);
                    table.ForeignKey(
                        name: "FK_TEST_PARAMETER_TEST_PLAN_TEST_PLANTEST_CODE",
                        column: x => x.TEST_PLANTEST_CODE,
                        principalTable: "TEST_PLAN",
                        principalColumn: "TEST_CODE",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TEST_PARAMETER_TEST_PLANTEST_CODE",
                table: "TEST_PARAMETER",
                column: "TEST_PLANTEST_CODE");

            migrationBuilder.CreateIndex(
                name: "IX_TEST_PLAN_DUT_CODE",
                table: "TEST_PLAN",
                column: "DUT_CODE");

            migrationBuilder.CreateIndex(
                name: "IX_TEST_PLAN_TEST_USERUSER_CODE",
                table: "TEST_PLAN",
                column: "TEST_USERUSER_CODE");
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
