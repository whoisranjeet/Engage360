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

            //return View(posts); // Pass posts to the view
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Dashboard(DashboardViewModel viewModel, IFormFile imageUpload)
        {
            if (imageUpload != null && imageUpload.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await imageUpload.CopyToAsync(memoryStream);
                    viewModel.CreatePost.ImageData = memoryStream.ToArray(); // Save the file data as a byte array in the DTO
                }
                var author = User.FindFirst(ClaimTypes.Name)?.Value;

                viewModel.CreatePost.Author = author;
                viewModel.CreatePost.DateOfPublishing = DateTime.Now;

                if (_dashboardService.CreatePost(viewModel.CreatePost))
                {
                    TempData["PostSuccess"] = "New Post Created Successfully !!!";
                    return RedirectToAction("Dashboard", "Dashboard");
                }
            }

            // Now postDto contains Title, Description, and ImageData.
            // You can save this data to the database here.

            return View(viewModel); // Return view with updated postDto (or redirect to another view as needed)
        }

        [Route("Contact Us")]
        public IActionResult ContactUs()
        {
            return View();
        }
    }
}