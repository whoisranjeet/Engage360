using EmployeePortal.Core.DTOs;
using EmployeePortal.Core.Interfaces;
using EmployeePortal.Core.Models;
using EmployeePortal.Data.Data;

namespace EmployeePortal.Data.Repositories
{
    public class SalaryRepository : ISalaryRepository
    {
        private readonly ApplicationDbContext _context;

        public SalaryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool CreditSalary(Salary salary)
        {
            var existingSalary = _context.Salaries.FirstOrDefault(s => s.EmployeeEmail == salary.EmployeeEmail && s.PayrollDate == salary.PayrollDate);

            if (existingSalary != null)
            {
                existingSalary.Basic = salary.Basic;
                existingSalary.HRA = salary.HRA;
                existingSalary.ShiftAllowance = salary.ShiftAllowance;
                existingSalary.TravelAllowance = salary.TravelAllowance;
                existingSalary.MiscellaneousCredit = salary.MiscellaneousCredit;
                existingSalary.PF = salary.PF;
                existingSalary.PT = salary.PT;
                existingSalary.MiscellaneousDebit = salary.MiscellaneousDebit;
                existingSalary.TotalEarning = salary.TotalEarning;
                existingSalary.TotalDeduction = salary.TotalDeduction;
                existingSalary.NetSalary = salary.NetSalary;
                existingSalary.Remarks = salary.Remarks;
                existingSalary.ProcessedBy = salary.ProcessedBy;

                _context.Salaries.Update(existingSalary);
            }
            else
            {
                _context.Salaries.Add(salary);
            }

            _context.SaveChanges();
            return true;
        }

        public List<Employee> GetEmployeeBasic()
        {
            var employee = _context.Employees.ToList();

            var employeeBasic = employee.Select(emp => new Employee
            {
                EmailAddress = emp.EmailAddress,
                Salary = emp.Salary
            }).ToList();

            return employeeBasic;
        }

        public List<string> GetAvailableDuration(string username)
        {
            var payrollDates = _context.Salaries.Where(s => s.EmployeeEmail == username).Select(s => s.PayrollDate).Distinct().ToList();
            
            if(payrollDates.Count > 0) { return payrollDates; }     
            
            return new List<string> {"No Data Found. !!!"};
        }

        public Salary GetEmployeeSalary(string employeeEmail, string duration)
        {
            var salary = _context.Salaries.FirstOrDefault(s => s.EmployeeEmail == employeeEmail && s.PayrollDate == duration);

            return salary;
        }
    }
}
