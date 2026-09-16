using System.Text.RegularExpressions;

namespace AbsSilkSaris.Services;

public static class SlugHelper
{
    public static string Slugify(string text)
    {
        if (string.IsNullOrWhiteSpace(text)) return "item";
        var s = text.Trim().ToLowerInvariant();
        s = Regex.Replace(s, @"[^a-z0-9\s-]", "");
        s = Regex.Replace(s, @"[\s-]+", "-").Trim('-');
        return string.IsNullOrEmpty(s) ? "item" : s;
    }
}
