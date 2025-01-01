using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CID_Tester.Migrations
{
    /// <inheritdoc />
    public partial class TestPlanMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "TEST_PLAN",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "TEST_PARAMETER",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "TEST_PLAN");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "TEST_PARAMETER");
        }
    }
}
