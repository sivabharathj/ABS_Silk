using AbsSilkSaris.Models;
using System.Text.Json;

namespace AbsSilkSaris.Services;

public class SessionCartService
{
    private const string CartKey = "ABS_CART";
    private const string WishKey = "ABS_WISH";
    private readonly IHttpContextAccessor _http;

    public SessionCartService(IHttpContextAccessor http)
    {
        _http = http;
    }

    private ISession Session
    {
        get { return _http.HttpContext!.Session; }
    }

    public List<CartLine> GetCart()
    {
        string json = Session.GetString(CartKey);
        if (string.IsNullOrEmpty(json))
        {
            return new List<CartLine>();
        }
        List<CartLine>? list = JsonSerializer.Deserialize<List<CartLine>>(json);
        if (list == null)
        {
            return new List<CartLine>();
        }
        return list;
    }

    public List<WishlistLine> GetWishlist()
    {
        string json = Session.GetString(WishKey);
        if (string.IsNullOrEmpty(json))
        {
            return new List<WishlistLine>();
        }
        List<WishlistLine>? list = JsonSerializer.Deserialize<List<WishlistLine>>(json);
        if (list == null)
        {
            return new List<WishlistLine>();
        }
        return list;
    }

    public void SaveCart(List<CartLine> lines)
    {
        Session.SetString(CartKey, JsonSerializer.Serialize(lines));
    }

    public void SaveWishlist(List<WishlistLine> lines)
    {
        Session.SetString(WishKey, JsonSerializer.Serialize(lines));
    }

    public int CartCount
    {
        get { return CollectionHelper.CartQuantity(GetCart()); }
    }

    public int WishlistCount
    {
        get { return GetWishlist().Count; }
    }

    public void AddToCart(Product product, int qty = 1)
    {
        var cart = GetCart();
        var existing = CollectionHelper.FindCartLine(cart, product.Id);
        if (existing == null)
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
        var existing = CollectionHelper.FindWishLine(list, product.Id);
        if (existing == null)
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

    public void ClearCart()
    {
        SaveCart(new List<CartLine>());
    }
}
