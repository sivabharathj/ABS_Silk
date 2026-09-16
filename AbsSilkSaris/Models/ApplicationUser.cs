using Microsoft.AspNetCore.Identity;

namespace AbsSilkSaris.Models;

public class ApplicationUser : IdentityUser
{
    public string? FullName { get; set; }
}
