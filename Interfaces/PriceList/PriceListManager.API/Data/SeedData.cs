using PriceListManager.API.Data;
using PriceListManager.API.Models;

namespace PriceListManager.API.Data;

public static class SeedData
{
    public static void Initialize(AppDbContext context, IConfiguration configuration)
    {
        // Create users with hashed passwords
        var users = new[]
        {
            new User
            {
                Email = "admin@example.com",
                Login = "admin",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                Role = UserRole.SuperAdmin
            },
            new User
            {
                Email = "manager@example.com",
                Login = "manager",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                Role = UserRole.Admin
            },
            new User
            {
                Email = "user@example.com",
                Login = "user",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                Role = UserRole.User
            }
        };

        context.Users.AddRange(users);
        context.SaveChanges();

        // Create categories
        var categories = new[]
        {
            new Category
            {
                Name = "Электроника",
                Description = "Бытовая техника и электроника",
                CreatedBy = 1
            },
            new Category
            {
                Name = "Одежда",
                Description = "Мужская и женская одежда",
                CreatedBy = 1
            },
            new Category
            {
                Name = "Книги",
                Description = "Художественная и учебная литература",
                CreatedBy = 1
            }
        };

        context.Categories.AddRange(categories);
        context.SaveChanges();

        // Create products
        var products = new[]
        {
            new Product
            {
                Name = "Смартфон XYZ",
                Article = "PH-001",
                Price = 29999.99m,
                Quantity = 15,
                CategoryId = 1,
                CreatedBy = 1
            },
            new Product
            {
                Name = "Ноутбук Pro",
                Article = "NB-002",
                Price = 54999.99m,
                Quantity = 8,
                CategoryId = 1,
                CreatedBy = 1
            },
            new Product
            {
                Name = "Планшет Mini",
                Article = "TB-003",
                Price = 19999.99m,
                Quantity = 12,
                CategoryId = 1,
                CreatedBy = 1
            },
            new Product
            {
                Name = "Фужерная рубашка",
                Article = "CL-001",
                Price = 2999.99m,
                Quantity = 50,
                CategoryId = 2,
                CreatedBy = 1
            },
            new Product
            {
                Name = "Джинсы классика",
                Article = "CL-002",
                Price = 3499.99m,
                Quantity = 25,
                CategoryId = 2,
                CreatedBy = 1
            },
            new Product
            {
                Name = "Война и мир",
                Article = "BK-001",
                Price = 899.99m,
                Quantity = 12,
                CategoryId = 3,
                CreatedBy = 1
            },
            new Product
            {
                Name = "Преступление и наказание",
                Article = "BK-002",
                Price = 599.99m,
                Quantity = 20,
                CategoryId = 3,
                CreatedBy = 1
            }
        };

        context.Products.AddRange(products);
        context.SaveChanges();
    }
}
