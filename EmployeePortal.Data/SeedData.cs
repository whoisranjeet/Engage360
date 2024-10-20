using EmployeePortal.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeePortal.Data
{
    public class Seed
    {
        public static async Task SeedRoles(AppDbContext context)
        {
            if (context.Roles.Any()) return;

            var roles = new List<Role>
            {
                new() { RoleName = "Admin" },
                new() { RoleName = "HR" },
                new() { RoleName = "Manager" },
                new() { RoleName = "Employee" }
            };

            await context.Roles.AddRangeAsync(roles);
            await context.SaveChangesAsync();
        }

        public static async Task SeedEmployees(AppDbContext context)
        {
            if (context.Employees.Any()) return;            

            var employees = new List<Employee>
            {            
                new() { FirstName = "Ranjeet", LastName = "Karmakar", EmailAddress = "ranjeetkarmakar@outlook.com", MobileNumber = "7304175111", Address = "Nagpur, India", Department = "Admin" },
                new() { FirstName = "Amit", LastName = "Sharma", EmailAddress = "amit.sharma@gmail.com", MobileNumber = "9876543210", Address = "Mumbai, India", Department = "HR" },
                new() { FirstName = "Neha", LastName = "Patil", EmailAddress = "neha.patil@outlook.com", MobileNumber = "9123456789", Address = "Pune, India", Department = "Manager" },
                new() { FirstName = "Kavita", LastName = "Singh", EmailAddress = "kavita.singh@gmail.com", MobileNumber = "8564123789", Address = "Bangalore, India", Department = "Employee" },
                new() { FirstName = "Rohit", LastName = "Verma", EmailAddress = "rohit.verma@hotmail.com", MobileNumber = "7845123698", Address = "Delhi, India", Department = "Admin" },
                new() { FirstName = "Sonal", LastName = "Jadhav", EmailAddress = "sonal.jadhav@gmail.com", MobileNumber = "7896541230", Address = "Chennai, India", Department = "HR" },
                new() { FirstName = "Ankit", LastName = "Deshmukh", EmailAddress = "ankit.deshmukh@hotmail.com", MobileNumber = "7412589632", Address = "Hyderabad, India", Department = "Employee" },
                new() { FirstName = "Priya", LastName = "Kulkarni", EmailAddress = "priya.kulkarni@gmail.com", MobileNumber = "9812345678", Address = "Pune, India", Department = "Admin" },
                new() { FirstName = "Suresh", LastName = "Rao", EmailAddress = "suresh.rao@hotmail.com", MobileNumber = "9823456789", Address = "Hyderabad, India", Department = "HR" },
                new() { FirstName = "Manisha", LastName = "Joshi", EmailAddress = "manisha.joshi@outlook.com", MobileNumber = "9876541230", Address = "Mumbai, India", Department = "Manager" },
                new() { FirstName = "Deepak", LastName = "Shetty", EmailAddress = "deepak.shetty@gmail.com", MobileNumber = "9123456787", Address = "Chennai, India", Department = "Employee" },
                new() { FirstName = "Akash", LastName = "Patel", EmailAddress = "akash.patel@outlook.com", MobileNumber = "9234567890", Address = "Delhi, India", Department = "Admin" },
                new() { FirstName = "Swati", LastName = "Nair", EmailAddress = "swati.nair@hotmail.com", MobileNumber = "9543217890", Address = "Nagpur, India", Department = "HR" },
                new() { FirstName = "Rahul", LastName = "Desai", EmailAddress = "rahul.desai@gmail.com", MobileNumber = "9456123789", Address = "Bangalore, India", Department = "Manager" },
                new() { FirstName = "Nikhil", LastName = "Gandhi", EmailAddress = "nikhil.gandhi@gmail.com", MobileNumber = "9654781236", Address = "Pune, India", Department = "Employee" },
                new() { FirstName = "Seema", LastName = "Iyer", EmailAddress = "seema.iyer@outlook.com", MobileNumber = "9654789654", Address = "Hyderabad, India", Department = "Admin" },
                new() { FirstName = "Ravi", LastName = "Kapoor", EmailAddress = "ravi.kapoor@gmail.com", MobileNumber = "9512347896", Address = "Delhi, India", Department = "HR" },
                new() { FirstName = "Siddharth", LastName = "Mehta", EmailAddress = "siddharth.mehta@outlook.com", MobileNumber = "9912345678", Address = "Mumbai, India", Department = "Manager" },
                new() { FirstName = "Arjun", LastName = "Singh", EmailAddress = "arjun.singh@hotmail.com", MobileNumber = "9632587410", Address = "Chennai, India", Department = "Employee" },
                new() { FirstName = "Pooja", LastName = "Shah", EmailAddress = "pooja.shah@outlook.com", MobileNumber = "9854671234", Address = "Bangalore, India", Department = "Employee" },
                new() { FirstName = "Meera", LastName = "Bhat", EmailAddress = "meera.bhat@gmail.com", MobileNumber = "9536782145", Address = "Nagpur, India", Department = "HR" },
                new() { FirstName = "Vikas", LastName = "Chopra", EmailAddress = "vikas.chopra@gmail.com", MobileNumber = "9321457896", Address = "Hyderabad, India", Department = "Manager" },
                new() { FirstName = "Gaurav", LastName = "Thakur", EmailAddress = "gaurav.thakur@hotmail.com", MobileNumber = "9412345670", Address = "Delhi, India", Department = "Employee" },
                new() { FirstName = "Nisha", LastName = "Agarwal", EmailAddress = "nisha.agarwal@gmail.com", MobileNumber = "9821456790", Address = "Mumbai, India", Department = "Admin" },
                new() { FirstName = "Sumit", LastName = "Verma", EmailAddress = "sumit.verma@gmail.com", MobileNumber = "9654781230", Address = "Chennai, India", Department = "HR" },
                new() { FirstName = "Tarun", LastName = "Mishra", EmailAddress = "tarun.mishra@hotmail.com", MobileNumber = "9745632189", Address = "Pune, India", Department = "Manager" },
                new() { FirstName = "Anjali", LastName = "Reddy", EmailAddress = "anjali.reddy@outlook.com", MobileNumber = "9876543214", Address = "Bangalore, India", Department = "Employee" },
                new() { FirstName = "Dinesh", LastName = "Naik", EmailAddress = "dinesh.naik@gmail.com", MobileNumber = "9876543125", Address = "Hyderabad, India", Department = "Employee" },
                new() { FirstName = "Shweta", LastName = "Sinha", EmailAddress = "shweta.sinha@outlook.com", MobileNumber = "9512347891", Address = "Delhi, India", Department = "HR" },
                new() { FirstName = "Karan", LastName = "Pillai", EmailAddress = "karan.pillai@gmail.com", MobileNumber = "9421347859", Address = "Nagpur, India", Department = "Manager" },
                new() { FirstName = "Vidya", LastName = "Menon", EmailAddress = "vidya.menon@gmail.com", MobileNumber = "9632147895", Address = "Mumbai, India", Department = "Employee" },
                new() { FirstName = "Raj", LastName = "Kapoor", EmailAddress = "raj.kapoor@hotmail.com", MobileNumber = "9412345678", Address = "Chennai, India", Department = "HR" },
                new() { FirstName = "Lakshmi", LastName = "Iyengar", EmailAddress = "lakshmi.iyengar@outlook.com", MobileNumber = "9812347890", Address = "Pune, India", Department = "HR" },
                new() { FirstName = "Ashok", LastName = "Gupta", EmailAddress = "ashok.gupta@gmail.com", MobileNumber = "9521476890", Address = "Hyderabad, India", Department = "Manager" },
                new() { FirstName = "Vandana", LastName = "Rana", EmailAddress = "vandana.rana@gmail.com", MobileNumber = "9852147896", Address = "Delhi, India", Department = "Employee" },
                new() { FirstName = "Sagar", LastName = "Deshmukh", EmailAddress = "sagar.deshmukh@gmail.com", MobileNumber = "9521378945", Address = "Bangalore, India", Department = "Manager" },
                new() { FirstName = "Sneha", LastName = "Shinde", EmailAddress = "sneha.shinde@outlook.com", MobileNumber = "9632147859", Address = "Chennai, India", Department = "HR" },
                new() { FirstName = "Vivek", LastName = "Bose", EmailAddress = "vivek.bose@hotmail.com", MobileNumber = "9412365789", Address = "Mumbai, India", Department = "Manager" },
                new() { FirstName = "Anita", LastName = "Saxena", EmailAddress = "anita.saxena@gmail.com", MobileNumber = "9321456789", Address = "Nagpur, India", Department = "Employee" },
                new() { FirstName = "Rohini", LastName = "Choudhary", EmailAddress = "rohini.choudhary@gmail.com", MobileNumber = "9412365478", Address = "Delhi, India", Department = "Manager" },
                new() { FirstName = "Rakesh", LastName = "Prasad", EmailAddress = "rakesh.prasad@gmail.com", MobileNumber = "9231465789", Address = "Hyderabad, India", Department = "HR" }
            };

            await context.Employees.AddRangeAsync(employees);
            await context.SaveChangesAsync();
        }
    }
}
