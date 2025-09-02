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

        public DashboardController(IDashboardService dashboardService, ApplicationDbContext context)
        {
            _dashboardService = dashboardService;
            _context = context;
        }

        [HttpGet]
        [Route("Dashboard")]
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

            var viewModel = new DashboardViewModel
            {
                Posts = posts
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost(DashboardViewModel viewModel)
        {
            if (viewModel.ImageUpload != null && viewModel.ImageUpload.Length > 0)
            {
                using var memoryStream = new MemoryStream();
                await viewModel.ImageUpload.CopyToAsync(memoryStream);
                viewModel.CreatePost.ImageData = memoryStream.ToArray();
            }

            viewModel.CreatePost.Author = User.FindFirst(ClaimTypes.Name)?.Value;
            viewModel.CreatePost.DateOfPublishing = DateTime.Now;

            if (_dashboardService.CreatePost(viewModel.CreatePost))
            {
                return Json(new
                {
                    success = true,
                    id = viewModel.CreatePost.Id,
                    title = viewModel.CreatePost.Title,
                    description = viewModel.CreatePost.Description,
                    author = viewModel.CreatePost.Author,
                    date = viewModel.CreatePost.DateOfPublishing.ToString("g"),
                    image = viewModel.CreatePost.ImageData != null
                        ? $"data:image/jpeg;base64,{Convert.ToBase64String(viewModel.CreatePost.ImageData)}"
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

        [Route("Contact-Us")]
        public IActionResult ContactUs()
        {
            return View();
        }
    }
}