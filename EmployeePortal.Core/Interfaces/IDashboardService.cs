using EmployeePortal.Core.DTOs;

namespace EmployeePortal.Core.Interfaces
{
    public interface IDashboardService
    {
        Task<bool> CreatePostAsync(PostDto postDto);
        Task<bool> DeletePostAsync(Guid id);
        Task<IEnumerable<PostDto>> GetPostsPagedAsync(int page, int pageSize);
    }
}
