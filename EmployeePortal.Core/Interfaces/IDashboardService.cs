using EmployeePortal.Core.DTOs;

namespace EmployeePortal.Core.Interfaces
{
    public interface IDashboardService
    {
        bool CreatePost(PostDto postDto);
        bool DeletePost(Guid id);
        IEnumerable<PostDto> GetPostsPaged(int page, int pageSize);
    }
}
