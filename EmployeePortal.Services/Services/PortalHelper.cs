using EmployeePortal.Core.Interfaces;

namespace EmployeePortal.Services.Services
{
    public class PortalHelper : IPortalHelper
    {
        public List<string> GetAllDepartment()
        {
            var Departments = new List<string>
            {
                "HR",
                "Finance",
                "Engineering",
                "Marketing",
                "Management",
                "Other"
            };

            return Departments;
        }

        public List<string> GetAllGender()
        {
            var Genders = new List<string>
            {
                "Male",
                "Female",
                "Other"
            };

            return Genders;
        }
    }
}
