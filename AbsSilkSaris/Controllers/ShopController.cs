using AbsSilkSaris.Data;
using AbsSilkSaris.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AbsSilkSaris.Controllers;

public class ShopController : Controller
{
    private readonly CatalogRepository _db;
    public ShopController(CatalogRepository db) => _db = db;

    public async Task<IActionResult> Index(string? category, string? subcategory, string? color, string? q, string sort = "featured", decimal? min = null, decimal? max = null)
    {
        var products = await _db.SearchProductsAsync(category, subcategory, color, q, sort, min, max);
        var cats = await _db.GetCategoriesAsync(activeOnly: true);
        var selected = cats.FirstOrDefault(c => c.Slug == category);
        var model = new ShopViewModel
        {
            Products = products,
            Categories = cats,
            SubCategories = await _db.GetSubCategoriesAsync(selected?.Id, menuOnly: false),
            Colors = await _db.GetColorsAsync(),
            CategorySlug = category,
            SubCategorySlug = subcategory,
            Color = color,
            Query = q,
            Sort = sort,
            MinPrice = min,
            MaxPrice = max,
            TotalCount = products.Count
        };
        ViewData["Title"] = selected?.Name ?? (string.IsNullOrWhiteSpace(subcategory) ? "Shop Collection" : products.FirstOrDefault()?.SubCategory?.Name ?? "Collection");
        return View(model);
    }

    public async Task<IActionResult> Details(string slug)
    {
        var product = await _db.GetProductBySlugAsync(slug);
        if (product is null) return NotFound();
        product.Images = await _db.GetProductImagesAsync(product.Id);
        var related = (await _db.SearchProductsAsync(product.Category?.Slug, null, null, null, "featured", null, null))
            .Where(p => p.Id != product.Id).Take(4).ToList();
        ViewBag.Related = related;
        ViewData["Title"] = product.Name;
        return View(product);
    }

    public async Task<IActionResult> QuickView(int id)
    {
        var product = await _db.GetProductByIdAsync(id);
        if (product is null) return NotFound();
        return PartialView("_QuickView", product);
    }
}
