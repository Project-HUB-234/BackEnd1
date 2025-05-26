namespace Project_Hub.DTOs
{
    public class AddCommentDTO
    {
        public int UserId { get; set; }
        public string Contant { get; set; }
        public int PostId { get; set; }
        public IFormFile? image { get; set; }
    }
}
