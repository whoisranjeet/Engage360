namespace EmployeePortal.Core.DTOs
{
    public class PostDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public byte[] ImageData { get; set; }
        public string Author { get; set; }
        public DateTime DateOfPublishing { get; set; }
    }
}
