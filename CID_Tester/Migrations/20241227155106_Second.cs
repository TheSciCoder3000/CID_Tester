using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CID_Tester.Migrations
{
    /// <inheritdoc />
    public partial class Second : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TEST_PARAMETER_TEST_PLAN_PARAM_CODE",
                table: "TEST_PARAMETER");

            migrationBuilder.AddColumn<int>(
                name: "TEST_CODE",
                table: "TEST_PARAMETER",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TEST_PARAMETER_TEST_CODE",
                table: "TEST_PARAMETER",
                column: "TEST_CODE");

            migrationBuilder.AddForeignKey(
                name: "FK_TEST_PARAMETER_TEST_PLAN_TEST_CODE",
                table: "TEST_PARAMETER",
                column: "TEST_CODE",
                principalTable: "TEST_PLAN",
                principalColumn: "TEST_CODE",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TEST_PARAMETER_TEST_PLAN_TEST_CODE",
                table: "TEST_PARAMETER");

            migrationBuilder.DropIndex(
                name: "IX_TEST_PARAMETER_TEST_CODE",
                table: "TEST_PARAMETER");

            migrationBuilder.DropColumn(
                name: "TEST_CODE",
                table: "TEST_PARAMETER");

            migrationBuilder.AddForeignKey(
                name: "FK_TEST_PARAMETER_TEST_PLAN_PARAM_CODE",
                table: "TEST_PARAMETER",
                column: "PARAM_CODE",
                principalTable: "TEST_PLAN",
                principalColumn: "TEST_CODE",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
