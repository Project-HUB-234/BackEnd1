namespace Project_Hub.DTOs
{
    public class UpdatePassword
    {

        public string OldPassword { get; set; }
        public int UserId { get; set; }
        public string NewPassword { get; set; }

    }
}
