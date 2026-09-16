using AbsSilkSaris.Data;
using AbsSilkSaris.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AbsSilkSaris.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class SettingsController : Controller
{
    private readonly CatalogRepository _db;
    private readonly ImageUploadService _images;
    public SettingsController(CatalogRepository db, ImageUploadService images) { _db = db; _images = images; }

    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Site options";
        return View(await _db.GetSettingsAsync());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(IFormCollection form, IFormFile? hero1, IFormFile? hero2)
    {
        foreach (var key in form.Keys.Where(k => k != "__RequestVerificationToken"))
        {
            await _db.UpsertSettingAsync(key, form[key].ToString());
        }
        var h1 = await _images.SaveAsync(hero1, "hero");
        if (h1 is not null) await _db.UpsertSettingAsync("HeroImage1", h1);
        var h2 = await _images.SaveAsync(hero2, "hero");
        if (h2 is not null) await _db.UpsertSettingAsync("HeroImage2", h2);
        TempData["Notice"] = "Site options saved. The storefront updates immediately.";
        return RedirectToAction(nameof(Index));
    }
}
