using System;
using System.Collections.Generic;

namespace DemWpf.Models;

public partial class PickUpPoint
{
    public int PicUpPointId { get; set; }

    public string Address { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
