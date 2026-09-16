using AbsSilkSaris.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AbsSilkSaris.Controllers;

public class OrderController : Controller
{
    private readonly ApplicationDbContext _db;
    public OrderController(ApplicationDbContext db) => _db = db;

    public IActionResult Track()
    {
        ViewData["Title"] = "Track your order";
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Track(string orderNumber, string email)
    {
        ViewData["Title"] = "Track your order";
        var order = await _db.Orders.Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.OrderNumber == orderNumber && o.Email == email);
        if (order is null)
        {
            ViewBag.Error = "We could not find an order with that number and email.";
            return View();
        }
        return View(order);
    }
}
