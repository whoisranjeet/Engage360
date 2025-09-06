using EmployeePortal.Core.Models;

namespace EmployeePortal.Core.Interfaces
{
    public interface IDashboardRepository
    {
        Task<bool> CreatePostAsync(Post post);
        Task<bool> DeletePostAsync(Guid id);
        Task<IEnumerable<Post>> GetPostsPagedAsync(int page, int pageSize);
    }
}
