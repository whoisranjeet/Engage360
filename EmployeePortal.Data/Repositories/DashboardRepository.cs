using EmployeePortal.Core.Interfaces;
using EmployeePortal.Core.Models;
using EmployeePortal.Data.Data;
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
        public bool CreatePost(Post post)
        {
            try
            {
                _context.Posts.Add(post);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error ocuured while creating a social post.");
                return false;
            }
        }

        public bool DeletePost(Guid id)
        {
            try
            {
                var post = _context.Posts.FirstOrDefault(p => p.Id == id);
                _context.Posts.Remove(post);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error ocuured while deleting a social post.");
                return false;
            }
        }
    }
}
