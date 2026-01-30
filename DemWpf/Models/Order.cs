using System;
using System.Collections.Generic;

namespace DemWpf.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public DateTime OrderDate { get; set; }

    public DateTime DeliveryDate { get; set; }

    public int PickUpPointId { get; set; }

    public int UserId { get; set; }

    public double Code { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<OrderedProduct> OrderedProducts { get; set; } = new List<OrderedProduct>();

    public virtual PickUpPoint PickUpPoint { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
