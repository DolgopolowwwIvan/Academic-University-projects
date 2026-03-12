namespace PriceListManager.API.Models;

public class ProductMovement
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public Product? Product { get; set; }
    public int? FromCategoryId { get; set; }
    public Category? FromCategory { get; set; }
    public int ToCategoryId { get; set; }
    public Category? ToCategory { get; set; }
    public DateTime MovedAt { get; set; } = DateTime.UtcNow;
    public int? MovedBy { get; set; }
    public User? MovedByUser { get; set; }
}
