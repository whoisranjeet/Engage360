using EmployeePortal.Core.DTOs;
using EmployeePortal.Core.Interfaces;
using EmployeePortal.Data.Data;
using EmployeePortal.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EmployeePortal.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IDashboardService _dashboardService;
        private readonly ApplicationDbContext _context;

        public DashboardController(IDashboardService dashboardService, ApplicationDbContext context, IEmployeeService employeeService)
        {
            _dashboardService = dashboardService;
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Dashboard()
        {
            var posts = await _context.Posts
                .Select(post => new PostDto
                {
                    Id = post.Id,
                    Title = post.Title,
                    Description = post.Description,
                    Author = post.Author,
                    DateOfPublishing = post.DateOfPublishing,
                    ImageData = post.ImageData
                })
                .ToListAsync();

            var viewModel = new LayoutViewModel
            {
                Posts = posts
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost(LayoutViewModel viewModel)
        {
            if (viewModel.Image != null && viewModel.Image.Length > 0)
            {
                using var memoryStream = new MemoryStream();
                await viewModel.Image.CopyToAsync(memoryStream);
                viewModel.Post.ImageData = memoryStream.ToArray();
            }

            viewModel.Post.Author = User.FindFirst(ClaimTypes.Name)?.Value;
            viewModel.Post.DateOfPublishing = DateTime.Now;

            if (_dashboardService.CreatePost(viewModel.Post))
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

        [HttpPost]
        public IActionResult DeletePost(Guid id)
        {
            bool isDeleted = _dashboardService.DeletePost(id);

            if (isDeleted)
            {
                return Json(new { success = true });
            }

            return BadRequest(new { success = false, message = "Could not delete post." });
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("Contact-Us")]        
        public IActionResult ContactUs()
        {
            return View();
        }

        //[HttpPost]
        //[AllowAnonymous]
        //[Route("Contact-Us")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> ContactUs(ContactUsViewModel viewModel)
        //{
        //    return View();
        //}
    }
}