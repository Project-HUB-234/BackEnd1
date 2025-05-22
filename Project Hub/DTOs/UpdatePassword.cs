namespace Project_Hub.DTOs
{
    public class UpdatePassword
    {

        public string? OldPassword { get; set; }
        public int? OTP { get; set; }
        public string Email { get; set; }
        public string NewPassword { get; set; }

    }
}
