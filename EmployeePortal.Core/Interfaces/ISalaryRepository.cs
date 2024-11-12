using EmployeePortal.Core.Models;

namespace EmployeePortal.Core.Interfaces
{
    public interface ISalaryRepository
    {
        List<Employee> GetEmployeeBasic();
        bool CreditSalary(Salary salary);
        List<string> GetAvailableDuration(string username);
        Salary GetEmployeeSalary(string employeeEmail, string duration);
    }
}
