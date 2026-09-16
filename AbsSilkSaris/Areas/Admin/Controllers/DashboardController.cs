using AbsSilkSaris.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AbsSilkSaris.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class DashboardController : Controller
{
    private readonly CatalogRepository _db;
    public DashboardController(CatalogRepository db) => _db = db;

    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Dashboard";
        var (categories, subCategories, products, orders) = await _db.DashboardCountsAsync();
        ViewBag.Categories = categories;
        ViewBag.SubCategories = subCategories;
        ViewBag.Products = products;
        ViewBag.OrderCount = orders;
        ViewBag.Orders = await _db.ListOrdersAsync();
        return View();
    }
}
