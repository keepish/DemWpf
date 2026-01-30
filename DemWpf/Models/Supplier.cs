namespace DemWpf.Models;

public partial class Supplier
{
    public int SupplierId { get; set; }

    public string Supplier1 { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
