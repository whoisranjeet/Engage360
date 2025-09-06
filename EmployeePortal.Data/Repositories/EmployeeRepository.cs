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
        public async Task<bool> UserSignInAsync(User user)
        {
            if (await _context.Users.FirstOrDefaultAsync(obj => obj.Username == user.Username && obj.Password == user.Password) != null)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> UserSignUpAsync(Employee employee)
        {
            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task AddEmployeeAsync(Employee employee)
        {
            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();
        }

        public async Task<Guid> GetDefaultRoleIdAsync(string defaultRoleName)
        {
            Guid roleId = new();
            try
            {
                var role = await _context.Roles.FirstOrDefaultAsync(obj => obj.RoleName == defaultRoleName);
                if (role != null)
                {
                    roleId = role.Id;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error ocuured during fetching roles.");
            }
            return roleId;
        }

        public async Task<List<Employee>> GetAllEmployeesAsync()
        {
            return await _context.Employees.ToListAsync();
        }

        public async Task<List<Role>> GetAllRolesAsync()
        {
            return await _context.Roles.ToListAsync();
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username.ToLower() == email.ToLower());
        }

        public async Task<bool> ModifyEmployeeRoleAsync(string EmailAddress, string RoleName)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(user => user.Username == EmailAddress);
                var role = await _context.Roles.FirstOrDefaultAsync(role => role.RoleName == RoleName);

                if (user != null && role != null)
                {
                    user.RoleId = role.Id;
                }
                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error ocuured during modifying user role.");
                return false;
            }
        }

        public async Task<bool> RemoveEmployeeAsync(string EmailAddress)
        {
            try
            {
                var employee = await _context.Employees.FirstOrDefaultAsync(emp => emp.EmailAddress == EmailAddress);
                var user = await _context.Users.FirstOrDefaultAsync(user => user.Username == EmailAddress);
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

        public async Task<Employee> GetEmployeeDetailsAsync(string emailAddress)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(emp => emp.EmailAddress == emailAddress);

            if (employee != null)
            {
                return employee;
            }

            return new Employee { };
        }

        public async Task<bool> UpdateEmployeeDetailsAsync(Employee emp, string emailAddress)
        {
            try
            {
                var employee = await _context.Employees.FirstOrDefaultAsync(emp => emp.EmailAddress == emailAddress);

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
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error ocuured during modifying user role.");
                return false;
            }
        }

        public async Task<IEnumerable<Employee>> GetEmployeesPagedAsync(int page, int pageSize)
        {
            return await _context.Employees
               .OrderBy(e => e.FirstName)
               .Skip((page - 1) * pageSize)
               .Take(pageSize)
               .Select(e => new Employee
               {
                   FirstName = e.FirstName,
                   LastName = e.LastName,
                   Gender = e.Gender,
                   ProfilePicture = e.ProfilePicture,
                   EmailAddress = e.EmailAddress,
                   MobileNumber = e.MobileNumber,
                   Address = e.Address,
                   Department = e.Department
               }).ToListAsync();
        }
    }
}
