using EmployeePortal.Core.Interfaces;
using EmployeePortal.Core.Models;
using EmployeePortal.Data.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EmployeePortal.Data.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DashboardRepository> _logger;

        public DashboardRepository(ApplicationDbContext context, ILogger<DashboardRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<bool> CreatePostAsync(Post post)
        {
            try
            {
                await _context.Posts.AddAsync(post);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error ocuured while creating a social post.");
                return false;
            }
        }

        public async Task<bool> DeletePostAsync(Guid id)
        {
            try
            {
                var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == id);
                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error ocuured while deleting a social post.");
                return false;
            }
        }

        public async Task<IEnumerable<Post>> GetPostsPagedAsync(int page, int pageSize)
        {

            return await _context.Posts
                .OrderByDescending(p => p.DateOfPublishing)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new Post
                {
                    Id = p.Id,
                    Title = p.Title,
                    Description = p.Description,
                    Author = p.Author,
                    DateOfPublishing = p.DateOfPublishing,
                    ImageData = p.ImageData,
                }).ToListAsync();
        }
    }
}
