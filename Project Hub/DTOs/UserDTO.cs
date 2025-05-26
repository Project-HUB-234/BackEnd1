namespace Project_Hub.DTOs
{
    public class UserDTO
    {
        public int UserId { get; set; }

        public string Email { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string? ProfilePicture { get; set; }

        public string? PhoneNumber { get; set; }
        public string? Brif { get; set; }
        public DateTime? DateJoined { get; set; }

        public string? QuickAccessQrcode { get; set; }
        public string? BackgroundPicture { get; set; }

        public string? Address { get; set; }
        public string? Job { get; set; }

    }
}
