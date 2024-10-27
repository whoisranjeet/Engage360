using System.ComponentModel.DataAnnotations;

namespace EmployeePortal.Core.Models
{
    public class Role
    {
        public Guid Id { get; set; }
        [Required]
        public string RoleName { get; set; }
    }
}
