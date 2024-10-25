using EmployeePortal.Core.Models;

namespace EmployeePortal.Core.Interfaces
{
    public interface IEmployeeRepository
    {
        bool UserSignIn(User user);
        bool UserSignUp(Employee employee);
        void AddEmployee(Employee employee);        
    }
}
