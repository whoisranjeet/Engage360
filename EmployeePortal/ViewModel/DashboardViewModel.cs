using EmployeePortal.Core.DTOs;

namespace EmployeePortal.ViewModel
{
    public class DashboardViewModel
    {
        public PostDto CreatePost { get; set; }
        public IFormFile ImageUpload { get; set; }
        public List<PostDto> Posts { get; set; }
    }
}
