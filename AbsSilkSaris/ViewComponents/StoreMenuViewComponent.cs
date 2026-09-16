using AbsSilkSaris.Data;
using AbsSilkSaris.Models;
using Microsoft.AspNetCore.Mvc;

namespace AbsSilkSaris.ViewComponents;

public class StoreMenuViewComponent : ViewComponent
{
    private readonly CatalogRepository _db;
    public StoreMenuViewComponent(CatalogRepository db) => _db = db;

    public async Task<IViewComponentResult> InvokeAsync(string view = "Default")
    {
        var menu = await _db.GetMenuAsync();
        return View(view, menu);
    }
}
