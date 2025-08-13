using System.ComponentModel.DataAnnotations;

namespace EmployeePortal.Core.Models
{
    public class User
    {
        public Guid Id { get; set; }
        [Required]
        public string Username { get; set; }
        public string Password { get; set; }
        public Guid? RoleId { get; set; }
        public virtual Role Role { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
