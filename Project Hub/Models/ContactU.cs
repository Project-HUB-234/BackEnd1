using System;
using System.Collections.Generic;

namespace Project_Hub.Models;

public partial class ContactU
{
    public int ContactId { get; set; }

    public string UserEmail { get; set; } = null!;

    public string ContactTitle { get; set; } = null!;

    public string ContactMessage { get; set; } = null!;

    public DateTime? CreatedDate { get; set; }
}
