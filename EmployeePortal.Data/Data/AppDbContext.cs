using EmployeePortal.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeePortal.Data.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Salary> Salaries { get; set; }
    }
}
