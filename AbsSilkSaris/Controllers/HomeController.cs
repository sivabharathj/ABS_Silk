using System.Diagnostics;
using AbsSilkSaris.Data;
using AbsSilkSaris.Models;
using AbsSilkSaris.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AbsSilkSaris.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _db;

    public HomeController(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index()
    {
        var model = new HomeViewModel
        {
            Categories = await _db.Categories.AsNoTracking().OrderBy(c => c.SortOrder).ToListAsync(),
            NewArrivals = await _db.Products.AsNoTracking().Include(p => p.Category).Where(p => p.IsNew).OrderByDescending(p => p.Id).Take(8).ToListAsync(),
            BestSellers = await _db.Products.AsNoTracking().Include(p => p.Category).Where(p => p.IsBestSeller).Take(10).ToListAsync(),
            Reviews = await _db.Reviews.AsNoTracking().ToListAsync()
        };
        ViewData["Title"] = "Timeless Elegance in Every Weave";
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

    public IActionResult Privacy()
    {
        ViewData["Title"] = "Privacy Policy";
        return View();
    }

    public IActionResult Terms()
    {
        ViewData["Title"] = "Terms & Conditions";
        return View();
    }

    public IActionResult Shipping()
    {
        ViewData["Title"] = "Shipping Policy";
        return View();
    }

    public IActionResult Returns()
    {
        ViewData["Title"] = "Return Policy";
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Newsletter(string email)
    {
        if (!string.IsNullOrWhiteSpace(email))
        {
            var exists = await _db.NewsletterSubscribers.AnyAsync(n => n.Email == email);
            if (!exists)
            {
                _db.NewsletterSubscribers.Add(new NewsletterSubscriber { Email = email.Trim() });
                await _db.SaveChangesAsync();
            }
            TempData["Notice"] = "Welcome to ABS Threads — new weaves and private previews, delivered with care.";
        }
        return RedirectToAction(nameof(Index));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
