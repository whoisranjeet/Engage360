using EmployeePortal.Core.DTOs;
using EmployeePortal.Core.Interfaces;
using EmployeePortal.Core.Models;
using Microsoft.Extensions.Logging;

namespace EmployeePortal.Services.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _dashboardRepository;
        private readonly ILogger<DashboardService> _logger;

        public DashboardService(IDashboardRepository dashboardRepository, ILogger<DashboardService> logger)
        {
            _dashboardRepository = dashboardRepository;
            _logger = logger;
        }

        public async Task<bool> CreatePostAsync(PostDto postDto)
        {
            try
            {
                var post = new Post
                {
                    Title = postDto.Title,
                    Description = postDto.Description,
                    ImageData = postDto.ImageData,
                    Author = postDto.Author,
                    DateOfPublishing = postDto.DateOfPublishing
                };

                await _dashboardRepository.CreatePostAsync(post);
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
                await _dashboardRepository.DeletePostAsync(id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error ocuured while deleting a social post.");
                return false;
            }
        }

        public async Task<IEnumerable<PostDto>> GetPostsPagedAsync(int page, int pageSize)
        {
            var posts = await _dashboardRepository.GetPostsPagedAsync(page, pageSize);
            
            return posts.Select(p => new PostDto
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                ImageData = p.ImageData,
                Author = p.Author,
                DateOfPublishing = p.DateOfPublishing
            });
        }
    }
}
