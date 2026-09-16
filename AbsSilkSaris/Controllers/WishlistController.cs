using AbsSilkSaris.Data;
using AbsSilkSaris.Services;
using Microsoft.AspNetCore.Mvc;

namespace AbsSilkSaris.Controllers;

public class WishlistController : Controller
{
    private readonly CatalogRepository _db;
    private readonly SessionCartService _cart;
    public WishlistController(CatalogRepository db, SessionCartService cart) { _db = db; _cart = cart; }

    public IActionResult Index()
    {
        ViewData["Title"] = "Wishlist";
        return View(_cart.GetWishlist());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Toggle(int id)
    {
        var product = await _db.GetProductByIdAsync(id);
        if (product is null) return NotFound();
        _cart.ToggleWishlist(product);
        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            return Json(new { ok = true, count = _cart.WishlistCount });
        return RedirectToAction(nameof(Index));
    }
}
