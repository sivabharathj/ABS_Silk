using AbsSilkSaris.Data;
using AbsSilkSaris.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AbsSilkSaris.Controllers;

public class ShopController : Controller
{
    private readonly ApplicationDbContext _db;

    public ShopController(ApplicationDbContext db) => _db = db;

    public async Task<IActionResult> Index(string? category, string? color, string? q, string sort = "featured", decimal? min = null, decimal? max = null)
    {
        var query = _db.Products.AsNoTracking().Include(p => p.Category).AsQueryable();
        if (!string.IsNullOrWhiteSpace(category))
        {
            query = query.Where(p => p.Category!.Slug == category);
        }
        if (!string.IsNullOrWhiteSpace(color))
        {
            query = query.Where(p => p.Color == color);
        }
        if (!string.IsNullOrWhiteSpace(q))
        {
            query = query.Where(p => p.Name.Contains(q) || p.Color.Contains(q) || p.Weave.Contains(q) || p.Sku.Contains(q));
        }
        if (min is not null) query = query.Where(p => p.Price >= min);
        if (max is not null) query = query.Where(p => p.Price <= max);

        query = sort switch
        {
            "price-asc" => query.OrderBy(p => p.Price),
            "price-desc" => query.OrderByDescending(p => p.Price),
            "newest" => query.OrderByDescending(p => p.Id),
            _ => query.OrderByDescending(p => p.IsBestSeller).ThenByDescending(p => p.IsNew)
        };

        var products = await query.ToListAsync();
        var model = new ShopViewModel
        {
            Products = products,
            Categories = await _db.Categories.AsNoTracking().OrderBy(c => c.SortOrder).ToListAsync(),
            Colors = await _db.Products.AsNoTracking().Select(p => p.Color).Distinct().OrderBy(c => c).ToListAsync(),
            CategorySlug = category,
            Color = color,
            Query = q,
            Sort = sort,
            MinPrice = min,
            MaxPrice = max,
            TotalCount = products.Count
        };
        ViewData["Title"] = string.IsNullOrWhiteSpace(category) ? "Shop Collection" : products.FirstOrDefault()?.Category?.Name ?? "Collection";
        return View(model);
    }

    public async Task<IActionResult> Details(string slug)
    {
        var product = await _db.Products.AsNoTracking().Include(p => p.Category).FirstOrDefaultAsync(p => p.Slug == slug);
        if (product is null) return NotFound();
        ViewBag.Related = await _db.Products.AsNoTracking().Include(p => p.Category)
            .Where(p => p.CategoryId == product.CategoryId && p.Id != product.Id)
            .Take(4).ToListAsync();
        ViewData["Title"] = product.Name;
        return View(product);
    }

    public async Task<IActionResult> QuickView(int id)
    {
        var product = await _db.Products.AsNoTracking().Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);
        if (product is null) return NotFound();
        return PartialView("_QuickView", product);
    }
}
