using EmployeePortal.Core.Models;
using EmployeePortal.Core.Interfaces;
using EmployeePortal.Data.Data;
using Microsoft.Extensions.Logging;
using System.Data;

namespace EmployeePortal.Data.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EmployeeRepository> _logger;

        public EmployeeRepository(ApplicationDbContext context, ILogger<EmployeeRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public bool UserSignIn(User user)
        {
            if (_context.Users.FirstOrDefault(obj => obj.Username == user.Username && obj.Password == user.Password) != null)
            {
                return true;
            }
            return false;
        }

        public bool UserSignUp(Employee employee)
        {
            _context.Employees.Add(employee);
            _context.SaveChanges();
            return true;
        }

        public void AddEmployee(Employee employee)
        {
            _context.Employees.Add(employee);
            _context.SaveChanges();
        }

        public Guid GetDefaultRoleId(string defaultRoleName)
        {
            Guid roleId = new();
            try
            {
                roleId = _context.Roles.FirstOrDefault(obj => obj.RoleName == defaultRoleName).Id;                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error ocuured during fetching roles.");
            }
            return roleId;
        }

        public List<Employee> GetAllEmployee()
        {
            return _context.Employees.ToList();
        }
    }
}
