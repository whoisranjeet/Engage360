using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeePortal.Data.Migrations
{
    /// <inheritdoc />
    public partial class addsalarymodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Salaries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PayrollDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Basic = table.Column<long>(type: "bigint", nullable: false),
                    HRA = table.Column<long>(type: "bigint", nullable: false),
                    ShiftAllowance = table.Column<long>(type: "bigint", nullable: false),
                    TravelAllowance = table.Column<long>(type: "bigint", nullable: false),
                    MiscellaneousCredit = table.Column<long>(type: "bigint", nullable: false),
                    PT = table.Column<long>(type: "bigint", nullable: false),
                    PF = table.Column<long>(type: "bigint", nullable: false),
                    MiscellaneousDebit = table.Column<long>(type: "bigint", nullable: false),
                    TotalEarning = table.Column<long>(type: "bigint", nullable: false),
                    TotalDeduction = table.Column<long>(type: "bigint", nullable: false),
                    NetSalary = table.Column<long>(type: "bigint", nullable: false),
                    ProcessedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Salaries", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Salaries");
        }
    }
}
