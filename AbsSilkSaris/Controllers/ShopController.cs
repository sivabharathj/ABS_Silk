using AbsSilkSaris.Data;
using AbsSilkSaris.Models;
using AbsSilkSaris.Services;
using AbsSilkSaris.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AbsSilkSaris.Controllers;

public class ShopController : Controller
{
    private readonly CatalogRepository _db;

    public ShopController(CatalogRepository db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index(string? category, string? subcategory, string? color, string? q, string sort = "featured", decimal? min = null, decimal? max = null)
    {
        var products = await _db.SearchProductsAsync(category, subcategory, color, q, sort, min, max);
        var cats = await _db.GetCategoriesAsync(activeOnly: true);
        Category? selected = CollectionHelper.FindCategoryBySlug(cats, category);
        int? selectedId = null;
        if (selected != null)
        {
            selectedId = selected.Id;
        }
        var model = new ShopViewModel();
        model.Products = products;
        model.Categories = cats;
        model.SubCategories = await _db.GetSubCategoriesAsync(selectedId, menuOnly: false);
        model.Colors = await _db.GetColorsAsync();
        model.CategorySlug = category;
        model.SubCategorySlug = subcategory;
        model.Color = color;
        model.Query = q;
        model.Sort = sort;
        model.MinPrice = min;
        model.MaxPrice = max;
        model.TotalCount = products.Count;
        if (selected != null)
        {
            model.Heading = selected.Name;
            ViewData["Title"] = selected.Name;
        }
        else
        {
            model.Heading = "Shop Collection";
            ViewData["Title"] = "Shop Collection";
        }
        return View(model);
    }

    public async Task<IActionResult> Details(string slug)
    {
        var product = await _db.GetProductBySlugAsync(slug);
        if (product is null)
        {
            return NotFound();
        }
        product.Images = await _db.GetProductImagesAsync(product.Id);
        string? catSlug = null;
        if (product.Category != null)
        {
            catSlug = product.Category.Slug;
        }
        var relatedSource = await _db.SearchProductsAsync(catSlug, null, null, null, "featured", null, null);
        ViewBag.Related = CollectionHelper.RelatedProducts(relatedSource, product.Id, 4);
        ViewData["Title"] = product.Name;
        return View(product);
    }

    public async Task<IActionResult> QuickView(int id)
    {
        var product = await _db.GetProductByIdAsync(id);
        if (product is null)
        {
            return NotFound();
        }
        return PartialView("_QuickView", product);
    }
}
