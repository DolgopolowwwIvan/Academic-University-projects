namespace PriceListManager.API.Models.DTOs;

public class ProductDto
{
    public int Id { get; set; }
    public string? Article { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class CreateProductRequest
{
    public int CategoryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Article { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}

public class UpdateProductRequest
{
    public string? Article { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public int CategoryId { get; set; }
}

public class MoveProductRequest
{
    public int TargetCategoryId { get; set; }
}

public class MoveBulkRequest
{
    public List<int> ProductIds { get; set; } = new();
    public int TargetCategoryId { get; set; }
}

public class ProductSearchRequest
{
    public string? Name { get; set; }
    public int? CategoryId { get; set; }
    public string? Article { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public bool? InStock { get; set; }
}
