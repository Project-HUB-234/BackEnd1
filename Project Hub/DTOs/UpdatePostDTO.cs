namespace Project_Hub.DTOs
{
    public class UpdatePostDTO
    {
        public int PostId { get; set; }

        public string Content { get; set; }

        public int PostCategory { get; set; }
        public List<IFormFile> PostPictures = new List<IFormFile>();

    }
}
