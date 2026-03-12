using Microsoft.EntityFrameworkCore;
using PriceListManager.API.Data;
using PriceListManager.API.Models;
using PriceListManager.API.Models.DTOs;

namespace PriceListManager.API.Services;

public interface ICategoryService
{
    Task<IEnumerable<CategoryDto>> GetAllAsync();
    Task<CategoryDto?> GetByIdAsync(int id);
    Task<(CategoryDto? Category, string? Error)> CreateAsync(CreateCategoryRequest request, int userId);
    Task<(bool Success, string? Error)> UpdateAsync(int id, UpdateCategoryRequest request);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsAsync(string name, int? excludeId = null);
}

public class CategoryService : ICategoryService
{
    private readonly AppDbContext _context;

    public CategoryService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CategoryDto>> GetAllAsync()
    {
        return await _context.Categories
            .Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                CreatedAt = c.CreatedAt,
                ProductsCount = c.Products.Count
            })
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    public async Task<CategoryDto?> GetByIdAsync(int id)
    {
        var category = await _context.Categories
            .Include(c => c.Products)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (category == null) return null;

        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            CreatedAt = category.CreatedAt,
            ProductsCount = category.Products.Count
        };
    }

    public async Task<(CategoryDto? Category, string? Error)> CreateAsync(CreateCategoryRequest request, int userId)
    {
        if (await ExistsAsync(request.Name))
        {
            return (null, "Категория с таким названием уже существует");
        }

        var category = new Category
        {
            Name = request.Name,
            Description = request.Description,
            CreatedBy = userId
        };

        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        return (new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            CreatedAt = category.CreatedAt,
            ProductsCount = 0
        }, null);
    }

    public async Task<(bool Success, string? Error)> UpdateAsync(int id, UpdateCategoryRequest request)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null) return (false, "Категория не найдена");

        if (await ExistsAsync(request.Name, id))
        {
            return (false, "Категория с таким названием уже существует");
        }

        category.Name = request.Name;
        category.Description = request.Description;

        await _context.SaveChangesAsync();
        return (true, null);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var category = await _context.Categories
            .Include(c => c.Products)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (category == null) return false;
        if (category.Products.Any()) return false;

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(string name, int? excludeId = null)
    {
        var query = _context.Categories.Where(c => c.Name.ToLower() == name.ToLower());
        if (excludeId.HasValue)
        {
            query = query.Where(c => c.Id != excludeId.Value);
        }
        return await query.AnyAsync();
    }
}
