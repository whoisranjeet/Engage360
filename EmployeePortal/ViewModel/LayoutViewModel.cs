using EmployeePortal.Core.DTOs;

namespace EmployeePortal.ViewModel
{
    public class LayoutViewModel
    {
        public PostDto Post { get; set; }
        public List<PostDto> Posts { get; set; }
        public IFormFile Image { get; set; }        
        public UserDto User { get; set; }
        public EmployeeDto Employee { get; set; }
    }
}
