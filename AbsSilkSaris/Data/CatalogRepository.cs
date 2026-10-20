using System.Data;
using AbsSilkSaris.Models;

namespace AbsSilkSaris.Data;

public class CatalogRepository
{
    private readonly IDbConnectionFactory _factory;
    public CatalogRepository(IDbConnectionFactory factory)
    {
        _factory = factory;
    }

    private Task<IDbConnection> Open()
    {
        return Ado.OpenAsync(_factory);
    }

    private static Category MapCategory(IDataRecord r)
    {
        Category item = new Category();
        item.Id = Ado.Int(r, "Id");
        item.Name = Ado.Str(r, "Name");
        item.Slug = Ado.Str(r, "Slug");
        item.Description = Ado.Str(r, "Description");
        item.ImageUrl = Ado.Str(r, "ImageUrl");
        item.SortOrder = Ado.Int(r, "SortOrder");
        item.IsActive = Ado.Flag(r, "IsActive");
        item.ShowInMenu = Ado.Flag(r, "ShowInMenu");
        return item;
    }

    private static SubCategory MapSub(IDataRecord r)
    {
        SubCategory item = new SubCategory();
        item.Id = Ado.Int(r, "Id");
        item.CategoryId = Ado.Int(r, "CategoryId");
        if (r.FieldCount > 8 && Has(r, "CategoryName"))
        {
            item.CategoryName = Ado.Str(r, "CategoryName");
        }
        item.Name = Ado.Str(r, "Name");
        item.Slug = Ado.Str(r, "Slug");
        item.Description = Ado.Str(r, "Description");
        item.ImageUrl = Ado.Str(r, "ImageUrl");
        item.SortOrder = Ado.Int(r, "SortOrder");
        item.IsActive = Ado.Flag(r, "IsActive");
        item.ShowInMenu = Ado.Flag(r, "ShowInMenu");
        return item;
    }

    private static AdminUser MapAdmin(IDataRecord r)
    {
        AdminUser item = new AdminUser();
        item.Id = Ado.Int(r, "Id");
        item.Username = Ado.Str(r, "Username");
        item.PasswordHash = Ado.Str(r, "PasswordHash");
        item.FullName = Ado.Str(r, "FullName");
        item.Role = Ado.Str(r, "Role");
        return item;
    }

    private static string MapColor(IDataRecord r)
    {
        return Ado.Str(r, "Color");
    }

    private static ProductImage MapProductImage(IDataRecord r)
    {
        ProductImage item = new ProductImage();
        item.Id = Ado.Int(r, "Id");
        item.ProductId = Ado.Int(r, "ProductId");
        item.ImageUrl = Ado.Str(r, "ImageUrl");
        item.SortOrder = Ado.Int(r, "SortOrder");
        item.IsPrimary = Ado.Flag(r, "IsPrimary");
        return item;
    }

    private static Review MapReview(IDataRecord r)
    {
        Review item = new Review();
        item.Id = Ado.Int(r, "Id");
        item.CustomerName = Ado.Str(r, "CustomerName");
        item.Location = Ado.Str(r, "Location");
        item.PhotoUrl = Ado.Str(r, "PhotoUrl");
        item.Stars = Ado.Int(r, "Stars");
        item.Title = Ado.Str(r, "Title");
        item.Body = Ado.Str(r, "Body");
        item.ProductId = Ado.IntN(r, "ProductId");
        return item;
    }

    private static SettingRow MapSetting(IDataRecord r)
    {
        SettingRow item = new SettingRow();
        item.Key = Ado.Str(r, "Key");
        item.Value = Ado.Str(r, "Value");
        return item;
    }

    private static Order MapOrder(IDataRecord r)
    {
        Order item = new Order();
        item.Id = Ado.Int(r, "Id");
        item.OrderNumber = Ado.Str(r, "OrderNumber");
        item.CustomerName = Ado.Str(r, "CustomerName");
        item.Email = Ado.Str(r, "Email");
        item.Phone = Has(r, "Phone") ? Ado.Str(r, "Phone") : "";
        item.Address = Has(r, "Address") ? Ado.Str(r, "Address") : "";
        item.City = Has(r, "City") ? Ado.Str(r, "City") : "";
        item.State = Has(r, "State") ? Ado.Str(r, "State") : "";
        item.Pincode = Has(r, "Pincode") ? Ado.Str(r, "Pincode") : "";
        item.Status = Ado.Str(r, "Status");
        item.CreatedAt = Ado.Dt(r, "CreatedAt");
        item.Subtotal = Has(r, "Subtotal") ? Ado.Dec(r, "Subtotal") : 0;
        item.Shipping = Has(r, "Shipping") ? Ado.Dec(r, "Shipping") : 0;
        item.Total = Ado.Dec(r, "Total");
        return item;
    }

    private static OrderItem MapOrderItem(IDataRecord r)
    {
        OrderItem item = new OrderItem();
        item.Id = Ado.Int(r, "Id");
        item.OrderId = Ado.Int(r, "OrderId");
        item.ProductId = Ado.Int(r, "ProductId");
        item.ProductName = Ado.Str(r, "ProductName");
        item.Quantity = Ado.Int(r, "Quantity");
        item.UnitPrice = Ado.Dec(r, "UnitPrice");
        return item;
    }

    private class SettingRow
    {
        public string Key { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
    }

    private static bool Has(IDataRecord r, string name)
    {
        for (var i = 0; i < r.FieldCount; i++)
            if (string.Equals(r.GetName(i), name, StringComparison.OrdinalIgnoreCase)) return true;
        return false;
    }

    private static Product MapProduct(IDataRecord r)
    {
        var p = new Product
        {
            Id = Ado.Int(r, "Id"),
            Name = Ado.Str(r, "Name"),
            Slug = Ado.Str(r, "Slug"),
            Sku = Ado.Str(r, "Sku"),
            Description = Ado.Str(r, "Description"),
            Color = Ado.Str(r, "Color"),
            Weave = Ado.Str(r, "Weave"),
            Occasion = Ado.Str(r, "Occasion"),
            Price = Ado.Dec(r, "Price"),
            CompareAtPrice = Ado.DecN(r, "CompareAtPrice"),
            ImageUrl = Ado.Str(r, "ImageUrl"),
            HoverImageUrl = Ado.StrN(r, "HoverImageUrl"),
            CategoryId = Ado.Int(r, "CategoryId"),
            SubCategoryId = Ado.IntN(r, "SubCategoryId"),
            IsNew = Ado.Flag(r, "IsNew"),
            IsBestSeller = Ado.Flag(r, "IsBestSeller"),
            IsActive = Ado.Flag(r, "IsActive"),
            Stock = Ado.Int(r, "Stock"),
            Rating = Ado.Dbl(r, "Rating"),
            ReviewCount = Ado.Int(r, "ReviewCount")
        };
        if (Has(r, "CategoryName") && !string.IsNullOrEmpty(Ado.Str(r, "CategoryName")))
        {
            p.Category = new Category { Id = p.CategoryId, Name = Ado.Str(r, "CategoryName"), Slug = Has(r, "CategorySlug") ? Ado.Str(r, "CategorySlug") : "" };
        }
        if (Has(r, "SubCategoryName") && p.SubCategoryId is not null)
        {
            p.SubCategory = new SubCategory { Id = p.SubCategoryId.Value, Name = Ado.Str(r, "SubCategoryName"), Slug = Has(r, "SubCategorySlug") ? Ado.Str(r, "SubCategorySlug") : "" };
        }
        return p;
    }

    public async Task<int> AdminCountAsync()
    {
        await using var conn = (System.Data.Common.DbConnection)await Open();
        var n = await Ado.ScalarAsync(conn, "SELECT COUNT(*) FROM Admins");
        return Convert.ToInt32(n);
    }

    public async Task InsertAdminAsync(string username, string hash, string fullName, string role)
    {
        await using var conn = (System.Data.Common.DbConnection)await Open();
        await Ado.ExecuteAsync(conn,
            "INSERT INTO Admins (Username, PasswordHash, FullName, Role) VALUES (@u,@p,@n,@r)",
            ("u", username), ("p", hash), ("n", fullName), ("r", role));
    }

    public async Task<AdminUser?> GetAdminByUsernameAsync(string username)
    {
        await using var conn = (System.Data.Common.DbConnection)await Open();
        return await Ado.QuerySingleAsync(conn,
            "SELECT Id, Username, PasswordHash, FullName, Role FROM Admins WHERE Username=@u",
            MapAdmin, ("u", username));
    }

    public async Task<int> CategoryCountAsync()
    {
        await using var conn = (System.Data.Common.DbConnection)await Open();
        return Convert.ToInt32(await Ado.ScalarAsync(conn, "SELECT COUNT(*) FROM Categories"));
    }

    public async Task<List<Category>> GetCategoriesAsync(bool menuOnly = false, bool activeOnly = false)
    {
        await using var conn = (System.Data.Common.DbConnection)await Open();
        var sql = "SELECT Id, Name, Slug, Description, ImageUrl, SortOrder, IsActive, ShowInMenu FROM Categories WHERE 1=1";
        var args = new List<(string, object?)>();
        if (activeOnly) sql += " AND IsActive=1";
        if (menuOnly) sql += " AND ShowInMenu=1";
        sql += " ORDER BY SortOrder, Name";
        return await Ado.QueryAsync(conn, sql, MapCategory, args.ToArray());
    }

    public async Task<Category?> GetCategoryAsync(int id)
    {
        await using var conn = (System.Data.Common.DbConnection)await Open();
        return await Ado.QuerySingleAsync(conn,
            "SELECT Id, Name, Slug, Description, ImageUrl, SortOrder, IsActive, ShowInMenu FROM Categories WHERE Id=@id",
            MapCategory, ("id", id));
    }

    public async Task<int> InsertCategoryAsync(Category c)
    {
        await using var conn = (System.Data.Common.DbConnection)await Open();
        await Ado.ExecuteAsync(conn,
            "INSERT INTO Categories (Name, Slug, Description, ImageUrl, SortOrder, IsActive, ShowInMenu) VALUES (@n,@s,@d,@i,@o,@a,@m)",
            ("n", c.Name), ("s", c.Slug), ("d", c.Description), ("i", c.ImageUrl),
            ("o", c.SortOrder), ("a", c.IsActive ? 1 : 0), ("m", c.ShowInMenu ? 1 : 0));
        return Convert.ToInt32(await Ado.ScalarAsync(conn, _factory.LastInsertIdSql));
    }

    public async Task UpdateCategoryAsync(Category c)
    {
        await using var conn = (System.Data.Common.DbConnection)await Open();
        await Ado.ExecuteAsync(conn,
            "UPDATE Categories SET Name=@n, Slug=@s, Description=@d, ImageUrl=@i, SortOrder=@o, IsActive=@a, ShowInMenu=@m WHERE Id=@id",
            ("n", c.Name), ("s", c.Slug), ("d", c.Description), ("i", c.ImageUrl),
            ("o", c.SortOrder), ("a", c.IsActive ? 1 : 0), ("m", c.ShowInMenu ? 1 : 0), ("id", c.Id));
    }

    public async Task DeleteCategoryAsync(int id)
    {
        await using var conn = (System.Data.Common.DbConnection)await Open();
        await Ado.ExecuteAsync(conn, "DELETE FROM Products WHERE CategoryId=@id", ("id", id));
        await Ado.ExecuteAsync(conn, "DELETE FROM SubCategories WHERE CategoryId=@id", ("id", id));
        await Ado.ExecuteAsync(conn, "DELETE FROM Categories WHERE Id=@id", ("id", id));
    }

    public async Task<List<SubCategory>> GetSubCategoriesAsync(int? categoryId = null, bool menuOnly = false)
    {
        await using var conn = (System.Data.Common.DbConnection)await Open();
        var sql = """
            SELECT s.Id, s.CategoryId, s.Name, s.Slug, s.Description, s.ImageUrl, s.SortOrder, s.IsActive, s.ShowInMenu, c.Name AS CategoryName
            FROM SubCategories s INNER JOIN Categories c ON c.Id=s.CategoryId WHERE 1=1
            """;
        var args = new List<(string, object?)>();
        if (categoryId is not null) { sql += " AND s.CategoryId=@cid"; args.Add(("cid", categoryId)); }
        if (menuOnly) sql += " AND s.ShowInMenu=1 AND s.IsActive=1";
        sql += " ORDER BY s.SortOrder, s.Name";
        return await Ado.QueryAsync(conn, sql, MapSub, args.ToArray());
    }

    public async Task<SubCategory?> GetSubCategoryAsync(int id)
    {
        await using var conn = (System.Data.Common.DbConnection)await Open();
        return await Ado.QuerySingleAsync(conn, """
            SELECT s.Id, s.CategoryId, s.Name, s.Slug, s.Description, s.ImageUrl, s.SortOrder, s.IsActive, s.ShowInMenu, c.Name AS CategoryName
            FROM SubCategories s INNER JOIN Categories c ON c.Id=s.CategoryId WHERE s.Id=@id
            """, MapSub, ("id", id));
    }

    public async Task<int> InsertSubCategoryAsync(SubCategory s)
    {
        await using var conn = (System.Data.Common.DbConnection)await Open();
        await Ado.ExecuteAsync(conn,
            "INSERT INTO SubCategories (CategoryId, Name, Slug, Description, ImageUrl, SortOrder, IsActive, ShowInMenu) VALUES (@c,@n,@s,@d,@i,@o,@a,@m)",
            ("c", s.CategoryId), ("n", s.Name), ("s", s.Slug), ("d", s.Description), ("i", s.ImageUrl),
            ("o", s.SortOrder), ("a", s.IsActive ? 1 : 0), ("m", s.ShowInMenu ? 1 : 0));
        return Convert.ToInt32(await Ado.ScalarAsync(conn, _factory.LastInsertIdSql));
    }

    public async Task UpdateSubCategoryAsync(SubCategory s)
    {
        await using var conn = (System.Data.Common.DbConnection)await Open();
        await Ado.ExecuteAsync(conn,
            "UPDATE SubCategories SET CategoryId=@c, Name=@n, Slug=@s, Description=@d, ImageUrl=@i, SortOrder=@o, IsActive=@a, ShowInMenu=@m WHERE Id=@id",
            ("c", s.CategoryId), ("n", s.Name), ("s", s.Slug), ("d", s.Description), ("i", s.ImageUrl),
            ("o", s.SortOrder), ("a", s.IsActive ? 1 : 0), ("m", s.ShowInMenu ? 1 : 0), ("id", s.Id));
    }

    public async Task DeleteSubCategoryAsync(int id)
    {
        await using var conn = (System.Data.Common.DbConnection)await Open();
        await Ado.ExecuteAsync(conn, "UPDATE Products SET SubCategoryId=NULL WHERE SubCategoryId=@id", ("id", id));
        await Ado.ExecuteAsync(conn, "DELETE FROM SubCategories WHERE Id=@id", ("id", id));
    }

    public async Task<List<MenuCategory>> GetMenuAsync()
    {
        var cats = await GetCategoriesAsync(menuOnly: true, activeOnly: true);
        var subs = await GetSubCategoriesAsync(menuOnly: true);
        var menu = new List<MenuCategory>();
        for (int i = 0; i < cats.Count; i++)
        {
            MenuCategory item = new MenuCategory();
            item.Category = cats[i];
            item.SubCategories = new List<SubCategory>();
            for (int j = 0; j < subs.Count; j++)
            {
                if (subs[j].CategoryId == cats[i].Id)
                {
                    item.SubCategories.Add(subs[j]);
                }
            }
            menu.Add(item);
        }
        return menu;
    }

    public async Task<List<Product>> SearchProductsAsync(string? categorySlug, string? subSlug, string? color, string? q, string sort, decimal? min, decimal? max, bool? isNew = null, bool? best = null, int maxRows = 0)
    {
        await using var conn = (System.Data.Common.DbConnection)await Open();
        var sql = """
            SELECT p.*, c.Name AS CategoryName, c.Slug AS CategorySlug, s.Name AS SubCategoryName, s.Slug AS SubCategorySlug
            FROM Products p
            INNER JOIN Categories c ON c.Id=p.CategoryId
            LEFT JOIN SubCategories s ON s.Id=p.SubCategoryId
            WHERE p.IsActive=1
            """;
        var args = new List<(string, object?)>();
        if (!string.IsNullOrWhiteSpace(categorySlug)) { sql += " AND c.Slug=@cslug"; args.Add(("cslug", categorySlug)); }
        if (!string.IsNullOrWhiteSpace(subSlug)) { sql += " AND s.Slug=@sslug"; args.Add(("sslug", subSlug)); }
        if (!string.IsNullOrWhiteSpace(color)) { sql += " AND p.Color=@color"; args.Add(("color", color)); }
        if (!string.IsNullOrWhiteSpace(q)) { sql += " AND (p.Name LIKE @q OR p.Color LIKE @q OR p.Weave LIKE @q OR p.Sku LIKE @q)"; args.Add(("q", "%" + q + "%")); }
        if (min is not null) { sql += " AND p.Price>=@min"; args.Add(("min", min)); }
        if (max is not null) { sql += " AND p.Price<=@max"; args.Add(("max", max)); }
        if (isNew == true) sql += " AND p.IsNew=1";
        if (best == true) sql += " AND p.IsBestSeller=1";
        if (sort == "price-asc")
        {
            sql += " ORDER BY p.Price ASC";
        }
        else if (sort == "price-desc")
        {
            sql += " ORDER BY p.Price DESC";
        }
        else if (sort == "newest")
        {
            sql += " ORDER BY p.Id DESC";
        }
        else
        {
            sql += " ORDER BY p.IsBestSeller DESC, p.IsNew DESC, p.Id DESC";
        }
        if (maxRows > 0)
        {
            if (_factory.IsSqlServer)
            {
                sql += " OFFSET 0 ROWS FETCH NEXT " + maxRows + " ROWS ONLY";
            }
            else
            {
                sql += " LIMIT " + maxRows;
            }
        }
        return await Ado.QueryAsync(conn, sql, MapProduct, args.ToArray());
    }

    public async Task<List<Product>> ListAllProductsAsync()
    {
        await using var conn = (System.Data.Common.DbConnection)await Open();
        return await Ado.QueryAsync(conn, """
            SELECT p.*, c.Name AS CategoryName, c.Slug AS CategorySlug, s.Name AS SubCategoryName, s.Slug AS SubCategorySlug
            FROM Products p
            INNER JOIN Categories c ON c.Id=p.CategoryId
            LEFT JOIN SubCategories s ON s.Id=p.SubCategoryId
            ORDER BY p.Id DESC
            """, MapProduct);
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        await using var conn = (System.Data.Common.DbConnection)await Open();
        return await Ado.QuerySingleAsync(conn, """
            SELECT p.*, c.Name AS CategoryName, c.Slug AS CategorySlug, s.Name AS SubCategoryName, s.Slug AS SubCategorySlug
            FROM Products p INNER JOIN Categories c ON c.Id=p.CategoryId
            LEFT JOIN SubCategories s ON s.Id=p.SubCategoryId WHERE p.Id=@id
            """, MapProduct, ("id", id));
    }

    public async Task<Product?> GetProductBySlugAsync(string slug)
    {
        await using var conn = (System.Data.Common.DbConnection)await Open();
        return await Ado.QuerySingleAsync(conn, """
            SELECT p.*, c.Name AS CategoryName, c.Slug AS CategorySlug, s.Name AS SubCategoryName, s.Slug AS SubCategorySlug
            FROM Products p INNER JOIN Categories c ON c.Id=p.CategoryId
            LEFT JOIN SubCategories s ON s.Id=p.SubCategoryId WHERE p.Slug=@slug
            """, MapProduct, ("slug", slug));
    }

    public async Task<List<string>> GetColorsAsync()
    {
        await using var conn = (System.Data.Common.DbConnection)await Open();
        return await Ado.QueryAsync(conn, "SELECT DISTINCT Color FROM Products WHERE Color<>'' ORDER BY Color", MapColor);
    }

    public async Task<int> InsertProductAsync(Product p)
    {
        await using var conn = (System.Data.Common.DbConnection)await Open();
        await Ado.ExecuteAsync(conn, """
            INSERT INTO Products (Name, Slug, Sku, Description, Color, Weave, Occasion, Price, CompareAtPrice, ImageUrl, HoverImageUrl, CategoryId, SubCategoryId, IsNew, IsBestSeller, IsActive, Stock, Rating, ReviewCount)
            VALUES (@n,@slug,@sku,@d,@col,@w,@oc,@pr,@cmp,@img,@h,@cid,@sid,@nw,@bs,@act,@st,@rt,@rc)
            """,
            ("n", p.Name), ("slug", p.Slug), ("sku", p.Sku), ("d", p.Description), ("col", p.Color), ("w", p.Weave),
            ("oc", p.Occasion), ("pr", p.Price), ("cmp", p.CompareAtPrice), ("img", p.ImageUrl), ("h", p.HoverImageUrl),
            ("cid", p.CategoryId), ("sid", p.SubCategoryId), ("nw", p.IsNew ? 1 : 0), ("bs", p.IsBestSeller ? 1 : 0),
            ("act", p.IsActive ? 1 : 0), ("st", p.Stock), ("rt", p.Rating), ("rc", p.ReviewCount));
        return Convert.ToInt32(await Ado.ScalarAsync(conn, _factory.LastInsertIdSql));
    }

    public async Task UpdateProductAsync(Product p)
    {
        await using var conn = (System.Data.Common.DbConnection)await Open();
        await Ado.ExecuteAsync(conn, """
            UPDATE Products SET Name=@n, Slug=@slug, Sku=@sku, Description=@d, Color=@col, Weave=@w, Occasion=@oc,
            Price=@pr, CompareAtPrice=@cmp, ImageUrl=@img, HoverImageUrl=@h, CategoryId=@cid, SubCategoryId=@sid,
            IsNew=@nw, IsBestSeller=@bs, IsActive=@act, Stock=@st WHERE Id=@id
            """,
            ("n", p.Name), ("slug", p.Slug), ("sku", p.Sku), ("d", p.Description), ("col", p.Color), ("w", p.Weave),
            ("oc", p.Occasion), ("pr", p.Price), ("cmp", p.CompareAtPrice), ("img", p.ImageUrl), ("h", p.HoverImageUrl),
            ("cid", p.CategoryId), ("sid", p.SubCategoryId), ("nw", p.IsNew ? 1 : 0), ("bs", p.IsBestSeller ? 1 : 0),
            ("act", p.IsActive ? 1 : 0), ("st", p.Stock), ("id", p.Id));
    }

    public async Task DeleteProductAsync(int id)
    {
        await using var conn = (System.Data.Common.DbConnection)await Open();
        await Ado.ExecuteAsync(conn, "DELETE FROM ProductImages WHERE ProductId=@id", ("id", id));
        await Ado.ExecuteAsync(conn, "DELETE FROM Products WHERE Id=@id", ("id", id));
    }

    public async Task<List<ProductImage>> GetProductImagesAsync(int productId)
    {
        await using var conn = (System.Data.Common.DbConnection)await Open();
        return await Ado.QueryAsync(conn,
            "SELECT Id, ProductId, ImageUrl, SortOrder, IsPrimary FROM ProductImages WHERE ProductId=@id ORDER BY IsPrimary DESC, SortOrder, Id",
            MapProductImage, ("id", productId));
    }

    public async Task AddProductImageAsync(int productId, string url, int sort, bool primary)
    {
        await using var conn = (System.Data.Common.DbConnection)await Open();
        if (primary)
        {
            await Ado.ExecuteAsync(conn, "UPDATE ProductImages SET IsPrimary=0 WHERE ProductId=@id", ("id", productId));
            await Ado.ExecuteAsync(conn, "UPDATE Products SET ImageUrl=@u WHERE Id=@id", ("u", url), ("id", productId));
        }
        await Ado.ExecuteAsync(conn,
            "INSERT INTO ProductImages (ProductId, ImageUrl, SortOrder, IsPrimary) VALUES (@p,@u,@s,@pr)",
            ("p", productId), ("u", url), ("s", sort), ("pr", primary ? 1 : 0));
    }

    public async Task DeleteProductImageAsync(int imageId)
    {
        await using var conn = (System.Data.Common.DbConnection)await Open();
        await Ado.ExecuteAsync(conn, "DELETE FROM ProductImages WHERE Id=@id", ("id", imageId));
    }

    public async Task<List<Review>> GetReviewsAsync()
    {
        await using var conn = (System.Data.Common.DbConnection)await Open();
        return await Ado.QueryAsync(conn, "SELECT Id, CustomerName, Location, PhotoUrl, Stars, Title, Body, ProductId FROM Reviews ORDER BY Id DESC",
            MapReview);
    }

    public async Task InsertReviewAsync(Review r)
    {
        await using var conn = (System.Data.Common.DbConnection)await Open();
        await Ado.ExecuteAsync(conn,
            "INSERT INTO Reviews (CustomerName, Location, PhotoUrl, Stars, Title, Body, ProductId) VALUES (@n,@l,@p,@s,@t,@b,@pid)",
            ("n", r.CustomerName), ("l", r.Location), ("p", r.PhotoUrl), ("s", r.Stars), ("t", r.Title), ("b", r.Body), ("pid", r.ProductId));
    }

    public async Task DeleteReviewAsync(int id)
    {
        await using var conn = (System.Data.Common.DbConnection)await Open();
        await Ado.ExecuteAsync(conn, "DELETE FROM Reviews WHERE Id=@id", ("id", id));
    }

    public async Task<Dictionary<string, string>> GetSettingsAsync()
    {
        await using var conn = (System.Data.Common.DbConnection)await Open();
        var rows = await Ado.QueryAsync(conn, "SELECT Key, Value FROM SiteSettings", MapSetting);
        Dictionary<string, string> map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        for (int i = 0; i < rows.Count; i++)
        {
            map[rows[i].Key] = rows[i].Value;
        }
        return map;
    }

    public async Task UpsertSettingAsync(string key, string value)
    {
        await using var conn = (System.Data.Common.DbConnection)await Open();
        if (_factory.IsSqlServer)
        {
            await Ado.ExecuteAsync(conn, """
                IF EXISTS (SELECT 1 FROM SiteSettings WHERE [Key]=@k) UPDATE SiteSettings SET [Value]=@v WHERE [Key]=@k
                ELSE INSERT INTO SiteSettings ([Key],[Value]) VALUES (@k,@v)
                """, ("k", key), ("v", value));
        }
        else
        {
            await Ado.ExecuteAsync(conn, "INSERT INTO SiteSettings (Key, Value) VALUES (@k,@v) ON CONFLICT(Key) DO UPDATE SET Value=excluded.Value",
                ("k", key), ("v", value));
        }
    }

    public async Task SubscribeAsync(string email)
    {
        await using var conn = (System.Data.Common.DbConnection)await Open();
        var exists = Convert.ToInt32(await Ado.ScalarAsync(conn, "SELECT COUNT(*) FROM NewsletterSubscribers WHERE Email=@e", ("e", email)));
        if (exists == 0)
        {
            await Ado.ExecuteAsync(conn, "INSERT INTO NewsletterSubscribers (Email, SubscribedAt) VALUES (@e,@d)",
                ("e", email), ("d", DateTime.UtcNow.ToString("o")));
        }
    }

    public async Task<int> InsertOrderAsync(Order order)
    {
        await using var conn = (System.Data.Common.DbConnection)await Open();
        await Ado.ExecuteAsync(conn, """
            INSERT INTO Orders (OrderNumber, CustomerName, Email, Phone, Address, City, State, Pincode, Status, CreatedAt, Subtotal, Shipping, Total)
            VALUES (@n,@cn,@em,@ph,@ad,@ci,@st,@pin,@status,@dt,@sub,@ship,@tot)
            """,
            ("n", order.OrderNumber), ("cn", order.CustomerName), ("em", order.Email), ("ph", order.Phone),
            ("ad", order.Address), ("ci", order.City), ("st", order.State), ("pin", order.Pincode),
            ("status", order.Status), ("dt", order.CreatedAt.ToString("o")), ("sub", order.Subtotal),
            ("ship", order.Shipping), ("tot", order.Total));
        var id = Convert.ToInt32(await Ado.ScalarAsync(conn, _factory.LastInsertIdSql));
        foreach (var item in order.Items)
        {
            await Ado.ExecuteAsync(conn,
                "INSERT INTO OrderItems (OrderId, ProductId, ProductName, Quantity, UnitPrice) VALUES (@o,@p,@n,@q,@u)",
                ("o", id), ("p", item.ProductId), ("n", item.ProductName), ("q", item.Quantity), ("u", item.UnitPrice));
        }
        return id;
    }

    public async Task<Order?> GetOrderByNumberAsync(string number, string? email = null)
    {
        await using var conn = (System.Data.Common.DbConnection)await Open();
        var sql = "SELECT * FROM Orders WHERE OrderNumber=@n";
        var args = new List<(string, object?)> { ("n", number) };
        if (!string.IsNullOrWhiteSpace(email)) { sql += " AND Email=@e"; args.Add(("e", email)); }
        var order = await Ado.QuerySingleAsync(conn, sql, MapOrder, args.ToArray());
        if (order is null) return null;
        order.Items = await Ado.QueryAsync(conn, "SELECT * FROM OrderItems WHERE OrderId=@id", MapOrderItem, ("id", order.Id));
        return order;
    }

    public async Task<List<Order>> ListOrdersAsync()
    {
        await using var conn = (System.Data.Common.DbConnection)await Open();
        return await Ado.QueryAsync(conn, "SELECT * FROM Orders ORDER BY Id DESC", MapOrder);
    }

    public async Task<(int Categories, int SubCategories, int Products, int Orders)> DashboardCountsAsync()
    {
        await using var conn = (System.Data.Common.DbConnection)await Open();
        var c = Convert.ToInt32(await Ado.ScalarAsync(conn, "SELECT COUNT(*) FROM Categories"));
        var s = Convert.ToInt32(await Ado.ScalarAsync(conn, "SELECT COUNT(*) FROM SubCategories"));
        var p = Convert.ToInt32(await Ado.ScalarAsync(conn, "SELECT COUNT(*) FROM Products"));
        var o = Convert.ToInt32(await Ado.ScalarAsync(conn, "SELECT COUNT(*) FROM Orders"));
        return (c, s, p, o);
    }
}
