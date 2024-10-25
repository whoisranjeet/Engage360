using EmployeePortal.Core.DTOs;

namespace EmployeePortal.Core.Interfaces
{
    public interface IEmployeeService
    {
        bool UserSignIn(UserDto userDto);        
        bool UserSignUp(EmployeeDto employeeDto);
        void AddEmployee(EmployeeDto employeeDto);
    }
}
