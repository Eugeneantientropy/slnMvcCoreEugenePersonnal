using System;
using System.Collections.Generic;

namespace prjMvcCoreEugenePersonnal.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int? UserId { get; set; }

    public decimal TotalPrice { get; set; }

    public DateTime? DateOrdered { get; set; }

    public string ShippingAddress { get; set; } = null!;

    public string OrderStatus { get; set; } = null!;

    public string? EcpayId { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual User? User { get; set; }
}
