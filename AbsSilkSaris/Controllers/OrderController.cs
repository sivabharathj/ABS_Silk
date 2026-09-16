using AbsSilkSaris.Data;
using Microsoft.AspNetCore.Mvc;

namespace AbsSilkSaris.Controllers;

public class OrderController : Controller
{
    private readonly CatalogRepository _db;
    public OrderController(CatalogRepository db) => _db = db;

    public IActionResult Track()
    {
        ViewData["Title"] = "Track your order";
        return View();
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Track(string orderNumber, string email)
    {
        ViewData["Title"] = "Track your order";
        var order = await _db.GetOrderByNumberAsync(orderNumber, email);
        if (order is null)
        {
            ViewBag.Error = "We could not find an order with that number and email.";
            return View();
        }
        return View(order);
    }
}
