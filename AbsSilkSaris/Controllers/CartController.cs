using AbsSilkSaris.Data;
using AbsSilkSaris.Services;
using Microsoft.AspNetCore.Mvc;

namespace AbsSilkSaris.Controllers;

public class CartController : Controller
{
    private readonly CatalogRepository _db;
    private readonly SessionCartService _cart;
    public CartController(CatalogRepository db, SessionCartService cart) { _db = db; _cart = cart; }

    public IActionResult Index()
    {
        ViewData["Title"] = "Shopping Cart";
        return View(_cart.GetCart());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(int id, int qty = 1)
    {
        var product = await _db.GetProductByIdAsync(id);
        if (product is null) return NotFound();
        _cart.AddToCart(product, qty);
        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            return Json(new { ok = true, count = _cart.CartCount, message = "Added to your atelier cart." });
        TempData["Notice"] = $"{product.Name} has been added to your cart.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult Update(int id, int qty)
    {
        var cart = _cart.GetCart();
        var line = cart.FirstOrDefault(x => x.ProductId == id);
        if (line is not null)
        {
            if (qty <= 0) cart.Remove(line); else line.Quantity = qty;
            _cart.SaveCart(cart);
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult Remove(int id)
    {
        _cart.SaveCart(_cart.GetCart().Where(x => x.ProductId != id).ToList());
        return RedirectToAction(nameof(Index));
    }
}
