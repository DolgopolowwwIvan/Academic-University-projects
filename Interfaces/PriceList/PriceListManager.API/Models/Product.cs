namespace PriceListManager.API.Models;

public class Product
{
    public int Id { get; set; }
    public string? Article { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public int CreatedBy { get; set; }
    public User? Creator { get; set; }
    public ICollection<ProductMovement> Movements { get; set; } = new List<ProductMovement>();
}
