using EmployeePortal.Core.DTOs;
using EmployeePortal.Core.Models;

namespace EmployeePortal.Core.Interfaces
{
    public interface IEmployeeService
    {
        bool UserSignIn(UserDto userDto);
        bool UserSignUp(EmployeeDto employeeDto);
        void AddEmployee(EmployeeDto employeeDto);
        Guid GetDefaultRoleId(string defaultRoleName);
        List<EmployeeDto> GetAllEmployees();
        List<RoleDto> GetAllRoles();
        List<UserDto> GetAllUsers();
        Task<User> GetUserByEmailAsync(string email);
        bool ModifyEmployeeRole(string EmailAddress, string RoleName);
        Task<bool> RemoveEmployee(string EmailAddress);
        EmployeeDto GetEmployeeDetails(string EmailAddress);
        bool UpdateEmployeeDetails(EmployeeDto emp, string emailAddress);
        IEnumerable<EmployeeDto> GetEmployeesPaged(int page, int pageSize);
    }
}
