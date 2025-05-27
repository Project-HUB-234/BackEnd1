namespace Project_Hub.DTOs
{
    public class UpdateCommentDTO
    {
        public int CommentId { get; set; }
        public string Content { get; set; } = null!;
        public IFormFile ? Image { get; set; }

        public bool RemoveImage { get; set; }

    }
}
