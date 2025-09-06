using EmployeePortal.Core.DTOs;

namespace EmployeePortal.ViewModel
{
    public class CreatePostViewModel
    {
        public PostDto Post { get; set; }
        public IFormFile Image { get; set; }
    }
}
