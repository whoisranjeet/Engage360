using EmployeePortal.Core.DTOs;
using EmployeePortal.Core.Models;

namespace EmployeePortal.Core.Interfaces
{
    public interface IEmployeeRepository
    {
        bool UserSignIn(User user);
        bool UserSignUp(Employee employee);
        void AddEmployee(Employee employee);
        Guid GetDefaultRoleId(string defaultRoleName);
        List<Employee> GetAllEmployees();
        List<Role> GetAllRoles();
        List<User> GetAllUsers();
        Task<User> GetUserByEmailAsync(string email);
        bool ModifyEmployeeRole(string EmailAddress, string RoleName);
        Task<bool> RemoveEmployee(string EmailAddress);
        Employee GetEmployeeDetails(string EmailAddress);
        bool UpdateEmployeeDetails(Employee emp, string emailAddress);
        IEnumerable<Employee> GetEmployeesPaged(int page, int pageSize);
    }
}
