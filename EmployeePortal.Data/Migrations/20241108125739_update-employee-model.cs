using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeePortal.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateemployeemodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "Salary",
                table: "Employees",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Salary",
                table: "Employees");
        }
    }
}
