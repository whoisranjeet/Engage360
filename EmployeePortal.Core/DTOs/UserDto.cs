using System.ComponentModel.DataAnnotations;

namespace EmployeePortal.Core.DTOs
{
    public class UserDto
    {
        [Required]
        [Display(Name = "Username/Email")]
        public string Username { get; set; }
        [Required]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}
