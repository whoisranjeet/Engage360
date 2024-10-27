using System.ComponentModel.DataAnnotations;

namespace EmployeePortal.Core.Models
{ 
    public class Employee
    {
        public Guid Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string EmailAddress { get; set; }
        public string MobileNumber { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Department { get; set; }
        
        public Guid? UserId { get; set; }
        public virtual User User { get; set; }
    }
}
