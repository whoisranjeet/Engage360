namespace EmployeePortal.Core.DTOs
{
    public class RoleMappingDto
    {
        public List<RoleDto> Roles { get; set; }
        public List<EmployeeDto> Employees { get; set; }
        public List<UserDto> Users { get; set; }
        public string SelectedEmployeeName { get; set; }
        public string RoleName { get; set; }
        public string EmailAddress { get; set; }
    }
}
