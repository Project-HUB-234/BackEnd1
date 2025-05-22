namespace Project_Hub.DTOs
{
    public class UpdateUserDTO
    {
        public int UserId{ get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public IFormFile? ProfilePicture { get; set; }
        public IFormFile? QuickAccessQrcode { get; set; }
        public IFormFile? BackgroundPicture { get; set; }
    }
}
