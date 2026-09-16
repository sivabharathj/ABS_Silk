using AbsSilkSaris.Data;
using AbsSilkSaris.Models;
using AbsSilkSaris.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AbsSilkSaris.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class ProductsController : Controller
{
    private readonly CatalogRepository _db;
    private readonly ImageUploadService _images;
    public ProductsController(CatalogRepository db, ImageUploadService images) { _db = db; _images = images; }

    public async Task<IActionResult> Index() => View(await _db.ListAllProductsAsync());

    public async Task<IActionResult> Create()
    {
        await FillLookups();
        return View(new Product { IsActive = true, Stock = 8, Rating = 4.8 });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Product model, IFormFile? primaryImage, List<IFormFile>? gallery)
    {
        await FillLookups();
        if (string.IsNullOrWhiteSpace(model.Name) || model.CategoryId == 0)
        {
            ModelState.AddModelError("Name", "Name and category are required.");
            return View(model);
        }
        if (string.IsNullOrWhiteSpace(model.Slug)) model.Slug = SlugHelper.Slugify(model.Sku.Length > 0 ? model.Sku : model.Name);
        if (string.IsNullOrWhiteSpace(model.Sku)) model.Sku = "ABS-" + Guid.NewGuid().ToString("N")[..6].ToUpperInvariant();
        var url = await _images.SaveAsync(primaryImage, "products");
        if (url is not null) model.ImageUrl = url;
        if (model.SubCategoryId == 0) model.SubCategoryId = null;
        var id = await _db.InsertProductAsync(model);
        if (!string.IsNullOrWhiteSpace(model.ImageUrl))
            await _db.AddProductImageAsync(id, model.ImageUrl, 0, true);
        if (gallery is not null)
        {
            var i = 1;
            foreach (var file in gallery.Where(f => f.Length > 0))
            {
                var g = await _images.SaveAsync(file, "products");
                if (g is not null) await _db.AddProductImageAsync(id, g, i++, false);
            }
        }
        TempData["Notice"] = "Product published.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var item = await _db.GetProductByIdAsync(id);
        if (item is null) return NotFound();
        item.Images = await _db.GetProductImagesAsync(id);
        await FillLookups();
        return View(item);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Product model, IFormFile? primaryImage, List<IFormFile>? gallery)
    {
        if (string.IsNullOrWhiteSpace(model.Slug)) model.Slug = SlugHelper.Slugify(model.Name);
        var url = await _images.SaveAsync(primaryImage, "products");
        if (url is not null) model.ImageUrl = url;
        if (model.SubCategoryId == 0) model.SubCategoryId = null;
        await _db.UpdateProductAsync(model);
        if (url is not null) await _db.AddProductImageAsync(model.Id, url, 0, true);
        if (gallery is not null)
        {
            foreach (var file in gallery.Where(f => f.Length > 0))
            {
                var g = await _images.SaveAsync(file, "products");
                if (g is not null) await _db.AddProductImageAsync(model.Id, g, 10, false);
            }
        }
        TempData["Notice"] = "Product updated.";
        return RedirectToAction(nameof(Edit), new { id = model.Id });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteImage(int id, int productId)
    {
        await _db.DeleteProductImageAsync(id);
        return RedirectToAction(nameof(Edit), new { id = productId });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _db.DeleteProductAsync(id);
        TempData["Notice"] = "Product removed.";
        return RedirectToAction(nameof(Index));
    }

    private async Task FillLookups()
    {
        ViewBag.Categories = await _db.GetCategoriesAsync();
        ViewBag.SubCategories = await _db.GetSubCategoriesAsync();
    }
}
