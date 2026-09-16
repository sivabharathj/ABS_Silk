using AbsSilkSaris.Models;
using System.Text.Json;

namespace AbsSilkSaris.Services;

public class SessionCartService
{
    private const string CartKey = "ABS_CART";
    private const string WishKey = "ABS_WISH";
    private readonly IHttpContextAccessor _http;

    public SessionCartService(IHttpContextAccessor http) => _http = http;

    private ISession Session => _http.HttpContext!.Session;

    public List<CartLine> GetCart() =>
        JsonSerializer.Deserialize<List<CartLine>>(Session.GetString(CartKey) ?? "[]") ?? new();

    public List<WishlistLine> GetWishlist() =>
        JsonSerializer.Deserialize<List<WishlistLine>>(Session.GetString(WishKey) ?? "[]") ?? new();

    public void SaveCart(List<CartLine> lines) =>
        Session.SetString(CartKey, JsonSerializer.Serialize(lines));

    public void SaveWishlist(List<WishlistLine> lines) =>
        Session.SetString(WishKey, JsonSerializer.Serialize(lines));

    public int CartCount => GetCart().Sum(x => x.Quantity);
    public int WishlistCount => GetWishlist().Count;

    public void AddToCart(Product product, int qty = 1)
    {
        var cart = GetCart();
        var existing = cart.FirstOrDefault(x => x.ProductId == product.Id);
        if (existing is null)
        {
            cart.Add(new CartLine
            {
                ProductId = product.Id,
                Name = product.Name,
                ImageUrl = product.ImageUrl,
                Price = product.Price,
                Quantity = qty
            });
        }
        else
        {
            existing.Quantity += qty;
        }
        SaveCart(cart);
    }

    public void ToggleWishlist(Product product)
    {
        var list = GetWishlist();
        var existing = list.FirstOrDefault(x => x.ProductId == product.Id);
        if (existing is null)
        {
            list.Add(new WishlistLine
            {
                ProductId = product.Id,
                Name = product.Name,
                ImageUrl = product.ImageUrl,
                Price = product.Price
            });
        }
        else
        {
            list.Remove(existing);
        }
        SaveWishlist(list);
    }

    public void ClearCart() => SaveCart(new List<CartLine>());
}
