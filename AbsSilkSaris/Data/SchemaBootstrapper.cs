using AbsSilkSaris.Services;

namespace AbsSilkSaris.Data;

public class SchemaBootstrapper
{
    private readonly IDbConnectionFactory _factory;
    public SchemaBootstrapper(IDbConnectionFactory factory) => _factory = factory;

    public async Task EnsureCreatedAsync()
    {
        await using var conn = (System.Data.Common.DbConnection)await Ado.OpenAsync(_factory);
        foreach (var sql in Tables())
        {
            await Ado.ExecuteAsync(conn, sql);
        }
    }

    private IEnumerable<string> Tables()
    {
        if (_factory.IsSqlServer)
        {
            yield return """
                IF OBJECT_ID('Admins','U') IS NULL CREATE TABLE Admins (
                    Id INT IDENTITY(1,1) PRIMARY KEY,
                    Username NVARCHAR(80) NOT NULL UNIQUE,
                    PasswordHash NVARCHAR(500) NOT NULL,
                    FullName NVARCHAR(120) NOT NULL,
                    Role NVARCHAR(40) NOT NULL
                );
                """;
            yield return """
                IF OBJECT_ID('Categories','U') IS NULL CREATE TABLE Categories (
                    Id INT IDENTITY(1,1) PRIMARY KEY,
                    Name NVARCHAR(160) NOT NULL,
                    Slug NVARCHAR(180) NOT NULL UNIQUE,
                    Description NVARCHAR(MAX) NOT NULL DEFAULT '',
                    ImageUrl NVARCHAR(400) NOT NULL DEFAULT '',
                    SortOrder INT NOT NULL DEFAULT 0,
                    IsActive BIT NOT NULL DEFAULT 1,
                    ShowInMenu BIT NOT NULL DEFAULT 1
                );
                """;
            yield return """
                IF OBJECT_ID('SubCategories','U') IS NULL CREATE TABLE SubCategories (
                    Id INT IDENTITY(1,1) PRIMARY KEY,
                    CategoryId INT NOT NULL,
                    Name NVARCHAR(160) NOT NULL,
                    Slug NVARCHAR(180) NOT NULL,
                    Description NVARCHAR(MAX) NOT NULL DEFAULT '',
                    ImageUrl NVARCHAR(400) NOT NULL DEFAULT '',
                    SortOrder INT NOT NULL DEFAULT 0,
                    IsActive BIT NOT NULL DEFAULT 1,
                    ShowInMenu BIT NOT NULL DEFAULT 1,
                    CONSTRAINT FK_Sub_Cat FOREIGN KEY (CategoryId) REFERENCES Categories(Id) ON DELETE CASCADE
                );
                """;
            yield return """
                IF OBJECT_ID('Products','U') IS NULL CREATE TABLE Products (
                    Id INT IDENTITY(1,1) PRIMARY KEY,
                    Name NVARCHAR(240) NOT NULL,
                    Slug NVARCHAR(240) NOT NULL UNIQUE,
                    Sku NVARCHAR(80) NOT NULL UNIQUE,
                    Description NVARCHAR(MAX) NOT NULL DEFAULT '',
                    Color NVARCHAR(80) NOT NULL DEFAULT '',
                    Weave NVARCHAR(80) NOT NULL DEFAULT '',
                    Occasion NVARCHAR(80) NOT NULL DEFAULT '',
                    Price DECIMAL(18,2) NOT NULL,
                    CompareAtPrice DECIMAL(18,2) NULL,
                    ImageUrl NVARCHAR(400) NOT NULL DEFAULT '',
                    HoverImageUrl NVARCHAR(400) NULL,
                    CategoryId INT NOT NULL,
                    SubCategoryId INT NULL,
                    IsNew BIT NOT NULL DEFAULT 0,
                    IsBestSeller BIT NOT NULL DEFAULT 0,
                    IsActive BIT NOT NULL DEFAULT 1,
                    Stock INT NOT NULL DEFAULT 8,
                    Rating FLOAT NOT NULL DEFAULT 4.8,
                    ReviewCount INT NOT NULL DEFAULT 0,
                    CONSTRAINT FK_Prod_Cat FOREIGN KEY (CategoryId) REFERENCES Categories(Id),
                    CONSTRAINT FK_Prod_Sub FOREIGN KEY (SubCategoryId) REFERENCES SubCategories(Id)
                );
                """;
            yield return """
                IF OBJECT_ID('ProductImages','U') IS NULL CREATE TABLE ProductImages (
                    Id INT IDENTITY(1,1) PRIMARY KEY,
                    ProductId INT NOT NULL,
                    ImageUrl NVARCHAR(400) NOT NULL,
                    SortOrder INT NOT NULL DEFAULT 0,
                    IsPrimary BIT NOT NULL DEFAULT 0,
                    CONSTRAINT FK_Img_Prod FOREIGN KEY (ProductId) REFERENCES Products(Id) ON DELETE CASCADE
                );
                """;
            yield return """
                IF OBJECT_ID('SiteSettings','U') IS NULL CREATE TABLE SiteSettings (
                    [Key] NVARCHAR(80) PRIMARY KEY,
                    [Value] NVARCHAR(MAX) NOT NULL
                );
                """;
            yield return """
                IF OBJECT_ID('Reviews','U') IS NULL CREATE TABLE Reviews (
                    Id INT IDENTITY(1,1) PRIMARY KEY,
                    CustomerName NVARCHAR(120) NOT NULL,
                    Location NVARCHAR(120) NOT NULL DEFAULT '',
                    PhotoUrl NVARCHAR(400) NOT NULL DEFAULT '',
                    Stars INT NOT NULL,
                    Title NVARCHAR(200) NOT NULL DEFAULT '',
                    Body NVARCHAR(MAX) NOT NULL DEFAULT '',
                    ProductId INT NULL
                );
                """;
            yield return """
                IF OBJECT_ID('Orders','U') IS NULL CREATE TABLE Orders (
                    Id INT IDENTITY(1,1) PRIMARY KEY,
                    OrderNumber NVARCHAR(40) NOT NULL UNIQUE,
                    CustomerName NVARCHAR(160) NOT NULL,
                    Email NVARCHAR(160) NOT NULL,
                    Phone NVARCHAR(40) NOT NULL DEFAULT '',
                    Address NVARCHAR(400) NOT NULL DEFAULT '',
                    City NVARCHAR(80) NOT NULL DEFAULT '',
                    State NVARCHAR(80) NOT NULL DEFAULT '',
                    Pincode NVARCHAR(20) NOT NULL DEFAULT '',
                    Status NVARCHAR(40) NOT NULL,
                    CreatedAt DATETIME2 NOT NULL,
                    Subtotal DECIMAL(18,2) NOT NULL,
                    Shipping DECIMAL(18,2) NOT NULL,
                    Total DECIMAL(18,2) NOT NULL
                );
                """;
            yield return """
                IF OBJECT_ID('OrderItems','U') IS NULL CREATE TABLE OrderItems (
                    Id INT IDENTITY(1,1) PRIMARY KEY,
                    OrderId INT NOT NULL,
                    ProductId INT NOT NULL,
                    ProductName NVARCHAR(240) NOT NULL,
                    Quantity INT NOT NULL,
                    UnitPrice DECIMAL(18,2) NOT NULL,
                    CONSTRAINT FK_Oi_Ord FOREIGN KEY (OrderId) REFERENCES Orders(Id) ON DELETE CASCADE
                );
                """;
            yield return """
                IF OBJECT_ID('NewsletterSubscribers','U') IS NULL CREATE TABLE NewsletterSubscribers (
                    Id INT IDENTITY(1,1) PRIMARY KEY,
                    Email NVARCHAR(160) NOT NULL UNIQUE,
                    SubscribedAt DATETIME2 NOT NULL
                );
                """;
            yield break;
        }

        yield return """
            CREATE TABLE IF NOT EXISTS Admins (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Username TEXT NOT NULL UNIQUE,
                PasswordHash TEXT NOT NULL,
                FullName TEXT NOT NULL,
                Role TEXT NOT NULL
            );
            """;
        yield return """
            CREATE TABLE IF NOT EXISTS Categories (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL,
                Slug TEXT NOT NULL UNIQUE,
                Description TEXT NOT NULL DEFAULT '',
                ImageUrl TEXT NOT NULL DEFAULT '',
                SortOrder INTEGER NOT NULL DEFAULT 0,
                IsActive INTEGER NOT NULL DEFAULT 1,
                ShowInMenu INTEGER NOT NULL DEFAULT 1
            );
            """;
        yield return """
            CREATE TABLE IF NOT EXISTS SubCategories (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                CategoryId INTEGER NOT NULL,
                Name TEXT NOT NULL,
                Slug TEXT NOT NULL,
                Description TEXT NOT NULL DEFAULT '',
                ImageUrl TEXT NOT NULL DEFAULT '',
                SortOrder INTEGER NOT NULL DEFAULT 0,
                IsActive INTEGER NOT NULL DEFAULT 1,
                ShowInMenu INTEGER NOT NULL DEFAULT 1,
                FOREIGN KEY(CategoryId) REFERENCES Categories(Id) ON DELETE CASCADE
            );
            """;
        yield return """
            CREATE TABLE IF NOT EXISTS Products (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL,
                Slug TEXT NOT NULL UNIQUE,
                Sku TEXT NOT NULL UNIQUE,
                Description TEXT NOT NULL DEFAULT '',
                Color TEXT NOT NULL DEFAULT '',
                Weave TEXT NOT NULL DEFAULT '',
                Occasion TEXT NOT NULL DEFAULT '',
                Price REAL NOT NULL,
                CompareAtPrice REAL NULL,
                ImageUrl TEXT NOT NULL DEFAULT '',
                HoverImageUrl TEXT NULL,
                CategoryId INTEGER NOT NULL,
                SubCategoryId INTEGER NULL,
                IsNew INTEGER NOT NULL DEFAULT 0,
                IsBestSeller INTEGER NOT NULL DEFAULT 0,
                IsActive INTEGER NOT NULL DEFAULT 1,
                Stock INTEGER NOT NULL DEFAULT 8,
                Rating REAL NOT NULL DEFAULT 4.8,
                ReviewCount INTEGER NOT NULL DEFAULT 0,
                FOREIGN KEY(CategoryId) REFERENCES Categories(Id),
                FOREIGN KEY(SubCategoryId) REFERENCES SubCategories(Id)
            );
            """;
        yield return """
            CREATE TABLE IF NOT EXISTS ProductImages (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                ProductId INTEGER NOT NULL,
                ImageUrl TEXT NOT NULL,
                SortOrder INTEGER NOT NULL DEFAULT 0,
                IsPrimary INTEGER NOT NULL DEFAULT 0,
                FOREIGN KEY(ProductId) REFERENCES Products(Id) ON DELETE CASCADE
            );
            """;
        yield return """
            CREATE TABLE IF NOT EXISTS SiteSettings (
                Key TEXT PRIMARY KEY,
                Value TEXT NOT NULL
            );
            """;
        yield return """
            CREATE TABLE IF NOT EXISTS Reviews (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                CustomerName TEXT NOT NULL,
                Location TEXT NOT NULL DEFAULT '',
                PhotoUrl TEXT NOT NULL DEFAULT '',
                Stars INTEGER NOT NULL,
                Title TEXT NOT NULL DEFAULT '',
                Body TEXT NOT NULL DEFAULT '',
                ProductId INTEGER NULL
            );
            """;
        yield return """
            CREATE TABLE IF NOT EXISTS Orders (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                OrderNumber TEXT NOT NULL UNIQUE,
                CustomerName TEXT NOT NULL,
                Email TEXT NOT NULL,
                Phone TEXT NOT NULL DEFAULT '',
                Address TEXT NOT NULL DEFAULT '',
                City TEXT NOT NULL DEFAULT '',
                State TEXT NOT NULL DEFAULT '',
                Pincode TEXT NOT NULL DEFAULT '',
                Status TEXT NOT NULL,
                CreatedAt TEXT NOT NULL,
                Subtotal REAL NOT NULL,
                Shipping REAL NOT NULL,
                Total REAL NOT NULL
            );
            """;
        yield return """
            CREATE TABLE IF NOT EXISTS OrderItems (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                OrderId INTEGER NOT NULL,
                ProductId INTEGER NOT NULL,
                ProductName TEXT NOT NULL,
                Quantity INTEGER NOT NULL,
                UnitPrice REAL NOT NULL,
                FOREIGN KEY(OrderId) REFERENCES Orders(Id) ON DELETE CASCADE
            );
            """;
        yield return """
            CREATE TABLE IF NOT EXISTS NewsletterSubscribers (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Email TEXT NOT NULL UNIQUE,
                SubscribedAt TEXT NOT NULL
            );
            """;
    }
}

public class CatalogSeeder
{
    private readonly CatalogRepository _db;
    public CatalogSeeder(CatalogRepository db) => _db = db;

    public async Task SeedAsync()
    {
        if (await _db.AdminCountAsync() == 0)
        {
            var hash = PasswordUtil.Hash("Admin@123");
            await _db.InsertAdminAsync("admin", hash, "ABS Atelier Admin", "Admin");
        }

        if (await _db.CategoryCountAsync() > 0) return;

        await _db.UpsertSettingAsync("HeroHeading", "Timeless Elegance in Every Weave");
        await _db.UpsertSettingAsync("HeroSubheading", "Authentic Kanchipuram Silk Sarees for Every Celebration");
        await _db.UpsertSettingAsync("Announcement", "Complimentary shipping on silks above ₹15,000 · Authenticated zari · Worldwide delivery");
        await _db.UpsertSettingAsync("Phone", "+91 44 4862 1100");
        await _db.UpsertSettingAsync("WhatsApp", "919876543210");
        await _db.UpsertSettingAsync("Email", "atelier@abssilksaris.com");
        await _db.UpsertSettingAsync("Address", "14, Silk Weavers Street, Kanchipuram, Tamil Nadu 631501");
        await _db.UpsertSettingAsync("HeroImage1", "/images/hero/hero-kanchipuram-saree.png");
        await _db.UpsertSettingAsync("HeroImage2", "/images/hero/hero-ivory-palace.png");

        async Task<int> C(string name, string slug, string desc, string img, int sort) =>
            await _db.InsertCategoryAsync(new Models.Category
            {
                Name = name, Slug = slug, Description = desc, ImageUrl = img, SortOrder = sort, IsActive = true, ShowInMenu = true
            });

        var bridal = await C("Bridal Silk Sarees", "bridal-silk-sarees", "Auspicious silks woven for timeless bridal moments.", "/images/categories/category-bridal.png", 1);
        var wedding = await C("Wedding Collection", "wedding-collection", "Heirloom Kanchipurams for every wedding ritual.", "/images/categories/category-wedding.png", 2);
        var zari = await C("Pure Zari Sarees", "pure-zari-sarees", "Authentic gold zari that catches temple light.", "/images/categories/category-zari.png", 3);
        var traditional = await C("Traditional Kanchipuram Sarees", "traditional-kanchipuram-sarees", "Classic korvai borders and temple motifs.", "/images/categories/category-traditional.png", 4);
        var soft = await C("Soft Silk Sarees", "soft-silk-sarees", "Lighter weaves for festive days and easy drape.", "/images/categories/category-softsilk.png", 5);
        var party = await C("Party Wear Silk Sarees", "party-wear-silk-sarees", "Statement silks for evenings worth dressing up.", "/images/categories/category-party.png", 6);

        async Task<int> S(int cat, string name, string slug, int sort) =>
            await _db.InsertSubCategoryAsync(new Models.SubCategory
            {
                CategoryId = cat, Name = name, Slug = slug, SortOrder = sort, IsActive = true, ShowInMenu = true, Description = name, ImageUrl = ""
            });

        var temple = await S(bridal, "Temple Border Bridal", "temple-border-bridal", 1);
        var muhurtham = await S(bridal, "Muhurtham Silks", "muhurtham-silks", 2);
        var ivory = await S(wedding, "Ivory & Gold", "ivory-gold", 1);
        var rituals = await S(wedding, "Wedding Rituals", "wedding-rituals", 2);
        var checks = await S(zari, "Zari Checks", "zari-checks", 1);
        var ganga = await S(zari, "Ganga Jamuna", "ganga-jamuna", 2);
        var korvai = await S(traditional, "Korvai Contrast", "korvai-contrast", 1);
        var kattam = await S(traditional, "Kattam Checks", "kattam-checks", 2);
        var festiveSoft = await S(soft, "Festive Soft Silk", "festive-soft-silk", 1);
        var everyday = await S(soft, "Everyday Classics", "everyday-classics", 2);
        var reception = await S(party, "Reception & Sangeet", "reception-sangeet", 1);
        var contemporary = await S(party, "Contemporary Party", "contemporary-party", 2);

        async Task P(string name, string sku, int cat, int? sub, decimal price, decimal? compare, string img, string color, string weave, string occasion, bool isNew, bool best, string desc)
        {
            var id = await _db.InsertProductAsync(new Models.Product
            {
                Name = name,
                Slug = sku.ToLowerInvariant(),
                Sku = sku,
                CategoryId = cat,
                SubCategoryId = sub,
                Price = price,
                CompareAtPrice = compare,
                ImageUrl = img,
                Color = color,
                Weave = weave,
                Occasion = occasion,
                IsNew = isNew,
                IsBestSeller = best,
                IsActive = true,
                Description = desc,
                Rating = 4.8,
                ReviewCount = 12,
                Stock = 8
            });
            await _db.AddProductImageAsync(id, img, 0, true);
        }

        await P("ABS Handloom Maroon Temple Border Bridal Kanchipuram", "ABS-KBF515", bridal, temple, 42800, 48900, "/images/products/product-maroon-temple.png", "Maroon", "Temple Border", "Bridal", true, true, "A heavy bridal Kanchipuram in deep maroon with dense gold zari temple borders and peacock buttas.");
        await P("ABS Handloom Ivory & Gold Wedding Silk", "ABS-KBF887", wedding, ivory, 36500, 41200, "/images/products/product-ivory-gold.png", "Ivory", "Zari Checks", "Wedding", true, true, "Ivory body with blush-pink contrast and gold zari — a luminous wedding silk.");
        await P("ABS Mayilkazhuthu Peacock Blue Kanjivaram", "ABS-KBF632", traditional, korvai, 22120, 24990, "/images/products/product-peacock-blue.png", "Peacock Blue", "Korvai Contrast", "Festival", true, true, "The beloved mayil kazhuthu shade with a purple contrast border.");
        await P("ABS Rani Pink Pure Zari Kanchipuram", "ABS-KBF836", zari, checks, 22880, null, "/images/products/product-rani-pink.png", "Rani Pink", "Butta", "Wedding", true, false, "Rani pink mulberry silk scattered with gold buttas.");
        await P("ABS Bottle Green Checks with Maroon Border", "ABS-KBF276", traditional, kattam, 18990, 21350, "/images/products/product-bottle-green.png", "Bottle Green", "Kattam Checks", "Festival", false, true, "Fine kattam checks in bottle green silk, maroon retta pettu border.");
        await P("ABS Honey Mustard Handloom Soft Silk", "ABS-KBF215", soft, everyday, 10950, 12400, "/images/products/product-mustard.png", "Mustard", "Soft Silk", "Everyday", true, true, "A lighter handloom soft silk in honey mustard.");
        await P("ABS Black Ganga Jamuna Border Kanjivaram", "ABS-SSK154", zari, ganga, 24420, 26800, "/images/products/product-black-ganga.png", "Black", "Ganga Jamuna", "Party", false, true, "Deep black body with a striking red-and-gold Ganga Jamuna border.");
        await P("ABS Pista Green Soft Silk with Gold Border", "ABS-KBF427", soft, festiveSoft, 15480, 17200, "/images/products/product-mint-soft.png", "Pista Green", "Soft Silk", "Party", true, false, "Airy pistachio silk with a delicate gold selvedge.");
        await P("ABS Plum Purple Temple Motif Bridal Silk", "ABS-KBF835", bridal, muhurtham, 22530, 25100, "/images/products/product-plum.png", "Plum Purple", "Temple Border", "Bridal", false, true, "Plum purple bridal silk with temple motifs along the pallu.");
        await P("ABS Bright Teal Statement Kanchipuram", "ABS-KBF673", party, reception, 40830, 44900, "/images/products/product-teal.png", "Teal", "Turning Border", "Party", true, false, "A rare teal body with dense gold threadwork.");
        await P("ABS Araku Maroon Traditional Korvai", "ABS-KBF804", traditional, korvai, 23480, 25900, "/images/products/product-maroon-temple.png", "Maroon", "Korvai Contrast", "Wedding", false, false, "Araku maroon body, classic korvai contrast.");
        await P("ABS Wedding Ivory Palace Silk", "ABS-KBF516", wedding, rituals, 31200, 34800, "/images/products/product-ivory-gold.png", "Ivory", "Borderless Butta", "Wedding", false, true, "When buttas become the border — an ivory Kanchipuram.");
        await P("ABS Party Rani Pink Contemporary Silk", "ABS-KBF820", party, contemporary, 19880, 22100, "/images/products/product-rani-pink.png", "Rani Pink", "Thread Checks", "Party", true, false, "A contemporary rani pink silk for cocktail and sangeet evenings.");
        await P("ABS Traditional Peacock Motif Green Silk", "ABS-KBF582", traditional, kattam, 20590, 22900, "/images/products/product-bottle-green.png", "Bottle Green", "Yaanai & Mayil", "Festival", false, false, "Stately peacock and yaanai motifs on bottle green silk.");
        await P("ABS Soft Silk Mint Festive Drape", "ABS-KBF571", soft, festiveSoft, 13850, 15500, "/images/products/product-mint-soft.png", "Mint", "Soft Silk", "Festival", true, false, "Mint festive soft silk, light on the shoulder.");
        await P("ABS Pure Zari Mustard Temple Silk", "ABS-KBF245", zari, checks, 20950, 23200, "/images/products/product-mustard.png", "Mustard", "Temple Border", "Wedding", false, true, "Mustard body, black temple border, tested gold zari.");

        await _db.InsertReviewAsync(new Models.Review { CustomerName = "Priya Venkatesh", Location = "Chennai", PhotoUrl = "/images/reviews/review-priya.png", Stars = 5, Title = "A bridal silk that felt like an heirloom", Body = "The maroon temple-border Kanchipuram I wore for my muhurtham was even more luminous in person." });
        await _db.InsertReviewAsync(new Models.Review { CustomerName = "Ananya Krishnan", Location = "Bengaluru", PhotoUrl = "/images/reviews/review-ananya.png", Stars = 5, Title = "Packaging and weave quality are exceptional", Body = "Ordered the honey mustard soft silk. It draped like a dream and arrived in museum-worthy packaging." });
        await _db.InsertReviewAsync(new Models.Review { CustomerName = "Meera Iyer", Location = "Hyderabad", PhotoUrl = "/images/reviews/review-meera.png", Stars = 5, Title = "Authentic Kanchipuram, worldwide shipping", Body = "I live abroad and still found the exact korvai contrast I grew up seeing." });
    }
}
