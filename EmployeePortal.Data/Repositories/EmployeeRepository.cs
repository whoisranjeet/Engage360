using EmployeePortal.Core.Interfaces;
using EmployeePortal.Core.Models;
using EmployeePortal.Data.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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

        public List<Employee> GetAllEmployees()
        {
            return _context.Employees.ToList();
        }

        public List<Role> GetAllRoles()
        {
            return _context.Roles.ToList();
        }

        public List<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }

        public bool ModifyEmployeeRole(string EmailAddress, string RoleName)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(user => user.Username == EmailAddress);
                var role = _context.Roles.FirstOrDefault(role => role.RoleName == RoleName);

                if (user != null && role != null)
                {
                    user.RoleId = role.Id;
                }
                _context.Users.Update(user);
                _context.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error ocuured during modifying user role.");
                return false;
            }
        }

        public async Task<bool> RemoveEmployee(string EmailAddress)
        {
            try
            {
                var employee = await _context.Employees.FirstOrDefaultAsync(emp => emp.EmailAddress == EmailAddress);
                var user = _context.Users.FirstOrDefault(user => user.Username == EmailAddress);
                if (employee != null && user != null)
                {
                    _context.Employees.Remove(employee);
                    _context.Users.Remove(user);
                }
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error ocuured during removing employee details.");
                return false;
            }
        }

        public Employee GetEmployeeDetails(string emailAddress)
        {
            var employee = _context.Employees.FirstOrDefault(emp => emp.EmailAddress == emailAddress);

            if (employee != null)
            {
                return employee;
            }

            return new Employee { };
        }

        public bool UpdateEmployeeDetails(Employee emp, string emailAddress)
        {
            try
            {
                var employee = _context.Employees.FirstOrDefault(emp => emp.EmailAddress == emailAddress);

                employee.FirstName = emp.FirstName;
                employee.LastName = emp.LastName;
                employee.MobileNumber = emp.MobileNumber;
                employee.Address = emp.Address;
                employee.Department = emp.Department;
                employee.Gender = emp.Gender;
                employee.Salary = emp.Salary;

                if (emp.ProfilePicture != null)
                {
                    employee.ProfilePicture = emp.ProfilePicture;
                }

                _context.Employees.Update(employee);
                _context.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error ocuured during modifying user role.");
                return false;
            }
        }
    }
}
