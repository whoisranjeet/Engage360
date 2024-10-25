using EmployeePortal.Core.Models;
using EmployeePortal.Core.Interfaces;
using EmployeePortal.Data.Data;

namespace EmployeePortal.Data.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _context;

        public EmployeeRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool UserSignIn(User user)
        {
            if (_context.Users.FirstOrDefault(obj => obj.Username == user.Username && obj.Password == user.Password) != null)
            {
                return true;
            }
            return false;
        }

        public bool UserSignUp(Employee employee)
        {
            _context.Employees.Add(employee);
            _context.SaveChanges();
            return true;
        }

        public void AddEmployee(Employee employee)
        {
            _context.Employees.Add(employee);
            _context.SaveChanges();
        }
    }
}
