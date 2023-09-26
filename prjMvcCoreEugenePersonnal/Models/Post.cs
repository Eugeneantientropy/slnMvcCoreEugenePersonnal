using System;
using System.Collections.Generic;

namespace prjMvcCoreEugenePersonnal.Models;

public partial class Post
{
    public int PostId { get; set; }

    public int? UserId { get; set; }

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public DateTime? DatePosted { get; set; }

    public DateTime? LastEdited { get; set; }

    public string? PostImageUrl { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual User? User { get; set; }
}
