using EmployeePortal.Core.Interfaces;
using EmployeePortal.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EmployeePortal.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService, IEmployeeService employeeService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Dashboard()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost(CreatePostViewModel viewModel)
        {
            if (viewModel.Image != null && viewModel.Image.Length > 0)
            {
                using var memoryStream = new MemoryStream();
                await viewModel.Image.CopyToAsync(memoryStream);
                viewModel.Post.ImageData = memoryStream.ToArray();
            }

            viewModel.Post.Author = User.FindFirst(ClaimTypes.Name)?.Value;
            viewModel.Post.DateOfPublishing = DateTime.Now;

            if (await _dashboardService.CreatePostAsync(viewModel.Post))
            {
                return Json(new
                {
                    success = true,
                    id = viewModel.Post.Id,
                    title = viewModel.Post.Title,
                    description = viewModel.Post.Description,
                    author = viewModel.Post.Author,
                    date = viewModel.Post.DateOfPublishing.ToString("g"),
                    image = viewModel.Post.ImageData != null
                        ? $"data:image/jpeg;base64,{Convert.ToBase64String(viewModel.Post.ImageData)}"
                        : null
                });
            }

            return BadRequest(new { success = false, message = "Could not create post." });
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("Contact")]        
        public IActionResult ContactUs()
        {
            return View();
        }
    }
}