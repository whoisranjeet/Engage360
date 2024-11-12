using EmployeePortal.Core.DTOs;

namespace EmployeePortal.Core.Interfaces
{
    public interface ISalaryService
    {
        List<EmployeeDto> GetEmployeeBasic();
        bool CreditSalary(SalaryDto salaryDto);
        List<string> GetAvailableDuration(string username);
        SalaryDto GetEmployeeSalary(string employeeEmail, string duration);
    }
}
