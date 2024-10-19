using System.ComponentModel.DataAnnotations;

namespace EmployeePortal.Models
{
    public class SignInViewModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
