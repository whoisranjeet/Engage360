using EmployeePortal.Core.DTOs;

namespace EmployeePortal.ViewModel
{
    public class GeneratePayrollViewModel
    {
        public SalaryDto Salary { get; set; }
        public List<string> EmployeeEmails { get; set; }
    }
}
