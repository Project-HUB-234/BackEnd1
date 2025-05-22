using System;
using System.Collections.Generic;

namespace Project_Hub.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Email { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? ProfilePicture { get; set; }
    public string? BackgroundPicture { get; set; }

    public DateTime? DateJoined { get; set; }

    public string? PhoneNumber { get; set; }

    public string? QuickAccessQrcode { get; set; }

    public int RoleId { get; set; }

    public virtual ICollection<CommentLike> CommentLikes { get; set; } = new List<CommentLike>();

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<Login> Logins { get; set; } = new List<Login>();

    public virtual ICollection<PostLike> PostLikes { get; set; } = new List<PostLike>();

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();

    public virtual Role Role { get; set; } = null!;
}
