using AbsSilkSaris.Data;
using AbsSilkSaris.Models;
using AbsSilkSaris.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AbsSilkSaris.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class SubCategoriesController : Controller
{
    private readonly CatalogRepository _db;
    private readonly ImageUploadService _images;
    public SubCategoriesController(CatalogRepository db, ImageUploadService images) { _db = db; _images = images; }

    public async Task<IActionResult> Index()
    {
        ViewBag.Categories = await _db.GetCategoriesAsync();
        return View(await _db.GetSubCategoriesAsync());
    }

    public async Task<IActionResult> Create()
    {
        ViewBag.Categories = await _db.GetCategoriesAsync();
        return View(new SubCategory());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SubCategory model, IFormFile? image)
    {
        ViewBag.Categories = await _db.GetCategoriesAsync();
        if (string.IsNullOrWhiteSpace(model.Name) || model.CategoryId == 0)
        {
            ModelState.AddModelError("Name", "Name and parent category are required.");
            return View(model);
        }
        if (string.IsNullOrWhiteSpace(model.Slug)) model.Slug = SlugHelper.Slugify(model.Name);
        var url = await _images.SaveAsync(image, "subcategories");
        if (url is not null) model.ImageUrl = url;
        await _db.InsertSubCategoryAsync(model);
        TempData["Notice"] = "Subcategory saved. Enable Show in menu to list it under its category in the top nav.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var item = await _db.GetSubCategoryAsync(id);
        if (item is null) return NotFound();
        ViewBag.Categories = await _db.GetCategoriesAsync();
        return View("Create", item);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(SubCategory model, IFormFile? image)
    {
        if (string.IsNullOrWhiteSpace(model.Slug)) model.Slug = SlugHelper.Slugify(model.Name);
        var url = await _images.SaveAsync(image, "subcategories");
        if (url is not null) model.ImageUrl = url;
        await _db.UpdateSubCategoryAsync(model);
        TempData["Notice"] = "Subcategory updated.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _db.DeleteSubCategoryAsync(id);
        TempData["Notice"] = "Subcategory removed.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> ByCategory(int categoryId) =>
        Json(await _db.GetSubCategoriesAsync(categoryId));
}
