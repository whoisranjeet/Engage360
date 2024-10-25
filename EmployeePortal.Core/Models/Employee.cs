namespace EmployeePortal.Core.Models
{ 
    public class Employee
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string MobileNumber { get; set; }
        public string Address { get; set; }
        public string Department { get; set; }
        public Guid? UserId { get; set; }
        public virtual User User { get; set; }
    }
}
