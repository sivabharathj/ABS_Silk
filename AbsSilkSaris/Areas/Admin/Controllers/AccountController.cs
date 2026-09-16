using System.Security.Claims;
using AbsSilkSaris.Data;
using AbsSilkSaris.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AbsSilkSaris.Areas.Admin.Controllers;

[Area("Admin")]
public class AccountController : Controller
{
    private readonly CatalogRepository _db;
    public AccountController(CatalogRepository db) => _db = db;

    [AllowAnonymous]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewBag.ReturnUrl = returnUrl;
        ViewData["Title"] = "Admin login";
        return View();
    }

    [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(string username, string password, string? returnUrl = null)
    {
        var admin = await _db.GetAdminByUsernameAsync(username?.Trim() ?? "");
        if (admin is null || !PasswordUtil.Verify(password ?? "", admin.PasswordHash))
        {
            ViewBag.Error = "Invalid username or password.";
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, admin.Id.ToString()),
            new(ClaimTypes.Name, admin.Username),
            new(ClaimTypes.GivenName, admin.FullName),
            new(ClaimTypes.Role, admin.Role)
        };
        var id = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl)) return Redirect(returnUrl);
        return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home", new { area = "" });
    }
}
