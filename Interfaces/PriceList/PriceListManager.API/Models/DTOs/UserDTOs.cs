namespace PriceListManager.API.Models.DTOs;

public class UserDto
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Login { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class CreateAdminRequest
{
    public string Email { get; set; } = string.Empty;
    public string? Login { get; set; }
    public string Role { get; set; } = "Admin";
}

public class UpdateRoleRequest
{
    public string Role { get; set; } = string.Empty;
}
