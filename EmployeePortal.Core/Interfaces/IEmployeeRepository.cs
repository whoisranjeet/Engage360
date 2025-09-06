using EmployeePortal.Core.DTOs;
using EmployeePortal.Core.Models;

namespace EmployeePortal.Core.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<bool> UserSignInAsync(User user);
        Task<bool> UserSignUpAsync(Employee employee);
        Task AddEmployeeAsync(Employee employee);
        Task<Guid> GetDefaultRoleIdAsync(string defaultRoleName);
        Task<List<Employee>> GetAllEmployeesAsync();
        Task<List<Role>> GetAllRolesAsync();
        Task<List<User>> GetAllUsersAsync();
        Task<User> GetUserByEmailAsync(string email);
        Task<bool> ModifyEmployeeRoleAsync(string EmailAddress, string RoleName);
        Task<bool> RemoveEmployeeAsync(string EmailAddress);
        Task<Employee> GetEmployeeDetailsAsync(string EmailAddress);
        Task<bool> UpdateEmployeeDetailsAsync(Employee emp, string emailAddress);
        Task<IEnumerable<Employee>> GetEmployeesPagedAsync(int page, int pageSize);
    }
}
