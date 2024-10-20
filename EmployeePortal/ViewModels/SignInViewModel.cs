using System.ComponentModel.DataAnnotations;

namespace EmployeePortal.ViewModels
{
    public class SignInViewModel
    {
        [Required]
        [Display(Name ="Username/Email")]
        public string Username { get; set; }
        [Required]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}
