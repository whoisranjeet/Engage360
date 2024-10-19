using Microsoft.AspNetCore.Mvc;

namespace EmployeePortal.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
