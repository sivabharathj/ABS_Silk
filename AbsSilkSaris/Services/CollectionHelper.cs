using AbsSilkSaris.Models;

namespace AbsSilkSaris.Services;

public static class CollectionHelper
{
    public static List<T> TakeFirst<T>(List<T> source, int count)
    {
        var result = new List<T>();
        if (source == null)
        {
            return result;
        }
        int n = source.Count;
        if (n > count)
        {
            n = count;
        }
        for (int i = 0; i < n; i++)
        {
            result.Add(source[i]);
        }
        return result;
    }

    public static Category? FindCategoryBySlug(List<Category> list, string? slug)
    {
        if (list == null || string.IsNullOrWhiteSpace(slug))
        {
            return null;
        }
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].Slug == slug)
            {
                return list[i];
            }
        }
        return null;
    }

    public static CartLine? FindCartLine(List<CartLine> list, int productId)
    {
        if (list == null)
        {
            return null;
        }
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].ProductId == productId)
            {
                return list[i];
            }
        }
        return null;
    }

    public static WishlistLine? FindWishLine(List<WishlistLine> list, int productId)
    {
        if (list == null)
        {
            return null;
        }
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].ProductId == productId)
            {
                return list[i];
            }
        }
        return null;
    }

    public static List<CartLine> RemoveCartLine(List<CartLine> list, int productId)
    {
        var result = new List<CartLine>();
        if (list == null)
        {
            return result;
        }
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].ProductId != productId)
            {
                result.Add(list[i]);
            }
        }
        return result;
    }

    public static decimal CartSubtotal(List<CartLine> list)
    {
        decimal total = 0;
        if (list == null)
        {
            return total;
        }
        for (int i = 0; i < list.Count; i++)
        {
            total += list[i].Price * list[i].Quantity;
        }
        return total;
    }

    public static int CartQuantity(List<CartLine> list)
    {
        int total = 0;
        if (list == null)
        {
            return total;
        }
        for (int i = 0; i < list.Count; i++)
        {
            total += list[i].Quantity;
        }
        return total;
    }

    public static List<OrderItem> ToOrderItems(List<CartLine> list)
    {
        var items = new List<OrderItem>();
        if (list == null)
        {
            return items;
        }
        for (int i = 0; i < list.Count; i++)
        {
            CartLine line = list[i];
            OrderItem item = new OrderItem();
            item.ProductId = line.ProductId;
            item.ProductName = line.Name;
            item.Quantity = line.Quantity;
            item.UnitPrice = line.Price;
            items.Add(item);
        }
        return items;
    }

    public static List<Product> RelatedProducts(List<Product> source, int excludeId, int count)
    {
        var result = new List<Product>();
        if (source == null)
        {
            return result;
        }
        for (int i = 0; i < source.Count; i++)
        {
            if (source[i].Id == excludeId)
            {
                continue;
            }
            result.Add(source[i]);
            if (result.Count >= count)
            {
                break;
            }
        }
        return result;
    }

    public static string StarText(int stars)
    {
        string text = "";
        for (int i = 0; i < stars; i++)
        {
            text += "★";
        }
        return text;
    }

    public static bool HasItems<T>(IList<T>? list)
    {
        return list != null && list.Count > 0;
    }

    public static string GetSetting(Dictionary<string, string>? model, string key, string fallback)
    {
        if (model == null)
        {
            return fallback;
        }
        string? value;
        if (model.TryGetValue(key, out value) && !string.IsNullOrEmpty(value))
        {
            return value;
        }
        return fallback;
    }

    public static string ToSubCategoryJson(List<SubCategory>? list)
    {
        var sb = new System.Text.StringBuilder();
        sb.Append("[");
        if (list != null)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (i > 0)
                {
                    sb.Append(",");
                }
                SubCategory s = list[i];
                sb.Append("{\"Id\":");
                sb.Append(s.Id);
                sb.Append(",\"CategoryId\":");
                sb.Append(s.CategoryId);
                sb.Append(",\"Name\":");
                sb.Append(System.Text.Json.JsonSerializer.Serialize(s.Name));
                sb.Append("}");
            }
        }
        sb.Append("]");
        return sb.ToString();
    }
}
