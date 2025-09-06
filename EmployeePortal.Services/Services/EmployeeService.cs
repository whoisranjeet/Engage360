using EmployeePortal.Core.DTOs;
using EmployeePortal.Core.Interfaces;
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

        public async Task<bool> UserSignInAsync(UserDto userDto)
        {
            var user = new User
            {
                Username = userDto.Username,
                Password = userDto.Password
            };
            if (await _employeeRepository.UserSignInAsync(user))
            {
                return true;
            }
            return false;
        }

        public async Task<bool> UserSignUpAsync(EmployeeDto employeeDto)
        {
            var employee = new Employee
            {
                FirstName = employeeDto.FirstName,
                LastName = employeeDto.LastName,
                ProfilePicture = employeeDto.ProfilePicture,
                EmailAddress = employeeDto.EmailAddress,
                Gender = employeeDto.Gender,
                MobileNumber = employeeDto.MobileNumber,
                Address = employeeDto.Address,
                Department = employeeDto.Department,
                User = new User
                {
                    Username = employeeDto.EmailAddress,
                    Password = employeeDto.Password,
                    RoleId = await GetDefaultRoleIdAsync("Associate")
                }
            };
            if (await _employeeRepository.UserSignUpAsync(employee))
            {
                return true;
            }
            return false;
        }

        public async Task AddEmployeeAsync(EmployeeDto employeeDto)
        {
            var employee = new Employee
            {
                FirstName = employeeDto.FirstName,
                LastName = employeeDto.LastName,
                ProfilePicture = employeeDto.ProfilePicture,
                EmailAddress = employeeDto.EmailAddress,
                Gender = employeeDto.Gender,
                MobileNumber = employeeDto.MobileNumber,
                Address = employeeDto.Address,
                Department = employeeDto.Department,
                User = new User
                {
                    Username = employeeDto.EmailAddress,
                    Password = employeeDto.Password,
                    RoleId = await GetDefaultRoleIdAsync("Associate")
                }
            };
            await _employeeRepository.AddEmployeeAsync(employee);
        }

        public async Task<Guid> GetDefaultRoleIdAsync(string defaultRoleName)
        {
            return await _employeeRepository.GetDefaultRoleIdAsync(defaultRoleName);
        }

        public async Task<List<EmployeeDto>> GetAllEmployeesAsync()
        {
            var employees = await _employeeRepository.GetAllEmployeesAsync();

            var employeeDtos = employees.Select(employee => new EmployeeDto
            {
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                ProfilePicture = employee.ProfilePicture,
                EmailAddress = employee.EmailAddress,
                Gender = employee.Gender,
                MobileNumber = employee.MobileNumber,
                Address = employee.Address,
                Department = employee.Department,
                Salary = employee.Salary
            })
            .OrderBy(dto => dto.FirstName)
            .ToList();

            return employeeDtos;
        }

        public async Task<List<RoleDto>> GetAllRolesAsync()
        {
            var roles = await _employeeRepository.GetAllRolesAsync();

            var rolesDtos = roles.Select(role => new RoleDto
            {
                Id = role.Id,
                RoleName = role.RoleName
            }).ToList();

            return rolesDtos;
        }

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            var users = await _employeeRepository.GetAllUsersAsync();

            var usersDtos = users.Select(user => new UserDto
            {
                Username = user.Username,
                RoleId = user.RoleId
            }).ToList();

            return usersDtos;
        }

        public async Task<UserDto> GetUserByEmailAsync(string email)
        {
            var user = await _employeeRepository.GetUserByEmailAsync(email);
            var userDto = new UserDto
            {
                Username = user.Username,
                RoleId = user.RoleId
            };
            return userDto;
        }

        public async Task<bool> ModifyEmployeeRoleAsync(string EmailAddress, string RoleName)
        {
            if (await _employeeRepository.ModifyEmployeeRoleAsync(EmailAddress, RoleName))
            {
                return true;
            }
            return false;
        }

        public async Task<bool> RemoveEmployeeAsync(string EmailAddress)
        {
            if (await _employeeRepository.RemoveEmployeeAsync(EmailAddress))
            {
                return true;
            }
            return false;
        }

        public async Task<EmployeeDto> GetEmployeeDetailsAsync(string EmailAddress)
        {
            var employee = await _employeeRepository.GetEmployeeDetailsAsync(EmailAddress);
            var employeeDto = new EmployeeDto
            {
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                ProfilePicture = employee.ProfilePicture,
                EmailAddress = employee.EmailAddress,
                Gender = employee.Gender,
                MobileNumber = employee.MobileNumber,
                Department = employee.Department,
                Address = employee.Address
            };

            return employeeDto;
        }

        public async Task<bool> UpdateEmployeeDetailsAsync(EmployeeDto emp, string emailAddress)
        {
            var employee = new Employee
            {
                FirstName = emp.FirstName,
                LastName = emp.LastName,
                ProfilePicture = emp.ProfilePicture,
                EmailAddress = emp.EmailAddress,
                Gender = emp.Gender,
                MobileNumber = emp.MobileNumber,
                Department = emp.Department,
                Address = emp.Address,
                Salary = emp.Salary
            };

            if (await _employeeRepository.UpdateEmployeeDetailsAsync(employee, emailAddress))
            {
                return true;
            }

            return false;
        }

        public async Task<IEnumerable<EmployeeDto>> GetEmployeesPagedAsync(int page, int pageSize)
        {
            var emp = await _employeeRepository.GetEmployeesPagedAsync(page, pageSize);

            return emp.Select(e => new EmployeeDto
            {
                FirstName = e.FirstName,
                LastName = e.LastName,
                ProfilePicture = e.ProfilePicture,
                EmailAddress = e.EmailAddress,
                Gender = e.Gender,
                MobileNumber = e.MobileNumber,
                Department = e.Department,
                Address = e.Address,
            });
        }
    }
}
