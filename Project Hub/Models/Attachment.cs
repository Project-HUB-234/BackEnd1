namespace Project_Hub.Models;

public partial class Attachment
{
    public int AttachmentId { get; set; }

    public string AttachmentPath { get; set; } = null!;

    public DateTime? CreateDate { get; set; }

    public int? PostId { get; set; }

    public int? CommentId { get; set; }

    public virtual Comment? Comment { get; set; }

    public virtual Post? Post { get; set; }
}
