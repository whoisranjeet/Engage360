using EmployeePortal.Core.Interfaces;
using EmployeePortal.Core.DTOs;
using EmployeePortal.Core.Models;

namespace EmployeePortal.Services.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public bool UserSignIn(UserDto userDto)
        {
            var user = new User
            {
                Username = userDto.Username,
                Password = userDto.Password
            };
            if (_employeeRepository.UserSignIn(user))
            {
                return true;
            }                
            return false;
        }

        public bool UserSignUp(EmployeeDto employeeDto)
        {
            var employee = new Employee
            {
                FirstName = employeeDto.FirstName,
                LastName = employeeDto.LastName,
                EmailAddress = employeeDto.EmailAddress,
                MobileNumber = employeeDto.MobileNumber,
                Address = employeeDto.Address,
                Department = employeeDto.Department,
                User = new User
                {
                    Username = employeeDto.EmailAddress,
                    Password = employeeDto.Password
                }
            };
            if (_employeeRepository.UserSignUp(employee))
            {
                return true ;
            }
            return false;
        }

        public void AddEmployee(EmployeeDto employeeDto)
        {
            var employee = new Employee
            {
                FirstName = employeeDto.FirstName,
                LastName = employeeDto.LastName,
                EmailAddress = employeeDto.EmailAddress,
                MobileNumber = employeeDto.MobileNumber,
                Address = employeeDto.Address,
                Department = employeeDto.Department
            };
            _employeeRepository.AddEmployee(employee);
        }        
    }
}
