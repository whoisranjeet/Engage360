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

        public bool CreatePost(PostDto postDto)
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

                _dashboardRepository.CreatePost(post);
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
                _dashboardRepository.DeletePost(id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error ocuured while deleting a social post.");
                return false;
            }
        }

        public IEnumerable<PostDto> GetPostsPaged(int page, int pageSize)
        {
            var posts = _dashboardRepository.GetPostsPaged(page, pageSize);
            
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
