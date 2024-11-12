using EmployeePortal.Core.DTOs;

namespace EmployeePortal.ViewModel
{
    public class UpdateEmployeeViewModel
    {
        public List<EmployeeDto> Employees { get; set; }
        public EmployeeDto SelectedEmployee { get; set; }
    }
}
