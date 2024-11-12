using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeePortal.Data.Migrations
{
    /// <inheritdoc />
    public partial class addprofilepicturecolumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "ProfilePicture",
                table: "Employees",
                type: "varbinary(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfilePicture",
                table: "Employees");
        }
    }
}
