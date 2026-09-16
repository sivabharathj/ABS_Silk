using AbsSilkSaris.Data;
using AbsSilkSaris.Models;
using AbsSilkSaris.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AbsSilkSaris.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class CategoriesController : Controller
{
    private readonly CatalogRepository _db;
    private readonly ImageUploadService _images;
    public CategoriesController(CatalogRepository db, ImageUploadService images) { _db = db; _images = images; }

    public async Task<IActionResult> Index() => View(await _db.GetCategoriesAsync());

    public IActionResult Create() => View(new Category());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Category model, IFormFile? image)
    {
        if (string.IsNullOrWhiteSpace(model.Name)) { ModelState.AddModelError("Name", "Required"); return View(model); }
        if (string.IsNullOrWhiteSpace(model.Slug)) model.Slug = SlugHelper.Slugify(model.Name);
        var url = await _images.SaveAsync(image, "categories");
        if (url is not null) model.ImageUrl = url;
        await _db.InsertCategoryAsync(model);
        TempData["Notice"] = "Category saved and will appear in the top menu if Show in menu is on.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var item = await _db.GetCategoryAsync(id);
        return item is null ? NotFound() : View("Create", item);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Category model, IFormFile? image)
    {
        if (string.IsNullOrWhiteSpace(model.Slug)) model.Slug = SlugHelper.Slugify(model.Name);
        var url = await _images.SaveAsync(image, "categories");
        if (url is not null) model.ImageUrl = url;
        await _db.UpdateCategoryAsync(model);
        TempData["Notice"] = "Category updated.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _db.DeleteCategoryAsync(id);
        TempData["Notice"] = "Category removed.";
        return RedirectToAction(nameof(Index));
    }
}
