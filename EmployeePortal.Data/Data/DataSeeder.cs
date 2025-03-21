using EmployeePortal.Core.Models;

namespace EmployeePortal.Data.Data
{
    public class DataSeeder
    {
        public static async Task SeedRoles(ApplicationDbContext context)
        {
            if (context.Roles.Any()) return;

            var roles = new List<Role>
            {
                new() { RoleName = "Admin" },
                new() { RoleName = "HR" },
                new() { RoleName = "Manager" },
                new() { RoleName = "Developer" },
                new() { RoleName = "Analyst" },
                new() { RoleName = "Associate" },
                new() { RoleName = "Tester" }
            };

            await context.Roles.AddRangeAsync(roles);
            await context.SaveChangesAsync();
        }
    }
}
