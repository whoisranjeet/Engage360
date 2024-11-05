using EmployeePortal.Core.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EmployeePortal.Core.DTOs
{
    public class EmployeeDto
    {
        [Required]
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [Required]
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        [Required]
        [DisplayName("Gender")]
        public string Gender { get; set; }
        [Required]
        [DisplayName("Email")]
        public string EmailAddress { get; set; }
        [DisplayName("Mobile")]
        public string MobileNumber { get; set; }
        public string Address { get; set; }
        [Required]
        [DisplayName("Department")]
        public string Department { get; set; }
        [Required]
        [DisplayName("Password")]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        [DisplayName("Confirm Password")]
        public string ConfirmPassword { get; set; }
        public virtual User User { get; set; }
    }
}
