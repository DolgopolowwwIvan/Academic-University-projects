using Microsoft.EntityFrameworkCore;
using PriceListManager.API.Data;
using PriceListManager.API.Models;
using PriceListManager.API.Models.DTOs;

namespace PriceListManager.API.Services;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAllAsync();
    Task<UserDto?> GetByIdAsync(int id);
    Task<(UserDto? User, string? Error, string? TempPassword)> CreateAdminAsync(CreateAdminRequest request, int createdById);
    Task<(bool Success, string? Error)> UpdateRoleAsync(int id, string role);
    Task<IEnumerable<UserDto>> SearchAsync(string query);
}

public class UserService : IUserService
{
    private readonly AppDbContext _context;

    public UserService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<UserDto>> GetAllAsync()
    {
        return await _context.Users
            .OrderBy(u => u.Login)
            .Select(u => new UserDto
            {
                Id = u.Id,
                Email = u.Email,
                Login = u.Login,
                Role = u.Role.ToString(),
                CreatedAt = u.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<UserDto?> GetByIdAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return null;

        return new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            Login = user.Login,
            Role = user.Role.ToString(),
            CreatedAt = user.CreatedAt
        };
    }

    public async Task<(UserDto? User, string? Error, string? TempPassword)> CreateAdminAsync(CreateAdminRequest request, int createdById)
    {
        // Check if email already exists
        if (await _context.Users.AnyAsync(u => u.Email.ToLower() == request.Email.ToLower()))
        {
            return (null, "Пользователь с таким email уже существует", null);
        }

        // Check if login already exists
        if (!string.IsNullOrWhiteSpace(request.Login))
        {
            if (await _context.Users.AnyAsync(u => u.Login.ToLower() == request.Login.ToLower()))
            {
                return (null, "Пользователь с таким логином уже существует", null);
            }
        }

        if (!Enum.TryParse<UserRole>(request.Role, out var role))
        {
            return (null, "Недопустимая роль", null);
        }

        // Generate temporary password
        var tempPassword = GenerateTempPassword();

        var user = new User
        {
            Email = request.Email,
            Login = request.Login ?? request.Email.Split('@')[0],
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(tempPassword),
            Role = role,
            CreatedBy = createdById
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return (new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            Login = user.Login,
            Role = user.Role.ToString(),
            CreatedAt = user.CreatedAt
        }, null, tempPassword);
    }

    public async Task<(bool Success, string? Error)> UpdateRoleAsync(int id, string role)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return (false, "Пользователь не найден");

        if (!Enum.TryParse<UserRole>(role, out var newRole))
        {
            return (false, "Недопустимая роль");
        }

        user.Role = newRole;
        await _context.SaveChangesAsync();
        return (true, null);
    }

    public async Task<IEnumerable<UserDto>> SearchAsync(string query)
    {
        if (string.IsNullOrWhiteSpace(query)) return await GetAllAsync();

        var queryLower = query.ToLower();
        return await _context.Users
            .Where(u => u.Login.ToLower().Contains(queryLower) || 
                       u.Email.ToLower().Contains(queryLower))
            .OrderBy(u => u.Login)
            .Select(u => new UserDto
            {
                Id = u.Id,
                Email = u.Email,
                Login = u.Login,
                Role = u.Role.ToString(),
                CreatedAt = u.CreatedAt
            })
            .Take(10)
            .ToListAsync();
    }

    private static string GenerateTempPassword()
    {
        const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghjkmnpqrstuvwxyz23456789";
        var random = new Random();
        return new string(Enumerable.Repeat(chars, 8)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}
