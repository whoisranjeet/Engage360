using EmployeePortal.Core.DTOs;
using EmployeePortal.Core.Interfaces;
using EmployeePortal.Core.Models;
using Microsoft.Extensions.Logging;

namespace EmployeePortal.Services.Services
{
    public class SalaryService : ISalaryService
    {
        private readonly ISalaryRepository _repository;
        private readonly ILogger<SalaryService> _logger;

        public SalaryService(ISalaryRepository repository, ILogger<SalaryService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public bool CreditSalary(SalaryDto salaryDto)
        {
            var salary = new Salary
            {
                EmployeeEmail = salaryDto.EmployeeEmail,
                PayrollDate = salaryDto.PayrollDate,
                Basic = salaryDto.Basic,
                HRA = salaryDto.HRA,
                ShiftAllowance = salaryDto.ShiftAllowance,
                TravelAllowance = salaryDto.TravelAllowance,
                MiscellaneousCredit = salaryDto.MiscellaneousCredit,
                PT = salaryDto.PT,
                PF = salaryDto.PF,
                MiscellaneousDebit = salaryDto.MiscellaneousDebit,
                TotalEarning = salaryDto.TotalEarning,
                TotalDeduction = salaryDto.TotalDeduction,
                NetSalary = salaryDto.NetSalary,
                ProcessedBy = salaryDto.ProcessedBy,
                Remarks = salaryDto.Remarks
            };

            _repository.CreditSalary(salary);
            return true;
        }

        public List<EmployeeDto> GetEmployeeBasic()
        {
            var basic = _repository.GetEmployeeBasic();
            var basicDto = basic.Select(emp => new EmployeeDto
            {
                EmailAddress = emp.EmailAddress,
                Salary = emp.Salary
            }).ToList();

            return basicDto;
        }

        public List<string> GetAvailableDuration(string username)
        {
            return _repository.GetAvailableDuration(username);
        }

        public SalaryDto GetEmployeeSalary(string employeeEmail, string duration)
        {
            var salary = _repository.GetEmployeeSalary(employeeEmail, duration);
            
            var salaryDto = new SalaryDto
            {
                EmployeeEmail = employeeEmail,
                PayrollDate = salary.PayrollDate,
                Basic = salary.Basic,
                HRA = salary.HRA,
                ShiftAllowance = salary.ShiftAllowance,
                TravelAllowance = salary.TravelAllowance,
                MiscellaneousCredit = salary.MiscellaneousCredit,
                PT = salary.PT,
                PF = salary.PF,
                MiscellaneousDebit = salary.MiscellaneousDebit,
                TotalEarning = salary.TotalEarning,
                TotalDeduction = salary.TotalDeduction,
                NetSalary = salary.NetSalary,
                ProcessedBy = salary.ProcessedBy,
                Remarks = salary.Remarks
            };

            return salaryDto;
        }
    }
}
