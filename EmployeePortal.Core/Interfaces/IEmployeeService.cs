using EmployeePortal.Core.DTOs;
using EmployeePortal.Core.Models;

namespace EmployeePortal.Core.Interfaces
{
    public interface IEmployeeService
    {
        Task<bool> UserSignInAsync(UserDto userDto);
        Task<bool> UserSignUpAsync(EmployeeDto employeeDto);
        Task AddEmployeeAsync(EmployeeDto employeeDto);
        Task<Guid> GetDefaultRoleIdAsync(string defaultRoleName);
        Task<List<EmployeeDto>> GetAllEmployeesAsync();
        Task<List<RoleDto>> GetAllRolesAsync();
        Task<List<UserDto>> GetAllUsersAsync();
        Task<UserDto> GetUserByEmailAsync(string email);
        Task<bool> ModifyEmployeeRoleAsync(string EmailAddress, string RoleName);
        Task<bool> RemoveEmployeeAsync(string EmailAddress);
        Task<EmployeeDto> GetEmployeeDetailsAsync(string EmailAddress);
        Task<bool> UpdateEmployeeDetailsAsync(EmployeeDto emp, string emailAddress);
        Task<IEnumerable<EmployeeDto>> GetEmployeesPagedAsync(int page, int pageSize);
    }
}
