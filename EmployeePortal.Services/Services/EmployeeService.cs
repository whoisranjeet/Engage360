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
                    Password = employeeDto.Password,
                    RoleId = GetDefaultRoleId("Associate")
                }
            };
            if (_employeeRepository.UserSignUp(employee))
            {
                return true;
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
                Department = employeeDto.Department,
                User = new User
                {
                    Username = employeeDto.EmailAddress,
                    Password = employeeDto.Password,
                    RoleId = GetDefaultRoleId("Associate")
                }
            };
            _employeeRepository.AddEmployee(employee);
        }

        public Guid GetDefaultRoleId(string defaultRoleName)
        {
            return _employeeRepository.GetDefaultRoleId(defaultRoleName);
        }

        public List<EmployeeDto> GetAllEmployees()
        {
            var employees = _employeeRepository.GetAllEmployees();

            var employeeDtos = employees.Select(employee => new EmployeeDto
            {
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                EmailAddress = employee.EmailAddress,
                MobileNumber = employee.MobileNumber,
                Address = employee.Address,
                Department = employee.Department
            })
            .OrderBy(dto => dto.FirstName)
            .ToList();

            return employeeDtos;
        }

        public List<RoleDto> GetAllRoles()
        {
            var roles = _employeeRepository.GetAllRoles();

            var rolesDtos = roles.Select(role => new RoleDto
            {
                Id = role.Id,
                RoleName = role.RoleName
            }).ToList();

            return rolesDtos;
        }

        public List<UserDto> GetAllUsers()
        {
            var users = _employeeRepository.GetAllUsers();

            var usersDtos = users.Select(user => new UserDto
            {
                Username = user.Username,
                RoleId = user.RoleId
            }).ToList();

            return usersDtos;
        }

        public bool ModifyEmployeeRole(string EmailAddress, string RoleName)
        {
            if (_employeeRepository.ModifyEmployeeRole(EmailAddress, RoleName))
            {
                return true;
            }
            return false;
        }

        public async Task<bool> RemoveEmployee(string EmailAddress)
        {
            if (await _employeeRepository.RemoveEmployee(EmailAddress))
            {
                return true;
            }
            return false;
        }
    }
}
