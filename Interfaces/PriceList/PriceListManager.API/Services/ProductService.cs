using Microsoft.EntityFrameworkCore;
using PriceListManager.API.Data;
using PriceListManager.API.Models;
using PriceListManager.API.Models.DTOs;

namespace PriceListManager.API.Services;

public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetAllAsync(int? categoryId = null, string? search = null);
    Task<ProductDto?> GetByIdAsync(int id);
    Task<IEnumerable<ProductDto>> SearchAsync(ProductSearchRequest request);
    Task<(ProductDto? Product, string? Error)> CreateAsync(CreateProductRequest request, int userId);
    Task<(bool Success, string? Error)> UpdateAsync(int id, UpdateProductRequest request);
    Task<bool> DeleteAsync(int id);
    Task<(ProductDto? Product, string? Error)> MoveAsync(int id, int targetCategoryId, int userId);
    Task<(int MovedCount, List<string> Errors)> MoveBulkAsync(MoveBulkRequest request, int userId);
    Task<IEnumerable<ProductMovementDto>> GetMovementsAsync(int productId);
    Task<bool> ExistsAsync(string name, int categoryId, int? excludeId = null);
    Task<bool> ArticleExistsAsync(string? article, int? excludeId = null);
}

public class ProductService : IProductService
{
    private readonly AppDbContext _context;

    public ProductService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ProductDto>> GetAllAsync(int? categoryId = null, string? search = null)
    {
        var query = _context.Products
            .Include(p => p.Category)
            .AsQueryable();

        if (categoryId.HasValue)
        {
            query = query.Where(p => p.CategoryId == categoryId.Value);
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchLower = search.ToLower();
            query = query.Where(p =>
                p.Name.ToLower().Contains(searchLower) ||
                (p.Article != null && p.Article.ToLower().Contains(searchLower)));
        }

        return await query
            .OrderBy(p => p.Name)
            .Select(p => MapToDto(p))
            .ToListAsync();
    }

    public async Task<ProductDto?> GetByIdAsync(int id)
    {
        var product = await _context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);

        return product == null ? null : MapToDto(product);
    }

    public async Task<IEnumerable<ProductDto>> SearchAsync(ProductSearchRequest request)
    {
        var query = _context.Products
            .Include(p => p.Category)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            var nameLower = request.Name.ToLower();
            query = query.Where(p => p.Name.ToLower().Contains(nameLower));
        }

        if (request.CategoryId.HasValue)
        {
            query = query.Where(p => p.CategoryId == request.CategoryId.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.Article))
        {
            query = query.Where(p => p.Article == request.Article);
        }

        if (request.MinPrice.HasValue)
        {
            query = query.Where(p => p.Price >= request.MinPrice.Value);
        }

        if (request.MaxPrice.HasValue)
        {
            query = query.Where(p => p.Price <= request.MaxPrice.Value);
        }

        if (request.InStock.HasValue)
        {
            if (request.InStock.Value)
            {
                query = query.Where(p => p.Quantity > 0);
            }
            else
            {
                query = query.Where(p => p.Quantity == 0);
            }
        }

        return await query
            .OrderBy(p => p.Name)
            .Select(p => MapToDto(p))
            .ToListAsync();
    }

    public async Task<(ProductDto? Product, string? Error)> CreateAsync(CreateProductRequest request, int userId)
    {
        if (await ExistsAsync(request.Name, request.CategoryId))
        {
            return (null, "Товар с таким названием уже существует в данной категории");
        }

        if (!string.IsNullOrWhiteSpace(request.Article) && await ArticleExistsAsync(request.Article))
        {
            return (null, "Товар с таким артикулом уже существует");
        }

        var product = new Product
        {
            Name = request.Name,
            Article = request.Article,
            Description = request.Description,
            Price = request.Price,
            Quantity = request.Quantity,
            CategoryId = request.CategoryId,
            CreatedBy = userId
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        var createdProduct = await _context.Products
            .Include(p => p.Category)
            .FirstAsync(p => p.Id == product.Id);

        return (MapToDto(createdProduct), null);
    }

    public async Task<(bool Success, string? Error)> UpdateAsync(int id, UpdateProductRequest request)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return (false, "Товар не найден");

        if (await ExistsAsync(request.Name, request.CategoryId, id))
        {
            return (false, "Товар с таким названием уже существует в данной категории");
        }

        if (!string.IsNullOrWhiteSpace(request.Article) && await ArticleExistsAsync(request.Article, id))
        {
            return (false, "Товар с таким артикулом уже существует");
        }

        product.Name = request.Name;
        product.Article = request.Article;
        product.Description = request.Description;
        product.Price = request.Price;
        product.Quantity = request.Quantity;
        product.CategoryId = request.CategoryId;

        await _context.SaveChangesAsync();
        return (true, null);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return false;

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<(ProductDto? Product, string? Error)> MoveAsync(int id, int targetCategoryId, int userId)
    {
        var product = await _context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null) return (null, "Товар не найден");

        var targetCategory = await _context.Categories.FindAsync(targetCategoryId);
        if (targetCategory == null) return (null, "Категория не найдена");

        if (product.CategoryId == targetCategoryId)
        {
            return (null, "Товар уже находится в выбранной категории");
        }

        var movement = new ProductMovement
        {
            ProductId = product.Id,
            FromCategoryId = product.CategoryId,
            ToCategoryId = targetCategoryId,
            MovedBy = userId
        };

        product.CategoryId = targetCategoryId;
        _context.ProductMovements.Add(movement);
        await _context.SaveChangesAsync();

        var updatedProduct = await _context.Products
            .Include(p => p.Category)
            .FirstAsync(p => p.Id == id);

        return (MapToDto(updatedProduct), null);
    }

    public async Task<(int MovedCount, List<string> Errors)> MoveBulkAsync(MoveBulkRequest request, int userId)
    {
        var errors = new List<string>();
        var movedCount = 0;

        foreach (var productId in request.ProductIds)
        {
            var result = await MoveAsync(productId, request.TargetCategoryId, userId);
            if (result.Error != null)
            {
                errors.Add($"Товар {productId}: {result.Error}");
            }
            else
            {
                movedCount++;
            }
        }

        return (movedCount, errors);
    }

    public async Task<IEnumerable<ProductMovementDto>> GetMovementsAsync(int productId)
    {
        return await _context.ProductMovements
            .Include(m => m.FromCategory)
            .Include(m => m.ToCategory)
            .Include(m => m.MovedByUser)
            .Where(m => m.ProductId == productId)
            .OrderByDescending(m => m.MovedAt)
            .Select(m => new ProductMovementDto
            {
                Id = m.Id,
                ProductId = m.ProductId,
                FromCategoryName = m.FromCategory != null ? m.FromCategory.Name : null,
                ToCategoryName = m.ToCategory != null ? m.ToCategory.Name : "",
                MovedAt = m.MovedAt,
                MovedByLogin = m.MovedByUser != null ? m.MovedByUser.Login : null
            })
            .ToListAsync();
    }

    public async Task<bool> ExistsAsync(string name, int categoryId, int? excludeId = null)
    {
        var query = _context.Products
            .Where(p => p.Name.ToLower() == name.ToLower() && p.CategoryId == categoryId);
        
        if (excludeId.HasValue)
        {
            query = query.Where(p => p.Id != excludeId.Value);
        }
        
        return await query.AnyAsync();
    }

    public async Task<bool> ArticleExistsAsync(string? article, int? excludeId = null)
    {
        if (string.IsNullOrWhiteSpace(article)) return false;
        
        var query = _context.Products.Where(p => p.Article == article);
        
        if (excludeId.HasValue)
        {
            query = query.Where(p => p.Id != excludeId.Value);
        }
        
        return await query.AnyAsync();
    }

    private static ProductDto MapToDto(Product product) => new()
    {
        Id = product.Id,
        Article = product.Article,
        Name = product.Name,
        Description = product.Description,
        Price = product.Price,
        Quantity = product.Quantity,
        CategoryId = product.CategoryId,
        CategoryName = product.Category?.Name ?? "",
        CreatedAt = product.CreatedAt
    };
}

public class ProductMovementDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string? FromCategoryName { get; set; }
    public string ToCategoryName { get; set; } = string.Empty;
    public DateTime MovedAt { get; set; }
    public string? MovedByLogin { get; set; }
}
