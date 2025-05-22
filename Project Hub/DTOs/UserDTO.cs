namespace Project_Hub.DTOs
{
    public class UserDTO
    {
        public string Email { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string? ProfilePicture { get; set; }

        public string? PhoneNumber { get; set; }

        public string? QuickAccessQrcode { get; set; }
        public string? BackgroundPicture { get; set; }
    }
}
