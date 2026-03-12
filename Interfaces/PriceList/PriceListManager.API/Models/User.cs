namespace PriceListManager.API.Models;

public enum UserRole
{
    User,
    Admin,
    SuperAdmin
}

public class User
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Login { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public UserRole Role { get; set; } = UserRole.User;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public int? CreatedBy { get; set; }
    public User? Creator { get; set; }
}
