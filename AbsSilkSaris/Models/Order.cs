using System.ComponentModel.DataAnnotations.Schema;

namespace AbsSilkSaris.Models;

public class Order
{
    public int Id { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string Pincode { get; set; } = string.Empty;
    public string Status { get; set; } = "Confirmed";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    [Column(TypeName = "decimal(18,2)")]
    public decimal Subtotal { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal Shipping { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal Total { get; set; }
    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
}

public class OrderItem
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public Order? Order { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal UnitPrice { get; set; }
}
