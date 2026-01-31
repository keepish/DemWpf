using System;
using System.Collections.Generic;
using System.IO;

namespace DemWpf.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string Article { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Unit { get; set; } = null!;

    public decimal Price { get; set; }

    public int SupplierId { get; set; }

    public int ManufacturerId { get; set; }

    public int CategoryId { get; set; }

    public int Discount { get; set; }

    public int Amount { get; set; }

    public string Description { get; set; } = null!;

    public string? Photo { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual Manufacturer Manufacturer { get; set; } = null!;

    public virtual ICollection<OrderedProduct> OrderedProducts { get; set; } = new List<OrderedProduct>();

    public virtual Supplier Supplier { get; set; } = null!;

    public string PhotoPath
    {
        get
        {
            string root = Environment.CurrentDirectory;
            string path = string.IsNullOrWhiteSpace(Photo)
                ? Path.Combine(root, "Images", "picture.png")
                : Path.Combine(root, "Images", Photo);
            return path;
        }
    }

    public bool IsBigDiscount => Discount > 15;

    public bool HasDiscount => Discount > 0;

    public decimal DiscountedPrice => Price * (1 - Discount / 100M);
}
