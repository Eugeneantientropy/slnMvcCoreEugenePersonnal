﻿using System;
using System.Collections.Generic;

namespace prjMvcCoreEugenePersonnal.Models;

public partial class OrderItem
{
    public int OrderItemId { get; set; }

    public int? OrderId { get; set; }

    public int? ProductId { get; set; }

    public int Quantity { get; set; }

    public decimal PriceAtPurchase { get; set; }

    public string? ProductName { get; set; }

    public virtual Order? Order { get; set; }

    public virtual Product? Product { get; set; }
}
