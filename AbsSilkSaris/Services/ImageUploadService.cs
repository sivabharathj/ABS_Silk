namespace AbsSilkSaris.Services;

public class ImageUploadService
{
    private readonly IWebHostEnvironment _env;
    private static readonly HashSet<string> Allowed = new(StringComparer.OrdinalIgnoreCase)
    {
        ".jpg", ".jpeg", ".png", ".webp", ".gif"
    };

    public ImageUploadService(IWebHostEnvironment env) => _env = env;

    public async Task<string?> SaveAsync(IFormFile? file, string folder)
    {
        if (file is null || file.Length == 0) return null;
        var ext = Path.GetExtension(file.FileName);
        if (!Allowed.Contains(ext)) throw new InvalidOperationException("Please upload a JPG, PNG, WEBP or GIF image.");
        var dir = Path.Combine(_env.WebRootPath, "uploads", folder);
        Directory.CreateDirectory(dir);
        var name = $"{Guid.NewGuid():N}{ext.ToLowerInvariant()}";
        var path = Path.Combine(dir, name);
        await using var stream = File.Create(path);
        await file.CopyToAsync(stream);
        return $"/uploads/{folder}/{name}";
    }
}
