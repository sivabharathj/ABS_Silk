using System.Diagnostics;
using AbsSilkSaris.Data;
using AbsSilkSaris.Models;
using AbsSilkSaris.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AbsSilkSaris.Controllers;

public class HomeController : Controller
{
    private readonly CatalogRepository _db;
    public HomeController(CatalogRepository db) => _db = db;

    public async Task<IActionResult> Index()
    {
        var settings = await _db.GetSettingsAsync();
        ViewBag.Settings = settings;
        var model = new HomeViewModel
        {
            Categories = await _db.GetCategoriesAsync(activeOnly: true),
            NewArrivals = (await _db.SearchProductsAsync(null, null, null, null, "newest", null, null, isNew: true)).Take(8).ToList(),
            BestSellers = (await _db.SearchProductsAsync(null, null, null, null, "featured", null, null, best: true)).Take(10).ToList(),
            Reviews = await _db.GetReviewsAsync()
        };
        ViewData["Title"] = settings.GetValueOrDefault("HeroHeading", "Timeless Elegance in Every Weave");
        return View(model);
    }

    public IActionResult About()
    {
        ViewData["Title"] = "Our Heritage";
        return View();
    }

    public IActionResult Contact()
    {
        ViewData["Title"] = "Contact";
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Contact(string name, string email, string message)
    {
        TempData["Notice"] = "Thank you. Our concierge will write back within one working day.";
        return RedirectToAction(nameof(Contact));
    }

    public IActionResult Privacy() { ViewData["Title"] = "Privacy Policy"; return View(); }
    public IActionResult Terms() { ViewData["Title"] = "Terms & Conditions"; return View(); }
    public IActionResult Shipping() { ViewData["Title"] = "Shipping Policy"; return View(); }
    public IActionResult Returns() { ViewData["Title"] = "Return Policy"; return View(); }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Newsletter(string email)
    {
        if (!string.IsNullOrWhiteSpace(email))
        {
            await _db.SubscribeAsync(email.Trim());
            TempData["Notice"] = "Welcome to ABS Threads — new weaves and private previews, delivered with care.";
        }
        return RedirectToAction(nameof(Index));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
}
