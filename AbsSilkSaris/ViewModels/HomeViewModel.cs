using AbsSilkSaris.Models;

namespace AbsSilkSaris.ViewModels;

public class HomeViewModel
{
    public List<Category> Categories { get; set; } = new();
    public List<Product> NewArrivals { get; set; } = new();
    public List<Product> BestSellers { get; set; } = new();
    public List<Review> Reviews { get; set; } = new();
}

public class ShopViewModel
{
    public List<Product> Products { get; set; } = new();
    public List<Category> Categories { get; set; } = new();
    public List<string> Colors { get; set; } = new();
    public List<SubCategory> SubCategories { get; set; } = new();
    public string? CategorySlug { get; set; }
    public string? SubCategorySlug { get; set; }
    public string? Color { get; set; }
    public string? Query { get; set; }
    public string Sort { get; set; } = "featured";
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public int TotalCount { get; set; }
}

public class CheckoutViewModel
{
    public string CustomerName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = "Tamil Nadu";
    public string Pincode { get; set; } = string.Empty;
    public List<CartLine> Lines { get; set; } = new();
    public decimal Subtotal { get; set; }
    public decimal Shipping { get; set; }
    public decimal Total { get; set; }
}
