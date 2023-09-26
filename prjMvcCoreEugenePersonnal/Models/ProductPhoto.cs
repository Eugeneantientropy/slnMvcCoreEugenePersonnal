using System;
using System.Collections.Generic;

namespace prjMvcCoreEugenePersonnal.Models;

public partial class ProductPhoto
{
    public int ProductPhotoId { get; set; }

    public string ProductUrl { get; set; }

    public int? ProductId { get; set; }

    public virtual Product? Product { get; set; }
}
