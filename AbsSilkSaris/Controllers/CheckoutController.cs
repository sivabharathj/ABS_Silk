using AbsSilkSaris.Data;
using AbsSilkSaris.Models;
using AbsSilkSaris.Services;
using AbsSilkSaris.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AbsSilkSaris.Controllers;

public class CheckoutController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly SessionCartService _cart;

    public CheckoutController(ApplicationDbContext db, SessionCartService cart)
    {
        _db = db;
        _cart = cart;
    }

    public IActionResult Index()
    {
        var lines = _cart.GetCart();
        if (lines.Count == 0) return RedirectToAction("Index", "Cart");
        var sub = lines.Sum(l => l.Price * l.Quantity);
        var ship = sub >= 15000 ? 0 : 350;
        ViewData["Title"] = "Secure Checkout";
        return View(new CheckoutViewModel
        {
            Lines = lines,
            Subtotal = sub,
            Shipping = ship,
            Total = sub + ship
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Place(CheckoutViewModel model)
    {
        var lines = _cart.GetCart();
        if (lines.Count == 0) return RedirectToAction("Index", "Cart");
        if (string.IsNullOrWhiteSpace(model.CustomerName) || string.IsNullOrWhiteSpace(model.Email))
        {
            TempData["Notice"] = "Please share your name and email so we can confirm the order.";
            return RedirectToAction(nameof(Index));
        }

        var sub = lines.Sum(l => l.Price * l.Quantity);
        var ship = sub >= 15000 ? 0 : 350;
        var order = new Order
        {
            OrderNumber = $"ABS{DateTime.UtcNow:yyMMddHHmmss}",
            CustomerName = model.CustomerName,
            Email = model.Email,
            Phone = model.Phone,
            Address = model.Address,
            City = model.City,
            State = model.State,
            Pincode = model.Pincode,
            Status = "Confirmed",
            Subtotal = sub,
            Shipping = ship,
            Total = sub + ship,
            Items = lines.Select(l => new OrderItem
            {
                ProductId = l.ProductId,
                ProductName = l.Name,
                Quantity = l.Quantity,
                UnitPrice = l.Price
            }).ToList()
        };
        _db.Orders.Add(order);
        await _db.SaveChangesAsync();
        _cart.ClearCart();
        return RedirectToAction(nameof(Success), new { id = order.OrderNumber });
    }

    public IActionResult Success(string id)
    {
        var order = _db.Orders.FirstOrDefault(o => o.OrderNumber == id);
        if (order is null) return RedirectToAction("Index", "Home");
        ViewData["Title"] = "Order confirmed";
        return View(order);
    }
}
