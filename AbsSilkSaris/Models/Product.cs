using System.ComponentModel.DataAnnotations.Schema;

namespace AbsSilkSaris.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Sku { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public string Weave { get; set; } = string.Empty;
    public string Occasion { get; set; } = string.Empty;
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal? CompareAtPrice { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string? HoverImageUrl { get; set; }
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
    public int? SubCategoryId { get; set; }
    public SubCategory? SubCategory { get; set; }
    public bool IsNew { get; set; }
    public bool IsBestSeller { get; set; }
    public bool IsActive { get; set; } = true;
    public List<ProductImage> Images { get; set; } = new();
    public int Stock { get; set; } = 8;
    public double Rating { get; set; } = 4.8;
    public int ReviewCount { get; set; } = 12;

    public int DiscountPercent =>
        CompareAtPrice is > 0 && CompareAtPrice > Price
            ? (int)Math.Round((1 - Price / CompareAtPrice.Value) * 100)
            : 0;
}
