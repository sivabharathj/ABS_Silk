namespace AbsSilkSaris.Models;

public class Review
{
    public int Id { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string PhotoUrl { get; set; } = string.Empty;
    public int Stars { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public int? ProductId { get; set; }
}
