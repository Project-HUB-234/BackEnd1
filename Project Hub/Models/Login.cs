namespace Project_Hub.Models;

public partial class Login
{
    public int LoginId { get; set; }

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public int UserId { get; set; }

    public virtual User User { get; set; } = null!;
}
