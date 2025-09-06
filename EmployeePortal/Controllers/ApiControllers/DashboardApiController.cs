using EmployeePortal.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeePortal.Controllers.ApiControllers
{
    public class DashboardApiController : Controller
    {
        private readonly IDashboardService _dashboardService;

        public DashboardApiController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet]
        [Route("api/posts")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPostsAsync(int page = 1, int pageSize = 10)
        {
            var employees = await _dashboardService.GetPostsPagedAsync(page, pageSize);
            return Json(employees);
        }

        [HttpPost]
        [Route("api/deletepost")]
        public async Task<IActionResult> DeletePost(Guid id)
        {
            bool isDeleted = await _dashboardService.DeletePostAsync(id);

            if (isDeleted)
            {
                return Json(new { success = true });
            }

            return BadRequest(new { success = false, message = "Could not delete post." });
        }
    }
}
