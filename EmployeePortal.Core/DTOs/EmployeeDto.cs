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
        public string Gender { get; set; }
        public byte[] ProfilePicture { get; set; }
        [Required]
        [DisplayName("Email")]
        public string EmailAddress { get; set; }
        [DisplayName("Mobile")]
        public string MobileNumber { get; set; }
        public string Address { get; set; }
        public string Department { get; set; }
        public long Salary { get; set; }
        public string Password { get; set; }
        public virtual User User { get; set; }
    }
}
