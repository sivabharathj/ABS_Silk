namespace AbsSilkSaris.Models;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public int SortOrder { get; set; }
    public bool IsActive { get; set; } = true;
    public bool ShowInMenu { get; set; } = true;
    public ICollection<Product> Products { get; set; } = new List<Product>();
    public List<SubCategory> SubCategories { get; set; } = new();
}
