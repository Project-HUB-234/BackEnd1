using System;
using System.Collections.Generic;

namespace Project_Hub.Models;

public partial class CommentLike
{
    public int LikeId { get; set; }

    public int? CommentId { get; set; }

    public int? UserId { get; set; }

    public DateTime? DateLiked { get; set; }

    public virtual Comment? Comment { get; set; }

    public virtual User? User { get; set; }
}
