using EmployeePortal.Core.Models;

namespace EmployeePortal.Core.Interfaces
{
    public interface IDashboardRepository
    {
        bool CreatePost(Post post);
        bool DeletePost(Guid id);

    }
}
