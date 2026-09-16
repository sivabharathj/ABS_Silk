using AbsSilkSaris.Models;
using Microsoft.EntityFrameworkCore;

namespace AbsSilkSaris.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(ApplicationDbContext db)
    {
        if (await db.Categories.AnyAsync())
        {
            return;
        }

        var categories = new List<Category>
        {
            new() { Name = "Bridal Silk Sarees", Slug = "bridal-silk-sarees", Description = "Auspicious silks woven for timeless bridal moments.", ImageUrl = "/images/categories/category-bridal.png", SortOrder = 1 },
            new() { Name = "Wedding Collection", Slug = "wedding-collection", Description = "Heirloom Kanchipurams for every wedding ritual.", ImageUrl = "/images/categories/category-wedding.png", SortOrder = 2 },
            new() { Name = "Pure Zari Sarees", Slug = "pure-zari-sarees", Description = "Authentic gold zari that catches temple light.", ImageUrl = "/images/categories/category-zari.png", SortOrder = 3 },
            new() { Name = "Traditional Kanchipuram Sarees", Slug = "traditional-kanchipuram-sarees", Description = "Classic korvai borders and temple motifs.", ImageUrl = "/images/categories/category-traditional.png", SortOrder = 4 },
            new() { Name = "Soft Silk Sarees", Slug = "soft-silk-sarees", Description = "Lighter weaves for festive days and easy drape.", ImageUrl = "/images/categories/category-softsilk.png", SortOrder = 5 },
            new() { Name = "Party Wear Silk Sarees", Slug = "party-wear-silk-sarees", Description = "Statement silks for evenings worth dressing up.", ImageUrl = "/images/categories/category-party.png", SortOrder = 6 }
        };
        db.Categories.AddRange(categories);
        await db.SaveChangesAsync();

        Product P(string name, string sku, int cat, decimal price, decimal? compare, string img, string color, string weave, string occasion, bool isNew, bool best, string desc, string? hover = null) => new()
        {
            Name = name,
            Slug = sku.ToLowerInvariant(),
            Sku = sku,
            CategoryId = cat,
            Price = price,
            CompareAtPrice = compare,
            ImageUrl = img,
            HoverImageUrl = hover,
            Color = color,
            Weave = weave,
            Occasion = occasion,
            IsNew = isNew,
            IsBestSeller = best,
            Description = desc,
            Rating = 4.7 + (sku.GetHashCode() % 3) * 0.1,
            ReviewCount = 8 + Math.Abs(sku.GetHashCode() % 40),
            Stock = 6 + Math.Abs(sku.GetHashCode() % 10)
        };

        var bridal = categories[0].Id;
        var wedding = categories[1].Id;
        var zari = categories[2].Id;
        var traditional = categories[3].Id;
        var soft = categories[4].Id;
        var party = categories[5].Id;

        db.Products.AddRange(
            P("ABS Handloom Maroon Temple Border Bridal Kanchipuram", "ABS-KBF515", bridal, 42800, 48900, "/images/products/product-maroon-temple.png", "Maroon", "Temple Border", "Bridal", true, true,
                "A heavy bridal Kanchipuram in deep maroon with dense gold zari temple borders and peacock buttas. Woven in pure mulberry silk for the muhurtham hour."),
            P("ABS Handloom Ivory & Gold Wedding Silk", "ABS-KBF887", wedding, 36500, 41200, "/images/products/product-ivory-gold.png", "Ivory", "Zari Checks", "Wedding", true, true,
                "Ivory body with blush-pink contrast and gold zari — a luminous wedding silk inspired by Kanchipuram palaces."),
            P("ABS Mayilkazhuthu Peacock Blue Kanjivaram", "ABS-KBF632", traditional, 22120, 24990, "/images/products/product-peacock-blue.png", "Peacock Blue", "Korvai Contrast", "Festival", true, true,
                "The beloved mayil kazhuthu shade with a purple contrast border. Classic korvai interlocking, authentic zari."),
            P("ABS Rani Pink Pure Zari Kanchipuram", "ABS-KBF836", zari, 22880, null, "/images/products/product-rani-pink.png", "Rani Pink", "Butta", "Wedding", true, false,
                "Rani pink mulberry silk scattered with gold buttas — a festive favourite from our zari atelier."),
            P("ABS Bottle Green Checks with Maroon Border", "ABS-KBF276", traditional, 18990, 21350, "/images/products/product-bottle-green.png", "Bottle Green", "Kattam Checks", "Festival", false, true,
                "Fine kattam checks in bottle green silk, finished with a maroon retta pettu border."),
            P("ABS Honey Mustard Handloom Soft Silk", "ABS-KBF215", soft, 10950, 12400, "/images/products/product-mustard.png", "Mustard", "Soft Silk", "Everyday", true, true,
                "A lighter handloom soft silk in honey mustard with a black contrast — made for days worth dressing up."),
            P("ABS Black Ganga Jamuna Border Kanjivaram", "ABS-SSK154", zari, 24420, 26800, "/images/products/product-black-ganga.png", "Black", "Ganga Jamuna", "Party", false, true,
                "Deep black body with a striking red-and-gold Ganga Jamuna border. Bold, contemporary Kanchipuram."),
            P("ABS Pista Green Soft Silk with Gold Border", "ABS-KBF427", soft, 15480, 17200, "/images/products/product-mint-soft.png", "Pista Green", "Soft Silk", "Party", true, false,
                "Airy pistachio silk with a delicate gold selvedge — easy drape, festive sheen."),
            P("ABS Plum Purple Temple Motif Bridal Silk", "ABS-KBF835", bridal, 22530, 25100, "/images/products/product-plum.png", "Plum Purple", "Temple Border", "Bridal", false, true,
                "Plum purple bridal silk with temple motifs along the pallu. Heirloom weight, luminous zari."),
            P("ABS Bright Teal Statement Kanchipuram", "ABS-KBF673", party, 40830, 44900, "/images/products/product-teal.png", "Teal", "Turning Border", "Party", true, false,
                "A rare teal body with dense gold threadwork — Dwarak-worthy drama for reception nights."),
            P("ABS Araku Maroon Traditional Korvai", "ABS-KBF804", traditional, 23480, 25900, "/images/products/product-maroon-temple.png", "Maroon", "Korvai Contrast", "Wedding", false, false,
                "Araku maroon body, classic korvai contrast. Woven in our Kanchipuram unit from mulberry silk and tested zari."),
            P("ABS Wedding Ivory Palace Silk", "ABS-KBF516", wedding, 31200, 34800, "/images/products/product-ivory-gold.png", "Ivory", "Borderless Butta", "Wedding", false, true,
                "When buttas become the border — an ivory Kanchipuram with a distinctive borderless look."),
            P("ABS Party Rani Pink Contemporary Silk", "ABS-KBF820", party, 19880, 22100, "/images/products/product-rani-pink.png", "Rani Pink", "Thread Checks", "Party", true, false,
                "A contemporary rani pink silk with fine thread checks for cocktail and sangeet evenings."),
            P("ABS Traditional Peacock Motif Green Silk", "ABS-KBF582", traditional, 20590, 22900, "/images/products/product-bottle-green.png", "Bottle Green", "Yaanai & Mayil", "Festival", false, false,
                "Stately peacock and yaanai motifs on bottle green silk — a tribute to Kanchipuram iconography."),
            P("ABS Soft Silk Mint Festive Drape", "ABS-KBF571", soft, 13850, 15500, "/images/products/product-mint-soft.png", "Mint", "Soft Silk", "Festival", true, false,
                "Mint festive soft silk, light on the shoulder, rich in sheen."),
            P("ABS Pure Zari Mustard Temple Silk", "ABS-KBF245", zari, 20950, 23200, "/images/products/product-mustard.png", "Mustard", "Temple Border", "Wedding", false, true,
                "Mustard body, black temple border, tested gold zari — a colour spotlight favourite.")
        );

        db.Reviews.AddRange(
            new Review { CustomerName = "Priya Venkatesh", Location = "Chennai", PhotoUrl = "/images/reviews/review-priya.png", Stars = 5, Title = "A bridal silk that felt like an heirloom", Body = "The maroon temple-border Kanchipuram I wore for my muhurtham was even more luminous in person. The zari has a quiet richness — not flashy, truly bridal." },
            new Review { CustomerName = "Ananya Krishnan", Location = "Bengaluru", PhotoUrl = "/images/reviews/review-ananya.png", Stars = 5, Title = "Packaging and weave quality are exceptional", Body = "Ordered the honey mustard soft silk for a family function. It draped like a dream and arrived in museum-worthy packaging. ABS feels like a heritage house." },
            new Review { CustomerName = "Meera Iyer", Location = "Hyderabad", PhotoUrl = "/images/reviews/review-meera.png", Stars = 5, Title = "Authentic Kanchipuram, worldwide shipping", Body = "I live abroad and still found the exact korvai contrast I grew up seeing. Tracking was clear, and the silk smells of the loom — in the best way." }
        );

        await db.SaveChangesAsync();
    }
}
