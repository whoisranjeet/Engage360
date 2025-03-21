using EmployeePortal.Core.DTOs;

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
        bool ModifyEmployeeRole(string EmailAddress, string RoleName);
        Task<bool> RemoveEmployee(string EmailAddress);
        EmployeeDto GetEmployeeDetails(string EmailAddress);
        bool UpdateEmployeeDetails(EmployeeDto emp, string emailAddress);
    }
}
